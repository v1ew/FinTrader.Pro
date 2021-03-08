using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinTrader.Pro.Bonds.Extensions;
using FinTrader.Pro.DB.Repositories;
using FinTrader.Pro.Iss.Columns;

namespace FinTrader.Pro.Bonds
{
    public class BondsService : IBondsService
    {
        private readonly IIssBondsRepository issBondsRepository;
        private readonly IFinTraderRepository traderRepository;

        public BondsService(IIssBondsRepository issBondsRepo, IFinTraderRepository traderRepo)
        {
            issBondsRepository = issBondsRepo;
            traderRepository = traderRepo;
        }

        public void SelectBonds()
        {

            var result = traderRepository.Bonds.OrderByDescending(b => b.CouponPercent);
        }

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
                        //ListLevel = bond[BondsColumnNames],
                        SecType = bond[BondsColumnNames.SecType],
                        CouponPercent = NullableValue.TryDoubleParse(bond[BondsColumnNames.CouponPercent]),
                        OfferDate = NullableValue.TryDateParse(bond[BondsColumnNames.OfferDate]),
                        LotValue = NullableValue.TryDoubleParse(bond[BondsColumnNames.LotValue]),

                    });
                }

                await traderRepository.AddBondsRangeAsync(newBonds.ToArray());
            }

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
    }
}
