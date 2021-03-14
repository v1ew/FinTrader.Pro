using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinTrader.Pro.Bonds.Extensions;
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

        public BondsService(IIssBondsRepository issBondsRepo, IFinTraderRepository traderRepo, ILogger<BondsService> logger)
        {
            issBondsRepository = issBondsRepo;
            traderRepository = traderRepo;
            this.logger = logger;
        }

        public async Task<Bond[]> SelectBondsAsync()
        {
            var result = traderRepository.Bonds.OrderByDescending(b => b.CouponPercent);
            return await result.ToArrayAsync();
        }

        /// <summary>
        /// Скачивает данные по облигациям и сохраняет в БД
        /// </summary>
        /// <returns></returns>
        public async Task UpdateStorage()
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
            var wrongIsins = (from c in traderRepository.Coupons
                              where c.InitialFaceValue > c.FaceValue
                              select c.Isin).Distinct();

            var wrongBonds = await traderRepository.Bonds.Where(b => !b.Discarded && wrongIsins.Contains(b.Isin)).ToListAsync();

            if (wrongBonds.Any())
            {
                await wrongBonds.ForEachAsync(async b => b.Discarded = true);
                await traderRepository.UpdateBondsRangeAsync(wrongBonds);
            }

            Recorder.Stop(logger);
        }
    }
}
