using FinTrader.Pro.Contracts.Enums;
using System;
using Newtonsoft.Json;

namespace FinTrader.Pro.Contracts
{
    /// <summary>
    /// Параметры отбора облигаций
    /// </summary>
    public class BondsPickerParams
    {
        [JsonProperty(PropertyName = "isIncludedFederal")]
        public bool IsIncludedFederal { get; set; }

        [JsonProperty(PropertyName = "isIncludedCorporate")]
        public bool IsIncludedCorporate { get; set; }

        [JsonProperty(PropertyName = "bondsClass")]
        public BondClass BondsClass { get; set; }

        [JsonProperty(PropertyName = "repaymentDate")]
        public DateTime? RepaymentDate { get; set; }

        /// <summary>
        /// Строго до даты
        /// </summary>
        [JsonProperty(PropertyName = "strictlyUpToDate")]
        public bool StrictlyUpToDate { get; set; }

        [JsonProperty(PropertyName = "calculationMethod")]
        public CalculationMethod Method { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public double? Amount { get; set; }

        /// <summary>
        /// Облигации без оферты
        /// </summary>
        [JsonProperty(PropertyName = "withoutOffer")]
        public bool WithoutOffer { get; set; }

        /// <summary>
        /// Одна облигация от эмитента
        /// </summary>
        [JsonProperty(PropertyName = "oneBondByIssuer")]
        public bool OneBondByIssuer { get; set; }

        /// <summary>
        /// Разделить сумму на два портфеля
        /// </summary>
        [JsonProperty(PropertyName = "isTwoPortfolios")]
        public bool TwoPortfolios { get; set; }
    }
}
