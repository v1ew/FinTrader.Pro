﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using FinTrader.Pro.Bonds.Extensions;
using FinTrader.Pro.Bonds.Helpers;
using FinTrader.Pro.Contracts;
using FinTrader.Pro.Contracts.Bonds;
using FinTrader.Pro.Contracts.Enums;
using FinTrader.Pro.DB.Models;
using FinTrader.Pro.DB.Repositories;
using FinTrader.Pro.Iss.Columns;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinTrader.Pro.Bonds
{
    public class BondsService : IBondsService
    {
        private readonly IIssBondsRepository issBondsRepository;
        private readonly IFinTraderRepository traderRepository;
        private readonly ILogger<BondsService> logger;
        // TODO: перенести в параметры
        private const int BONDS_NUM = 6;

        public BondsService(IIssBondsRepository issBondsRepo, IFinTraderRepository traderRepo, ILogger<BondsService> logger)
        {
            issBondsRepository = issBondsRepo;
            traderRepository = traderRepo;
            this.logger = logger;
        }

        public async Task<BondSet> SelectBondsAsync(BondsPickerParams filter)
        {
            var bonds = traderRepository.Bonds.Where(b => !b.Discarded);
            
            if (filter.IsIncludedCorporate != filter.IsIncludedFederal)
            {
                if (filter.IsIncludedCorporate) // выбираем только корпоративные
                {
                    bonds = bonds.Where(b => b.SecType != "3");
                }
                else // только ОФЗ
                {
                    bonds = bonds.Where(b => b.SecType == "3");
                }
            }

            switch (filter.BondsClass)
            {
                case BondClass.MostLiquid:
                    bonds = bonds.OrderByDescending(b => b.CouponPercent);
                    break;
                case BondClass.MostProfitable:
                    bonds = bonds.OrderByDescending(b => b.CouponPercent);
                    break;
                case BondClass.ByRepaymentDate:
                    if (filter.RepaymentDate.HasValue)
                    {
                        bonds = bonds.OrderByDescending(b => b.MatDate).Where(b => b.MatDate.Value.CompareTo(filter.RepaymentDate.Value) <= 0);
                    }
                    break;
                case BondClass.FarthestRepaynment:
                    bonds = bonds.OrderByDescending(b => b.MatDate);
                    break;
            }

            var bondIds = await bonds//.Distinct(new BondsComparer())//.GroupBy(b => b.SecId).Select(g => g.First())
                .OrderByDescending(b => b.CouponPercent)
                .Select(b => b.SecId)
                .Distinct()
                .Take(BONDS_NUM).ToArrayAsync();
            var selectedBonds = bonds
                .Where(b => bondIds.Contains(b.SecId))
                .Select(b => new SelectedBond
            {
                ShortName = b.ShortName,
                MatDate = b.MatDate.Value,
                CouponValue = b.CouponValue.Value,
                AmountToBye = 10,
                Sum = b.FaceValue.Value * 10,
                Isin = b.Isin
            });

            return new BondSet
            {
                Bonds = await selectedBonds.ToArrayAsync(),
                Coupons = await GetCouponsAsync(selectedBonds.ToDictionary(b => b.Isin, b => b.ShortName))
            };
        }

        /// <summary>
        /// Скачивает данные по облигациям и сохраняет в БД
        /// </summary>
        /// <returns></returns>
        public async Task UpdateBondsAsync()
        {
            var bonds = await issBondsRepository.LoadBondsAsync();
            var newBonds = new List<Bond>();
            var changedBonds = new List<Bond>();
            foreach(var bond in bonds)
            {
                // Если бумагу исключаем, то сохраняем об этом информацию в БД, чтобы в будущем ее не проверять лишний раз
                var discarded = false;
                
                var existingBond = traderRepository.Bonds.FirstOrDefault(b => b.SecId == bond[BondsColumnNames.SecId]);
                if (existingBond?.Discarded ?? false) continue;

                bool isQual = false;
                int emitter = 0;
                (isQual, emitter) = await GetBondInfo(bond[BondsColumnNames.SecId]);
                
                if (NullableValue.TryDoubleParse(bond[BondsColumnNames.CouponValue]) == 0 // Если размер купона - 0, то исключаем ее из своего списка
                    || await HasAmortizations(bond[BondsColumnNames.SecId]) // Не берем бумаги с амортизацией
                    || isQual) // Бумага для квалифицированных инвесторов
                {
                    discarded = true;
                }

                if (existingBond == null)
                {
                    var bondLoaded = MakeBond(bond, emitter, discarded);
                    newBonds.Add(bondLoaded);
                    //TODO: save log - record created
                }
                else
                {
                    if (haveChanges(existingBond, bond))
                    {
                        changedBonds.Add(existingBond);
                        //TODO: update record, save log
                    }
                }
            }

            if (newBonds.Any())
            {
                await traderRepository.AddBondsRangeAsync(newBonds.ToArray());
            }

            if (changedBonds.Any())
            {
                await traderRepository.UpdateBondsRangeAsync(changedBonds);
            }
        }

        public async Task UpdateMarketDataAsync()
        {
            var now = DateTime.Now;
            var marketData = await issBondsRepository.LoadBondsMarketDataAsync();
            var newRecords = new List<MarketRecord>();
            var changedRecords = new List<MarketRecord>();
            
            foreach (var mr in marketData)
            {
                if (!traderRepository.Bonds.Any(b => !b.Discarded && b.SecId == mr[BondsColumnNames.SecId]))
                    continue;
                
                var record = new MarketRecord
                {
                    SecId = mr[BondsColumnNames.SecId],
                    TradeDate = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0),
                    Value = NullableValue.TryIntParse(mr[BondsColumnNames.ValToday]),
                    Volume = NullableValue.TryIntParse(mr[BondsColumnNames.VolToday]),
                    ChangedTime = now
                };
                
                if (traderRepository.MarketRecords.Any(m => m.SecId == record.SecId && m.TradeDate.CompareTo(record.TradeDate) == 0))
                {
                    changedRecords.Add(record);
                }
                else
                {
                    newRecords.Add(record);
                }
            }

            if (newRecords.Any())
            {
                await traderRepository.AddMarketRecordsRangeAsync(newRecords.ToArray());
            }

            if (changedRecords.Any())
            {
                await traderRepository.UpdateMarketRecordsRangeAsync(newRecords);
            }
        }
        
        public async Task UpdateCouponsAsync()
        {
            var changedBonds = new List<string>();
            var now = DateTime.Now;
            
            var bondSecIds = traderRepository.Bonds
                .Where(b => !b.Discarded)
                .Select(b => b.SecId).ToArray();
            foreach (var secId in bondSecIds)
            {
                if (!await UpdateBondCouponsAsync(secId, now))
                {
                    changedBonds.Add(secId);
                }
            }

            if (changedBonds.Any())
            {
                var discardedBonds = await traderRepository.Bonds.Where(b => changedBonds.Contains(b.SecId))
                    .ToListAsync();
                discardedBonds.ForEach(b => b.Discarded = true);
                await traderRepository.UpdateBondsRangeAsync(discardedBonds);
            }
        }
        
        public async Task DiscardWrongBondsAsync()
        {
            var boardIds = new[]
            {
                "TQCB",
                "TQOB",
            };

            var secTypes = new[]
            {
                "3", // ОФЗ
                "6", // Корп
                "8" // Биржевые
            };
            Recorder.Start();
            var badIsins = await traderRepository.Bonds
                .Where(b => !b.Discarded && (!boardIds.Contains(b.BoardId) || b.FaceUnit != "SUR"))
                .Select(b => b.Isin).Distinct().ToListAsync();

            var wrongBonds = await traderRepository.Bonds
                .Where(b => !b.Discarded && (badIsins.Contains(b.Isin)
                                             || b.SecType == null
                                             || !secTypes.Contains(b.SecType)  // оставляем только ОФЗ, корп. и биржевые
                                             || (!b.MatDate.HasValue && !b.OfferDate.HasValue)
                                             || b.CouponPercent == null 
                                             || b.CouponPercent > 15))
                .ToListAsync();

            if (wrongBonds.Any())
            {
                wrongBonds.ForEach(b => b.Discarded = true);
                await traderRepository.UpdateBondsRangeAsync(wrongBonds);
            }

            Recorder.Stop(logger);
        }

        public async Task CheckCoupons()
        {
            var bonds = await traderRepository.Bonds.Where(b => !b.Discarded).ToArrayAsync();
            var badCouponsBonds = new List<Bond>();
            foreach (var bond in bonds)
            {
                if (!await CheckBondCoupons(bond)) 
                    badCouponsBonds.Add(bond);
            }

            if (badCouponsBonds.Any())
            {
                badCouponsBonds.ForEach(b => b.Discarded = true);
                await traderRepository.UpdateBondsRangeAsync(badCouponsBonds);
            }
        }

        private async Task<bool> CheckBondCoupons(Bond bond)
        {
            logger.Log(LogLevel.Debug, $"Check coupons for bond {bond.SecId}");
            var result = true;
            var coupons = await traderRepository.Coupons
                .Where(c => c.Isin == bond.Isin)
                .OrderBy(c => c.CouponDate)
                .ToArrayAsync();
            var startDate = coupons.Length > 0 ? coupons[0]?.CouponDate : null;
            var endDate = bond.OfferDate ?? bond.MatDate;
            if (startDate.HasValue && endDate.HasValue && bond.CouponPeriod.HasValue)
            {
                var i = 0;
                while (i < coupons.Length && startDate.Value.CompareTo(endDate.Value) > 0 && coupons[i].CouponDate.HasValue)
                {
                    if (startDate.Value.CompareTo(coupons[i++].CouponDate.Value) != 0)
                    {
                        result = false;
                        break;
                    }

                    startDate = startDate.Value.AddDays(bond.CouponPeriod.Value);
                }
            }
            else
            {
                result = false;
            }

            return result;
        }
        
        /// <summary>
        /// Получить массив купонов по массиву номеров isin
        /// Для вывода на странице
        /// </summary>
        /// <param name="bonds">Dictionary with key = isin, value = shortName</param>
        /// <returns></returns>
        private async Task<SelectedCoupon[]> GetCouponsAsync(IDictionary<string, string> bonds)
        {
            var today = DateTime.Now;
            return await traderRepository.Coupons
                .Where(c => bonds.Keys.Contains(c.Isin) 
                            && c.CouponDate.HasValue 
                            && c.CouponDate.Value.CompareTo(today) >= 0)
                .Select(c => new SelectedCoupon
                {
                    ShortName = bonds[c.Isin],
                    Isin = c.Isin,
                    Date = c.CouponDate ?? DateTime.MinValue,
                    Value = c.Value ?? 0,
                    Comment = ""
                })
                .OrderBy(sc => sc.Date)
                .ToArrayAsync();
        }

        /// <summary>
        /// Проверяет, есть ли амортизация
        /// </summary>
        /// <param name="secId"></param>
        /// <returns></returns>
        private async Task<bool> HasAmortizations(string secId)
        {
            try
            {
                var amortizations = await issBondsRepository.LoadAmortizationsAsync(secId);
                return amortizations.Any(a => a["data_source"] == "amortization");
            }
            catch (Exception)
            {
                logger.Log(LogLevel.Error, $"Error receiving amortization for {secId}");
            }

            return false;
        }

        private async Task<(bool, int)> GetBondInfo(string secId)
        {
            bool res1 = false;
            int res2 = 0;

            try
            {
                var bondInfo = await issBondsRepository.LoadBondsInfoAsync(secId);
                if (bondInfo != null)
                {
                    var isQual = bondInfo.Where(i => i["name"] == "ISQUALIFIEDINVESTORS").FirstOrDefault();
                    bool.TryParse(isQual["value"], out res1);

                    var emitter = bondInfo.Where(i => i["name"] == "EMITTER_ID").FirstOrDefault();
                    int.TryParse(emitter["value"], out res2);
                }
            }
            catch (Exception)
            {
                logger.Log(LogLevel.Error, $"Error by receiving bond info for {secId}");
            }

            return (res1, res2);
        }

        /// <summary>
        /// Создать облигацию из словаря данных
        /// </summary>
        /// <param name="bond"></param>
        /// <param name="emitter"></param>
        /// <returns></returns>
        private Bond MakeBond(Dictionary<string, string> bond, int emitter, bool discarded = false)
        {
            var bondLoaded = new Bond
            {
                SecId = bond[BondsColumnNames.SecId],
                BoardId = bond[BondsColumnNames.BoardId],
                Discarded = discarded,
                ShortName = bond[BondsColumnNames.ShortName],
                CouponValue = NullableValue.TryDoubleParse(bond[BondsColumnNames.CouponValue]),
                LotSize = NullableValue.TryIntParse(bond[BondsColumnNames.LotSize]),
                FaceValue = NullableValue.TryDoubleParse(bond[BondsColumnNames.FaceValue]),
                Status = bond[BondsColumnNames.Status],
                MatDate = NullableValue.TryDateParse(bond[BondsColumnNames.MatDate]),
                CouponPeriod = NullableValue.TryIntParse(bond[BondsColumnNames.CouponPeriod]),
                IssueSize = NullableValue.TryLongParse(bond[BondsColumnNames.IssueSize]),
                SecName = bond[BondsColumnNames.SecName],
                FaceUnit = bond[BondsColumnNames.FaceUnit],
                CurrencyId = bond[BondsColumnNames.CurrencyId],
                Isin = bond[BondsColumnNames.Isin],
                SecType = bond[BondsColumnNames.SecType],
                CouponPercent = NullableValue.TryDoubleParse(bond[BondsColumnNames.CouponPercent]),
                OfferDate = NullableValue.TryDateParse(bond[BondsColumnNames.OfferDate]),
                LotValue = NullableValue.TryDoubleParse(bond[BondsColumnNames.LotValue]),
                EmitterId = emitter,
            };

            return bondLoaded;
        }

        private Coupon MakeCoupon(Dictionary<string, string> coupon, DateTime couponDate)
        {
            var result = new Coupon
            {
                Isin = coupon[CouponsColumnNames.Isin],
                CouponDate = couponDate,
                RecordDate = NullableValue.TryDateParse(coupon[CouponsColumnNames.RecordDate]),
                StartDate = NullableValue.TryDateParse(coupon[CouponsColumnNames.StartDate]),
                InitialFaceValue = NullableValue.TryDoubleParse(coupon[CouponsColumnNames.InitialFaceValue]),
                FaceValue = NullableValue.TryDoubleParse(coupon[CouponsColumnNames.FaceValue]),
                FaceUnit = coupon[CouponsColumnNames.FaceUnit],
                Value = NullableValue.TryDoubleParse(coupon[CouponsColumnNames.Value]),
                ValuePrc = NullableValue.TryDoubleParse(coupon[CouponsColumnNames.ValuePrc]),
                ValueRub = NullableValue.TryDoubleParse(coupon[CouponsColumnNames.ValueRub]),
            };

            return result;
        }
        
        /// <summary>
        /// Алгоритм обновления:
        /// Получить данные с сайта биржи
        /// Проверить, есть ли амортизация: да - деактивировать бумагу
        /// Если дата амортизации совпадает с датой с датой погашения, то все ок 
        /// </summary>
        /// <returns>true - если все ок, иначе - false</returns>
        private async Task<bool> UpdateBondCouponsAsync(string secId, DateTime now)
        {
            // TODO: Добавить проверку на наличие всех купонов до даты погашения, либо оферты
            var coupons = await issBondsRepository.LoadCouponsAsync(secId);
            if (!coupons.Any())
                return false;

            var newCoupons = new List<Coupon>();
            var oldCoupons = new List<Coupon>();
            foreach (var coupon in coupons)
            {
                if (coupon[CouponsColumnNames.Value] == null) continue;
                
                var couponDate = NullableValue.TryDateParse(coupon[CouponsColumnNames.CouponDate]);
                // Проверяем актуальность выплаты купона по дате
                if (couponDate.HasValue && couponDate.Value.CompareTo(now) >= 0)
                {
                    var newCoupon = MakeCoupon(coupon, couponDate.Value);

                    var oldCoupon = traderRepository.Coupons
                        .FirstOrDefault(c => c.Isin == newCoupon.Isin && c.CouponDate.Value.CompareTo(couponDate.Value) == 0);
                    if (oldCoupon != null)
                    {
                        if (haveChanges(oldCoupon, newCoupon))
                        {
                            oldCoupons.Add(oldCoupon);
                        }
                    } else
                    {
                        newCoupons.Add(newCoupon);
                    }
                }
            }

            if (newCoupons.Any())
            {
                await traderRepository.AddCouponsRangeAsync(newCoupons.ToArray());
            }

            if (oldCoupons.Any())
            {
                await traderRepository.UpdateCouponsRangeAsync(oldCoupons);
            }

            return true;
        }

        /// <summary>
        /// Сравнение полученных данных с сайта биржи с данными в БД
        /// Если есть различия, данные обновляются
        /// Можно добавлять больше параметров для сравнения
        /// </summary>
        /// <param name="bondLoaded">Загруженные данные по облигации</param>
        /// <returns>true, если есть отличия</returns>
        private bool haveChanges(Bond bond, Dictionary<string, string> bondLoaded)
        {
            bool result = false;
            if (bondLoaded[BondsColumnNames.Status] != bond.Status)
            {
                bond.Status = bondLoaded[BondsColumnNames.Status];
                result = true;
            }
            var accruedInt = NullableValue.TryDoubleParse(bondLoaded[BondsColumnNames.AccruedInt]);
            if (accruedInt != bond.AccruedInt)
            {
                bond.AccruedInt = accruedInt;
                result = true;
            }
            var issueSize = NullableValue.TryLongParse(bondLoaded[BondsColumnNames.IssueSize]);
            if (issueSize != bond.IssueSize)
            {
                bond.IssueSize = issueSize;
                result = true;
            }
            var couponValue = NullableValue.TryDoubleParse(bondLoaded[BondsColumnNames.CouponValue]);
            if (couponValue != bond.CouponValue)
            {
                bond.CouponValue = couponValue;
                result = true;
            }
            var couponPercent = NullableValue.TryDoubleParse(bondLoaded[BondsColumnNames.CouponPercent]);
            if (couponPercent != bond.CouponPercent)
            {
                bond.CouponPercent = couponPercent;
                result = true;
            }

            return result;
        }

        private bool haveChanges(Coupon oldC, Coupon newC)
        {
            bool result = false;

            if (oldC.FaceValue != newC.FaceValue)
            {
                oldC.FaceValue = newC.FaceValue;
                result = true;
            }

            if (oldC.Value != newC.Value)
            {
                oldC.Value = newC.Value;
                oldC.ValueRub = newC.ValueRub;
                oldC.ValuePrc = newC.ValuePrc;
                result = true;
            }
            
            return result;
        }
    }
}
