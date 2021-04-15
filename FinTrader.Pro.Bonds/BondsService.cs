using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinTrader.Pro.Bonds.Extensions;
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

            bonds = bonds.OrderByDescending(b => b.CouponPercent).Take(BONDS_NUM);
            var selectedBonds = await bonds.Select(b => new SelectedBond
            {
                ShortName = b.ShortName,
                MatDate = b.MatDate.Value,
                CouponValue = b.CouponValue.Value,
                AmountToBye = 10,
                Sum = b.FaceValue.Value * 10
            }).ToArrayAsync();

            return new BondSet
            {
                Bonds = selectedBonds,
                Coupons = await GetCouponsAsync(bonds.Select(b => b.Isin).ToArray())
            };
        }

        /// <summary>
        /// Получить массив купонов по массиву номеров isin
        /// </summary>
        /// <param name="isins"></param>
        /// <returns></returns>
        private async Task<SelectedCoupon[]> GetCouponsAsync(string[] isins)
        {
            return await traderRepository.Coupons.Where(c => isins.Contains(c.Isin))
                .Select(c => new SelectedCoupon
                {
                    Isin = c.Isin,
                    Date = c.CouponDate ?? DateTime.MinValue,
                    Value = c.Value ?? 0
                })
                .OrderBy(sc => sc.Date)
                .ToArrayAsync();
        }

        /// <summary>
        /// Скачивает данные по облигациям и сохраняет в БД
        /// </summary>
        /// <returns></returns>
        public async Task UpdateStorageAsync()
        {
            var bonds = await issBondsRepository.LoadBondsAsync();
            var newBonds = new List<DB.Models.Bond>();
            foreach(var bond in bonds)
            {
                if (!traderRepository.Bonds.Any(b => b.SecId == bond[BondsColumnNames.SecId]))
                {
                    newBonds.Add(new DB.Models.Bond
                    {
                        SecId = bond[BondsColumnNames.SecId],
                        BoardId = bond[BondsColumnNames.BoardId],
                        ShortName = bond[BondsColumnNames.ShortName],
                        //PrevWaPrice = bond[BondsColumnNames],
                        //YieldAtPrevWaPrice = bond[BondsColumnNames],
                        CouponValue = NullableValue.TryDoubleParse(bond[BondsColumnNames.CouponValue]),
                        NextCoupon = NullableValue.TryDateParse(bond[BondsColumnNames.NextCoupon]),
                        //AccruedInt = bond[BondsColumnNames],
                        //PrevPrice = bond[BondsColumnNames],
                        LotSize = NullableValue.TryIntParse(bond[BondsColumnNames.LotSize]),
                        FaceValue = NullableValue.TryDoubleParse(bond[BondsColumnNames.FaceValue]),
                        //BoardName = bond[BondsColumnNames],
                        Status = bond[BondsColumnNames.Status],
                        MatDate = NullableValue.TryDateParse(bond[BondsColumnNames.MatDate]),
                        //Decimals = NullableValue.TryIntParse(bond[BondsColumnNames.Decimals]),
                        CouponPeriod = NullableValue.TryIntParse(bond[BondsColumnNames.CouponPeriod]),
                        IssueSize = NullableValue.TryLongParse(bond[BondsColumnNames.IssueSize]),
                        //PrevLegalClosePrice = bond[BondsColumnNames],
                        //PrevAdmittedQuote = bond[BondsColumnNames],
                        //PrevDate = bond[BondsColumnNames],
                        SecName = bond[BondsColumnNames.SecName],
                        //Remarks = bond[BondsColumnNames],
                        //InstrId = bond[BondsColumnNames],
                        //MarketCode = bond[BondsColumnNames],
                        //MinStep = bond[BondsColumnNames],
                        FaceUnit = bond[BondsColumnNames.FaceUnit],
                        //BuyBackPrice = bond[BondsColumnNames],
                        //BuyBackDate = bond[BondsColumnNames],
                        LatName = bond[BondsColumnNames.LatName],
                        RegNumber = bond[BondsColumnNames.RegNumber],
                        CurrencyId = bond[BondsColumnNames.CurrencyId],
                        //IssueSizePlaced = bond[BondsColumnNames],
                        Isin = bond[BondsColumnNames.Isin],
                        SecType = bond[BondsColumnNames.SecType],
                        CouponPercent = NullableValue.TryDoubleParse(bond[BondsColumnNames.CouponPercent]),
                        OfferDate = NullableValue.TryDateParse(bond[BondsColumnNames.OfferDate]),
                        LotValue = NullableValue.TryDoubleParse(bond[BondsColumnNames.LotValue]),

                    });
                }
            }

            await traderRepository.AddBondsRangeAsync(newBonds.ToArray());

            if (newBonds.Any())
            {
                foreach (var bond in newBonds)
                {
                    var coupons = await issBondsRepository.LoadCouponsAsync(bond.SecId);
                    if (!coupons.Any())
                        continue;

                    var newCoupons = new List<DB.Models.Coupon>();
                    foreach (var coupon in coupons)
                    {
                        newCoupons.Add(new DB.Models.Coupon
                        {
                            Isin = coupon[CouponsColumnNames.Isin],
                            CouponDate = NullableValue.TryDateParse(coupon[CouponsColumnNames.CouponDate]),
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

                    await traderRepository.AddCouponsRangeAsync(newCoupons.ToArray());
                }
            }
        }

        public async Task DiscardWrongBondsAsync()
        {
            Recorder.Start();
            // Убираем все с амортизацией
            var wrongIsins = (from c in traderRepository.Coupons
                              where c.InitialFaceValue > c.FaceValue
                              select c.Isin).Distinct();

            var wrongBonds = await traderRepository.Bonds
                .Where(b => !b.Discarded && (wrongIsins.Contains(b.Isin)
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
