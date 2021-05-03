using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Получить массив купонов по массиву номеров isin
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
                // Не берем бумаги с амортизацией
                if (await HasAmortizations(bond[BondsColumnNames.SecId])) continue;

                bool isQual = false;
                int emitter = 0;
                (isQual, emitter) = await GetBondInfo(bond[BondsColumnNames.SecId]);
                if (isQual) continue;
                
                var bondLoaded = new Bond
                {
                    SecId = bond[BondsColumnNames.SecId],
                    BoardId = bond[BondsColumnNames.BoardId],
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
                
                if (!traderRepository.Bonds.Any(b => b.SecId == bond[BondsColumnNames.SecId]))
                {
                    newBonds.Add(bondLoaded);
                    //TODO: save log - record created
                }
                else
                {
                    if (haveChanges())
                    {
                        changedBonds.Add(bondLoaded);
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
                await traderRepository.UpdateBondsRangeAsync(changedBonds.ToArray());
            }
        }

        public async Task UpdateCouponsAsync()
        {
            var changedBonds = new List<DB.Models.Bond>();
            var now = DateTime.Now;
            foreach (var bond in traderRepository.Bonds.Where(b => !b.Discarded))
            {
                if (!await UpdateBondCouponsAsync(bond.SecId, now))
                {
                    bond.Discarded = true;
                    changedBonds.Add(bond);
                }
            }

            if (changedBonds.Any())
            {
                await traderRepository.UpdateBondsRangeAsync(changedBonds.ToArray());
            }
        }

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
        /// Алгоритм обновления:
        /// Получить данные с сайта биржи
        /// Проверить, есть ли амортизация: да - деактивировать бумагу
        /// Если дата амортизации совпадает с датой с датой погашения, то все ок 
        /// </summary>
        /// <returns>true - если все ок, иначе - false</returns>
        private async Task<bool> UpdateBondCouponsAsync(string secId, DateTime now)
        {
            var coupons = await issBondsRepository.LoadCouponsAsync(secId);
            if (!coupons.Any())
                return false;

            var newCoupons = new List<DB.Models.Coupon>();
            foreach (var coupon in coupons)
            {
                var couponDate = NullableValue.TryDateParse(coupon[CouponsColumnNames.CouponDate]);
                // Проверяем актуальность выплаты купона по дате
                if (couponDate.HasValue && couponDate.Value.CompareTo(now) >= 0)
                {
                    newCoupons.Add(new DB.Models.Coupon
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
                    });
                }
            }

            await traderRepository.AddCouponsRangeAsync(newCoupons.ToArray());

            return true;
        }

        private bool haveChanges()
        {
            return true;
        }
        
        public async Task DiscardWrongBondsAsync()
        {
            var badBoardIds = new[]
            {
                "TQIR", // Сектор ПИР Московской биржи (ПИР — повышенный инвестиционный риск)
                "TQRD",
            };
            Recorder.Start();
            // Убираем все с амортизацией
            // var wrongIsins = (from c in traderRepository.Coupons
            //                   where c.InitialFaceValue > c.FaceValue
            //                   select c.Isin).Distinct();
            var wrongIsins = await traderRepository.Coupons
                .Where(c => c.InitialFaceValue > c.FaceValue)
                .Select(c => c.Isin).Distinct().ToListAsync();
            var badIsins = await traderRepository.Bonds
                .Where(b => badBoardIds.Contains(b.BoardId))
                .Select(b => b.Isin).Distinct().ToListAsync();

            var wrongBonds = await traderRepository.Bonds
                .Where(b => !b.Discarded && (wrongIsins.Contains(b.Isin)
                                             || badIsins.Contains(b.Isin)
                                             || b.SecType == null
                                             || (b.SecType != "3" && b.SecType != "6" && b.SecType != "8")  // оставляем только ОФЗ, корп. и биржевые
                                             || b.CouponPercent == null || b.CouponPercent > 15))
                .ToListAsync();

            if (wrongBonds.Any())
            {
                wrongBonds.ForEach(b => b.Discarded = true);
                await traderRepository.UpdateBondsRangeAsync(wrongBonds);
            }

            Recorder.Stop(logger);
        }
    }
}
