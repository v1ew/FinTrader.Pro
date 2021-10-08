using System;

namespace FinTrader.Pro.Bonds.Models
{
    public class BondExt : DB.Models.Bond
    {
        public DateTime? CommonDate => (OfferDate ?? MatDate).Value;

        public BondExt(DB.Models.Bond bond)
        {
            Comment = bond.Comment;
            Decimals = bond.Decimals;
            Discarded = bond.Discarded;
            Duration = bond.Duration;
            Isin = bond.Isin;
            Status = bond.Status;
            Updated = bond.Updated;
            Yield = bond.Yield;
            AccruedInt = bond.AccruedInt;
            BoardId = bond.BoardId;
            CouponPercent = bond.CouponPercent;
            CouponPeriod = bond.CouponPeriod;
            CouponValue = bond.CouponValue;
            CurrencyId = bond.CurrencyId;
            EmitterId = bond.EmitterId;
            FaceUnit = bond.FaceUnit;
            FaceValue = bond.FaceValue;
            IssueSize = bond.IssueSize;
            LotSize = bond.LotSize;
            LotValue = bond.LotValue;
            MatDate = bond.MatDate;
            ModifiedDuration = bond.ModifiedDuration;
            NextCoupon = bond.NextCoupon;
            OfferDate = bond.OfferDate;
            SecId = bond.SecId;
            SecName = bond.SecName;
            SecType = bond.SecType;
            ShortName = bond.ShortName;
            ValueAvg = bond.ValueAvg;
            PrevWaPrice = bond.PrevWaPrice;
            YieldAtPrevWaPrice = bond.YieldAtPrevWaPrice;
        }
    }
}