﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinTrader.Pro.Bonds.Models;
using FinTrader.Pro.Contracts;
using FinTrader.Pro.Contracts.Bonds;
using FinTrader.Pro.Contracts.Enums;
using FinTrader.Pro.DB.Models;
using FinTrader.Pro.DB.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinTrader.Pro.Bonds.Selector
{
    public class Runner
    {
        private const int NumBondsPerYear = 6;
        private readonly BondsPickerParams _pickerParams;
        private readonly List<int?> _emittersList;
        private readonly List<string> _firstPortfolio;
        private readonly IFinTraderRepository _traderRepository;

        public Runner(BondsPickerParams pickerParams, IFinTraderRepository traderRepository)
        {
            _pickerParams = pickerParams;
            _emittersList = new List<int?>();
            _firstPortfolio = new List<string>();
            _traderRepository = traderRepository;
        }

        /// <summary>
        /// Запускает подбор портфелей, в зависимости от требуемого количества - один или два
        /// </summary>
        /// <param name="bonds"></param>
        /// <returns></returns>
        public async Task<Portfolio[]> ExecuteAsync(IEnumerable<BondExt> bonds)
        {
            Portfolio portfolio1 = new Portfolio();
            Portfolio portfolio2 = new Portfolio();
            double amount = _pickerParams.Amount.Value * 1.03;

            // One portfolio
            if (!_pickerParams.TwoPortfolios)
            {
                portfolio1 = await GetPortfolioAsync(bonds, amount);

                return new [] { portfolio1 };
            }

            // Two portfolios
            portfolio1 = await GetPortfolioAsync(bonds, amount / 2.0);
            if (portfolio1.IsError)
            {
                return new [] { portfolio1 };
            }
            else
            {
                portfolio2 = await GetPortfolioAsync(bonds, amount - portfolio1.Sum);
            }

            return new [] { portfolio1, portfolio2 };
        }

        /// <summary>
        /// Вызывает подбор облигаций портфеля и
        /// расчет количества бумаг в портфель для получения требуемого размера купона
        /// </summary>
        /// <param name="bonds"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private async Task<Portfolio> GetPortfolioAsync(IEnumerable<BondExt> bonds, double amount)
        {
            var portfolio = new Portfolio();
            var selectedBonds = GetBondsSet(bonds);
            if (selectedBonds.Count == 0)
            {
                portfolio.IsError = true;
                portfolio.ErrorMessage =
                    "По запросу не найдены подходящие облигации. Измените параметры запроса и отправьте повторно, пожалуйста.";
            }
            else
            {
                portfolio = await CalculatePortfolioAsync(selectedBonds, amount);
            }

            return portfolio;
        }
        
        /// <summary>
        /// Осуществляет подбор облигаций, путем перебора бумаг и формирования наборов по схожим параметрам
        /// Возвращает первый полный набор
        /// </summary>
        /// <param name="bonds">Облигации, отобранные и отсортированные по параметрам фильтра</param>
        /// <returns>Набор бумаг, на основе которого формируется портфель</returns>
        private BondSelector Select(IEnumerable<BondExt> bonds)
        {
            var ranges = InitRanges();
            var sets = InitBondSets();

            //На вход получаем отобранные, упорядоченные по убыванию релевантности облигации
            BondSelector mySet = null;
            foreach(var bond in bonds) {
                BondSetType setType = BondSetType.None;
                if (!bond.CouponPeriod.HasValue || !bond.NextCoupon.HasValue) continue;
                // Если собираем второй портфель, то исключаем содержимое первого
                if (_firstPortfolio.Any() && _firstPortfolio.Contains(bond.Isin)) continue;
                
                // Определили тип облигации по периоду
                setType = ranges.Where(r => r.ThisRange(bond.CouponPeriod.Value)).Select(r => r.SetType).FirstOrDefault();
		
                // Добавили в нужные наборы
                foreach (var st in sets.Where(st => st.HaveKey(setType) && !st.IsFull(setType)))
                {
                    if (!(_pickerParams.OneBondByIssuer && _emittersList.Contains(bond.EmitterId)))
                    {
                        _emittersList.Add(bond.EmitterId);
                        st.Add(bond, setType);
                    }
                }
                // Как только один набор наполнится, поиск прекращаем
                if (sets.Any(st => st.IsFull()))
                {
                    mySet = sets.First(st => st.IsFull());
                    break;
                }
            }

            if (mySet != null)
            {
                // Сохраняем на случай, если надо будет собирать второй портфель
                foreach (var bondIsin in mySet.BondsList.Keys)
                {
                    _firstPortfolio.Add(bondIsin);
                }
            }
            
            return mySet;
        }

        /// <summary>
        /// Осуществляет расчет количества бумаг для портфеля с целью получения ежемесячно заданной суммы в виде купона
        /// Формирует портфель из набора облигаций
        /// </summary>
        /// <param name="selectedBonds">Набор облигаций портфеля</param>
        /// <returns>Готовый портфель</returns>
        public async Task<Portfolio> CalculatePortfolioAsync(List<SelectedBond> selectedBonds, double invAmount)
        {
            int avgPay = 0;
            double avgYield = 0.0;
            if (_pickerParams.Amount.HasValue)
            {
                if (_pickerParams.Method == CalculationMethod.InvestmentAmount)
                {
                    double cAvg = selectedBonds.Average(s => s.CouponValue);
                    selectedBonds.ForEach(s => s.K = (1 + (1 - s.CouponValue / cAvg)));
                    double oneBond = invAmount / selectedBonds.Count();
                    // Найдем количество бумаги к покупке
                    selectedBonds.ForEach(s => s.Sum = oneBond * s.K);
                    selectedBonds.ForEach(s => s.AmountToBye = (int)(s.Sum / s.Cost));
                    // Расчитаем реальную стоимость бумаги
                    selectedBonds.ForEach(s => s.Sum = s.AmountToBye * s.Cost);
                    // Вычислим реальную стоимость портфеля
                    invAmount = selectedBonds.Sum(s => s.Sum);
                    avgPay = (int)selectedBonds.Average(s => s.AmountToBye * s.CouponValue);
                    // Посчитаем примерную доходность портфеля
                    avgYield = selectedBonds.Sum(s => s.Yield * s.AmountToBye) / selectedBonds.Sum(s => s.AmountToBye);
                }
                else
                {
                    double monthPay = _pickerParams.Amount.Value;
                    avgPay = (int)monthPay;
                    selectedBonds.ForEach(s => s.AmountToBye = (int)Math.Round(monthPay / s.CouponValue));
                    selectedBonds.ForEach(s => s.Sum = s.AmountToBye * s.Cost);
                    // Вычислим реальную стоимость портфеля
                    invAmount = selectedBonds.Sum(s => s.Sum);
                    // Посчитаем примерную доходность портфеля
                    avgYield = selectedBonds.Sum(s => s.Yield * s.AmountToBye) / selectedBonds.Sum(s => s.AmountToBye);
                }
            }
            // TODO: move up
            var result = new Portfolio
            {
                Includes = (_pickerParams.IsIncludedCorporate ? "Корпоративные облигации" : "") 
                           + (_pickerParams.IsIncludedFederal ? (_pickerParams.IsIncludedCorporate ? " и ОФЗ" : "ОФЗ") : ""),
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
            
            return result;
        }

        /// <summary>
        /// Вызывает метод выбора облигаций для помесячного получения купона
        /// </summary>
        /// <param name="bonds"></param>
        /// <returns>Список отобранных облигаций</returns>
        private List<SelectedBond> GetBondsSet(IEnumerable<BondExt> bonds)
        {
            var bondsSel = Select(bonds);
            if (bondsSel == null)
            {
                return new List<SelectedBond>();
            }
            var selectedBonds = bonds
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
                }).ToList();

            return selectedBonds;
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
            var coupons = await _traderRepository.Coupons
                .Where(c => bonds.Keys.Contains(c.Isin) 
                            && c.CouponDate.HasValue 
                            && c.CouponDate.Value >= today)
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
            int counter = 0;
            foreach (var selectedCoupon in coupons)
            {
                selectedCoupon.Position = ++counter;
            }
            return coupons;
        }

        private List<BondSelector> InitBondSets() {
            var sets = new List<BondSelector> ();
            sets.Add(new BondSelector(new Dictionary<BondSetType, int> {
                {BondSetType.T31, NumBondsPerYear},
            }));
            sets.Add(new BondSelector(new Dictionary<BondSetType, int> {
                {BondSetType.T91, NumBondsPerYear},
            }));
            sets.Add(new BondSelector(new Dictionary<BondSetType, int> {
                {BondSetType.T182, NumBondsPerYear},
            }));

            return sets;
        }

        private List<DaysRange> InitRanges()
        {
            var ranges = new List<DaysRange> {
                new DaysRange {MinValue = 28, MaxValue = 31, SetType = BondSetType.T31},
                new DaysRange {MinValue = 91, MaxValue = 92, SetType = BondSetType.T91},
                new DaysRange {MinValue = 180, MaxValue = 184, SetType = BondSetType.T182},
            };

            return ranges;
        }
    }
}