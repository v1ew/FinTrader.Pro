using System;
using System.Collections.Generic;
using System.Text;

namespace FinTrader.Pro.DB.Models
{
    public class Coupon
    {
        public int CouponId { get; set; }
        /// <summary>
        /// Код ISIN
        /// </summary>
        public string Isin { get; set; }
        public DateTime? CouponDate { get; set; }
        public DateTime? RecordDate { get; set; }
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Первоначальная номинальная стоимость
        /// </summary>
        public double? InitialFaceValue { get; set; }
        /// <summary>
        /// Номинальная стоимость
        /// </summary>
        public double? FaceValue { get; set; }
        public string FaceUnit { get; set; }
        public double? Value { get; set; }
        public double? ValuePrc { get; set; }
        public double? ValueRub { get; set; }
    }
}
