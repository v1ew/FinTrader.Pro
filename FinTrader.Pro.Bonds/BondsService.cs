using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using FinTrader.Pro.Bonds.Extensions;
using FinTrader.Pro.Bonds.Helpers;
using FinTrader.Pro.Bonds.Models;
using FinTrader.Pro.Bonds.Selector;
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
        private const int MAX_YIELD = 15;

        public BondsService(IIssBondsRepository issBondsRepo, IFinTraderRepository traderRepo, ILogger<BondsService> logger)
        {
            issBondsRepository = issBondsRepo;
            traderRepository = traderRepo;
            this.logger = logger;
        }

        public async Task<Portfolio[]> SelectBondsAsync(BondsPickerParams filter)
        {
            var oneYearPlus = DateTime.Now.AddYears(1).AddDays(1);
            var bonds = traderRepository.Bonds
                .Where(b => !b.Discarded && b.Yield > 0 && b.Yield <= MAX_YIELD && b.ValueAvg > 0)
                .Where(b => (b.OfferDate ?? b.MatDate).HasValue) // есть дата погашения или оферты
                .Where(b => (b.OfferDate ?? b.MatDate).Value.CompareTo(oneYearPlus) > 0); // и эта дата дальше года

            if (filter.IsIncludedCorporate != filter.IsIncludedFederal)
            {
                if (filter.IsIncludedCorporate) // выбираем корпоративные и биржевые
                {
                    bonds = bonds.Where(b => b.SecType != "3");
                }
                else // только ОФЗ
                {
                    bonds = bonds.Where(b => b.SecType == "3");
                }
            }

            // Исключаем бумаги с офертой
            if (filter.WithoutOffer)
            {
                bonds = bonds.Where(b => b.OfferDate == null);
            }

            switch (filter.BondsClass)
            {
                case BondClass.MostLiquid:
                    bonds = bonds.OrderByDescending(b => b.ValueAvg).ThenByDescending(b => b.CouponPercent);
                    break;
                case BondClass.MostProfitable:
                    bonds = bonds.OrderByDescending(b => b.CouponPercent).ThenByDescending(b => b.ValueAvg);
                    break;
                case BondClass.ByRepaymentDate:
                    if (filter.RepaymentDate.HasValue)
                    {
                        bonds = bonds.Where(b => b.MatDate.Value.CompareTo(filter.RepaymentDate.Value) <= 0);
                    }
                    //TODO: По дате погашения - надо учитывать дату оферты

                    bonds = bonds.OrderByDescending(b => b.MatDate).ThenByDescending(b => b.CouponPercent);
                    break;
                case BondClass.FarthestRepaynment:
                    bonds = bonds.OrderByDescending(b => b.MatDate).ThenByDescending(b => b.CouponPercent);
                    break;
            }

            var runner = new Runner(filter);
            var bondsSel = runner.Select(bonds);
            if (bondsSel == null)
            {
                return new[]
                {
                    new Portfolio {
                        IsError = true, 
                        ErrorMessage = "По запросу не найдены подходящие облигации. Измените параметры запроса и отправьте повторно, пожалуйста."
                    }
                };
            }
            var selectedBonds = await bonds
                .Where(b => bondsSel.BondsList.Keys.Contains(b.Isin))
                .Select(b => new SelectedBond
            {
                ShortName = b.ShortName,
                MatDate = b.MatDate.Value,
                CouponValue = b.CouponValue.Value,
                AmountToBye = 10,
                Yield = b.Yield.Value,
                Sum = 0.0,
                Isin = b.Isin,
                Cost = b.PrevWaPrice.Value * b.FaceValue.Value / 100
            }).ToListAsync();

            double invAmount = 0.0;
            int avgPay = 0;
            double avgYield = 0.0;
            if (filter.Amount.HasValue)
            {
                if (filter.Method == CalculationMethod.InvestmentAmount)
                {
                    invAmount = filter.Amount.Value * 1.03;
                    double cAvg = selectedBonds.Average(s => s.CouponValue);
                    selectedBonds.ForEach(s => s.K = (1 + (1 - s.CouponValue / cAvg)));
                    double oneBond = invAmount / selectedBonds.Count();
                    selectedBonds.ForEach(s => s.Sum = oneBond * s.K);
                    selectedBonds.ForEach(s => s.AmountToBye = (int)(s.Sum / s.Cost));
                    avgPay = (int)selectedBonds.Average(s => s.AmountToBye * s.CouponValue);
                    avgYield = selectedBonds.Average(s => s.Yield);
                }
                else
                {
                    double monthPay = filter.Amount.Value;
                    avgPay = (int)monthPay;
                    selectedBonds.ForEach(s => s.AmountToBye = (int)Math.Round(monthPay / s.CouponValue));
                    selectedBonds.ForEach(s => s.Sum = s.AmountToBye * s.Cost);
                    invAmount = selectedBonds.Sum(s => s.Sum);
                    avgYield = selectedBonds.Average(s => s.Yield);
                }
            }
            // TODO: move up
            var result = new Portfolio
            {
                Includes = (filter.IsIncludedCorporate ? "Корпоративные облигации" : "") + (filter.IsIncludedFederal ? (filter.IsIncludedCorporate ? " и ОФЗ" : "ОФЗ") : ""),
                Sum = invAmount,
                Pay = avgPay,
                Yields = avgYield,
                MatDate = selectedBonds.Max(s => s.MatDate),
                BondSets = new List<BondSet>()
            };
            
            result.BondSets.Add(new BondSet
                {
                    Bonds = selectedBonds.ToArray(),
                    Coupons = await GetCouponsAsync(selectedBonds.ToDictionary(b => b.Isin, b => b))
                }
            );

            if (filter.TwoPortfolios)
            {
                return new [] {result, result};
            }
            return new [] {result};
        }

        /// <summary>
        /// Скачивает данные по облигациям и сохраняет в БД
        /// </summary>
        /// <returns></returns>
        public async Task UpdateBondsAsync()
        {
            var boardIds = new[]
            {
                "TQCB",
                "TQOB",
            };
            
            var bonds = await issBondsRepository.LoadBondsAsync();
            var newBonds = new List<Bond>();
            var changedBonds = new List<Bond>();
            foreach(var bond in bonds)
            {
                if (!boardIds.Contains(bond[BondsColumnNames.BoardId])) continue;
                // Если бумагу исключаем, то сохраняем об этом информацию в БД, чтобы в будущем ее не проверять лишний раз
                var discarded = false;
                
                var existingBond = traderRepository.Bonds.FirstOrDefault(b => b.SecId == bond[BondsColumnNames.SecId]);
                if (existingBond?.Discarded ?? false) continue;

                bool isQual = false;
                int emitter = 0;
                if (existingBond == null) {
                    (isQual, emitter) = await GetBondInfo(bond[BondsColumnNames.SecId]);

                    if (NullableValue.TryDoubleParse(bond[BondsColumnNames.CouponValue]) == 0 // Если размер купона - 0, то исключаем ее из своего списка
                        || await HasAmortizations(bond[BondsColumnNames.SecId]) // Не берем бумаги с амортизацией
                        || isQual) // Бумага для квалифицированных инвесторов
                    {
                        discarded = true;
                    }
                } else {
                    emitter = existingBond.EmitterId.Value;
                }

                var bondLoaded = MakeBond(bond, emitter, discarded);
                if (existingBond == null)
                {
                    newBonds.Add(bondLoaded);
                    //TODO: save log - record created
                }
                else
                {
                    //changedBonds.Add(bondLoaded);
                    if (haveChanges(existingBond, bondLoaded))
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
                discardedBonds.ForEach(b =>
                {
                    b.Discarded = true;
                    b.Comment = "UpdateCoupons";
                });
                await traderRepository.UpdateBondsRangeAsync(discardedBonds);
            }
        }
        
        public async Task DiscardWrongBondsAsync()
        {
            var secTypes = new[]
            {
                "3", // ОФЗ
                "6", // Корп
                "8" // Биржевые
            };
            
            Recorder.Start();
            var badIsins = await traderRepository.Bonds
                .Where(b => !b.Discarded && b.FaceUnit != "SUR")
                .Select(b => b.Isin).Distinct().ToListAsync();

            var wrongBonds = await traderRepository.Bonds
                .Where(b => !b.Discarded && (badIsins.Contains(b.Isin)
                                             || b.SecType == null
                                             || !secTypes.Contains(b.SecType)  // оставляем только ОФЗ, корп. и биржевые
                                             || (!b.MatDate.HasValue && !b.OfferDate.HasValue)
                                             || b.LotValue > 10000
                                             || b.CouponPeriod > 200
                                             || b.CouponPercent == null 
                                             || b.CouponPercent < 1 || b.CouponPercent > MAX_YIELD))
                .ToListAsync();

            if (wrongBonds.Any())
            {
                wrongBonds.ForEach(b =>
                {
                    b.Discarded = true;
                    b.Comment = "DiscardWrongBonds";
                });
                await traderRepository.UpdateBondsRangeAsync(wrongBonds);
            }

            Recorder.Stop(logger);
        }

        /// <summary>
        /// Выявляет облигации, у которых с купонами не все в порядке
        /// Отмечает их Discarded
        /// </summary>
        public async Task CheckCouponsAsync()
        {
            var bonds = await traderRepository.Bonds.Where(b => !b.Discarded).ToArrayAsync();
            var badCouponsBonds = new List<Bond>();
            foreach (var bond in bonds)
            {
                if (!await CheckBondCouponsAsync(bond)) 
                    badCouponsBonds.Add(bond);
            }

            if (badCouponsBonds.Any())
            {
                badCouponsBonds.ForEach(b =>
                {
                    b.Discarded = true;
                    b.Comment = "CheckCoupons";
                });
                await traderRepository.UpdateBondsRangeAsync(badCouponsBonds);
            }
        }

        /// <summary>
        /// Обновить дюрацию и доходность
        /// </summary>
        public async Task UpdateBondsDurationAsync()
        {
            var date = await UpdateTradeDateAsync();
            var durations = await issBondsRepository.LoadBondsDurationsAsync(date);
            var updatedBonds = new List<Bond>();
            foreach (var duration in durations)
            {
                var bond = await traderRepository.Bonds
                    .Where(b => b.SecId == duration[DurationsColumnNames.SecId])
                    .FirstOrDefaultAsync();
                if (bond?.Discarded == false)
                {
                    bond.Duration = NullableValue.TryDoubleParse(duration[DurationsColumnNames.Duration]);
                    bond.Yield = NullableValue.TryDoubleParse(duration[DurationsColumnNames.Yield]);
                    if (bond.Duration.HasValue && bond.Yield.HasValue)
                    {
                        bond.ModifiedDuration = bond.Duration / ((1 + bond.Yield / 100) * 365);
                    }
                    bond.Updated = DateTime.Now;

                    updatedBonds.Add(bond);
                }
            }

            await traderRepository.UpdateBondsRangeAsync(updatedBonds);
        }
        
        /// <summary>
        /// Обновить средние объемы торгов по бондам
        /// </summary>
        public async Task UpdateBondsValueAsync()
        {
            var dates = await traderRepository.TradeDates
                .OrderByDescending(d => d.Id).Take(5).ToListAsync();
            
            var bonds = await traderRepository.Bonds
                .Where(b => !b.Discarded).ToListAsync();
            foreach (var bond in bonds)
            {
                var hist = await issBondsRepository
                    .LoadBondHistoryAsync(bond.SecId, dates[dates.Count - 1].Date);
                var count = hist.Count();
                if (count < 1) continue;
                double sum = 0;
                foreach (var h in hist)
                {
                    var value = NullableValue.TryDoubleParse(h[HistoryColumnNames.Value]);
                    sum += value ?? 0;
                }

                bond.ValueAvg = sum / count;
                bond.Updated = DateTime.Now;
            }

            await traderRepository.UpdateBondsRangeAsync(bonds);
        }
        
        /// <summary>
        /// Проверяем, есть ли последние 5 дней торгов в БД
        /// Если не хватает,то добавляем новые даты
        /// </summary>
        /// <returns>Актуальную дату последнего дня торгов (ранее сегодня)</returns>
        public async Task<DateTime> UpdateTradeDateAsync()
        {
            var result = DateTime.Today.AddDays(-1);
            List<TradeDate> newDates = new List<TradeDate>();
            
            var last5Dates = await GetLastFiveTradeDatesAsync();
            foreach (var date in last5Dates)
            {
                if (!traderRepository.TradeDates.Any(d => d.Date.CompareTo(date) == 0))
                {
                    newDates.Add(new TradeDate {Date = date});
                }
            }

            if (newDates.Any())
            {
                await traderRepository.AddTradeDatesAsync(newDates.OrderBy(d => d.Date).ToArray());
            }
            
            if (traderRepository.TradeDates.Any())
            {
                var lastDate = await traderRepository.TradeDates.MaxAsync(d => d.Date);
                result = lastDate;
            }

            return result;
        }

        /// <summary>
        /// Получить с сайта биржи список из последних 5 торговых дней
        /// </summary>
        /// <returns>Список дат</returns>
        private async Task<List<DateTime>> GetLastFiveTradeDatesAsync()
        {
            // Получить первый попавшийся бонд
            var secId = await issBondsRepository.LoadAnyBondAsync();

            if (secId == string.Empty) return new List<DateTime>();
            
            // Получить дату первого числа предыдущего месяца
            var startDate = DateTime.Now.AddMonths(-1).AddDays(-DateTime.Now.Day + 1);

            var dates = await issBondsRepository.LoadBondHistoryDatesAsync(secId, startDate);

            return dates.TakeLast(5)
                .Select(d => NullableValue.TryDateParse(d[HistoryColumnNames.TradeDate]) ?? DateTime.Now)
                .ToList();
        }
        
        /// <summary>
        /// Проверяет наличие всех купонов облигации до даты погашения или ближайшей даты оферты
        /// </summary>
        /// <param name="bond">Облигация, для которой проверяем</param>
        /// <returns>true - если все нормально</returns>
        private async Task<bool> CheckBondCouponsAsync(Bond bond)
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
        private async Task<SelectedCoupon[]> GetCouponsAsync(IDictionary<string, SelectedBond> bonds)
        {
            var today = DateTime.Now;
            return await traderRepository.Coupons
                .Where(c => bonds.Keys.Contains(c.Isin) 
                            && c.CouponDate.HasValue 
                            && c.CouponDate.Value.CompareTo(today) >= 0)
                .Select(c => new SelectedCoupon
                {
                    ShortName = bonds[c.Isin].ShortName,
                    Isin = c.Isin,
                    Date = c.CouponDate ?? DateTime.MinValue,
                    Value = (c.Value ?? 0) * bonds[c.Isin].AmountToBye,
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
                NextCoupon = NullableValue.TryDateParse(bond[BondsColumnNames.NextCoupon]),
                CouponPeriod = NullableValue.TryIntParse(bond[BondsColumnNames.CouponPeriod]),
                IssueSize = NullableValue.TryLongParse(bond[BondsColumnNames.IssueSize]),
                PrevWaPrice = NullableValue.TryDoubleParse(bond[BondsColumnNames.PrevWaPrice]),
                YieldAtPrevWaPrice = NullableValue.TryDoubleParse(bond[BondsColumnNames.YieldAtPrevWaPrice]),
                SecName = bond[BondsColumnNames.SecName],
                FaceUnit = bond[BondsColumnNames.FaceUnit],
                CurrencyId = bond[BondsColumnNames.CurrencyId],
                Isin = bond[BondsColumnNames.Isin],
                SecType = bond[BondsColumnNames.SecType],
                CouponPercent = NullableValue.TryDoubleParse(bond[BondsColumnNames.CouponPercent]),
                OfferDate = NullableValue.TryDateParse(bond[BondsColumnNames.OfferDate]),
                LotValue = NullableValue.TryDoubleParse(bond[BondsColumnNames.LotValue]),
                EmitterId = emitter,
                Updated = DateTime.Now,
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
        private bool haveChanges(Bond bond, Bond bondLoaded)
        {
            bool result = false;
            if (bondLoaded.Status != bond.Status)
            {
                bond.Status = bondLoaded.Status;
                result = true;
            }
            if (bondLoaded.AccruedInt != bond.AccruedInt)
            {
                bond.AccruedInt = bondLoaded.AccruedInt;
                result = true;
            }
            if (bondLoaded.IssueSize != bond.IssueSize)
            {
                bond.IssueSize = bondLoaded.IssueSize;
                result = true;
            }
            if (bondLoaded.CouponValue != bond.CouponValue)
            {
                bond.CouponValue = bondLoaded.CouponValue;
                result = true;
            }
            if (bondLoaded.CouponPercent != bond.CouponPercent)
            {
                bond.CouponPercent = bondLoaded.CouponPercent;
                result = true;
            }
            if ((bond.NextCoupon.HasValue && bondLoaded.NextCoupon.HasValue && bondLoaded.NextCoupon.Value.CompareTo(bond.NextCoupon.Value) != 0)
                || (!bond.NextCoupon.HasValue && bondLoaded.NextCoupon.HasValue))
            {
                bond.NextCoupon = bondLoaded.NextCoupon;
                result = true;
            }
            if (bondLoaded.OfferDate != bond.OfferDate)
            {
                bond.OfferDate = bondLoaded.OfferDate;
                result = true;
            }

            if (result) bond.Updated = DateTime.Now;
            
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
