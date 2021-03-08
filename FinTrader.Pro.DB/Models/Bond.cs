using FinTrader.Pro.DB.Converters;
using Newtonsoft.Json;
using System;

namespace FinTrader.Pro.DB.Models
{
    [JsonConverter(typeof(JsonBondConverter))]
    public class Bond
    {
        /// <summary>
        /// Код ценной бумаги
        /// </summary>
        public string SecId { get; set; }
        public string BoardId { get; set; }
        /// <summary>
        /// Отмечаем неподходящие бумаги
        /// </summary>
        public bool Discarded { get; set; } = false;
        public string Name { get; set; }
        public string ShortName { get; set; }
        public double? PrevWaPrice { get; set; }
        public double? YieldAtPrevWaPrice { get; set; }
        /// <summary>
        /// Сумма купона, в валюте номинала
        /// </summary>
        public double? CouponValue { get; set; }
        public DateTime? NextCoupon { get; set; }
        public double? AccruedInt { get; set; }
        public double? PrevPrice { get; set; }
        public int? LotSize { get; set; }
        /// <summary>
        /// Первоначальная номинальная стоимость
        /// </summary>
        public double? InitialFaceValue { get; set; }
        /// <summary>
        /// Номинальная стоимость
        /// </summary>
        public double? FaceValue { get; set; }
        public string BoardName { get; set; }
        public string Status { get; set; }
        public DateTime? MatDate { get; set; }
        public int? Decimals { get; set; }
        /// <summary>
        /// Периодичность выплаты купона в год
        /// </summary>
        public int? CouponFrequency { get; set; }
        public int? CouponPeriod { get; set; }
        /// <summary>
        /// Объем выпуска
        /// </summary>
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
        public string Isin { get; set; }
        public string LatName { get; set; }
        public string RegNumber { get; set; }
        public string CurrencyId { get; set; }
        public long? IssueSizePlaced { get; set; }
        /// <summary>
        /// Уровень листинга
        /// </summary>
        public int? ListLevel { get; set; }
        /// <summary>
        /// Вид/категория ценной бумаги
        /// </summary>
        public string TypeName { get; set; }
        public string SecType { get; set; }
        /// <summary>
        /// Ставка купона, %
        /// </summary>
        public double? CouponPercent { get; set; }
        /// <summary>
        /// Возможен досрочный выкуп
        /// </summary>
        public bool? EarlyRepayment { get; set; }
        public DateTime? OfferDate { get; set; }
        public DateTime? IssueDate { get; set; }
        public double? LotValue { get; set; }
        /// <summary>
        /// Дней до погашения
        /// </summary>
        public int? DaysToRedemption { get; set; }
        /// <summary>
        /// Бумаги для квалифицированных инвесторов
        /// </summary>
        public bool? IsQualifiedInvestors { get; set; }
        /// <summary>
        /// Код эмитента
        /// </summary>
        public int? EmitterId { get; set; }
    }
}
