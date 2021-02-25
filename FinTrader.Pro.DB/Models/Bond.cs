using System;
using System.Collections.Generic;
using System.Text;

namespace FinTrader.Pro.DB.Models
{
    public class Bond
    {
        public string SecId { get; set; }
        public string BoardId { get; set; }
        public string ShortName { get; set; }
        public double? PrevWaPrice { get; set; }
        public double? YieldAtPrevWaPrice { get; set; }
        public double? CouponValue { get; set; }
        public DateTime? NextCoupon { get; set; }
        public double? AccruedInt { get; set; }
        public double? PrevPrice { get; set; }
        public int? LotSize { get; set; }
        public double? FaceValue { get; set; }
        public string BoardName { get; set; }
        public string Status { get; set; }
        public DateTime? MatDate { get; set; }
        public int? Decimals { get; set; }
        public int? CouponPeriod { get; set; }
        public long? IssueSize { get; set; }
        public double? PrevLegalClosePrice { get; set; }
        public double? PrevAdmittedQuote { get; set; }
        public DateTime? PrevDate { get; set; }
        public string SecName { get; set; }
        public string Remarks { get; set; }
        public string MarketCode { get; set; }
        public string InstrId { get; set; }
        public string SectorId { get; set; }
        public double? MinStep { get; set; }
        public string FaceUnit { get; set; }
        public double? BuyBackPrice { get; set; }
        public DateTime? BuyBackDate { get; set; }
        public string IsIn { get; set; }
        public string LatName { get; set; }
        public string RegNumber { get; set; }
        public string CurrencyId { get; set; }
        public long? IssueSizePlaced { get; set; }
        public int? ListLevel { get; set; }
        public string SecType { get; set; }
        public double? CouponPercent { get; set; }
        public DateTime? OfferDate { get; set; }
        public DateTime? SettleDate { get; set; }
        public double? LotValue { get; set; }
    }
}
