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

        public DateTime? RepaymentDate { get; set; }

        /// <summary>
        /// Строго до даты
        /// </summary>
        public bool StrictlyUpToDate { get; set; }

        public CalculationMethod Method { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public double? Amount { get; set; }

        /// <summary>
        /// Облигации без оферты
        /// </summary>
        public bool WithoutOffer { get; set; }

        /// <summary>
        /// Одна облигация от эмитента
        /// </summary>
        public bool OneBondByIssuer { get; set; }

        /// <summary>
        /// Разделить сумму на два портфеля
        /// </summary>
        public bool TwoPortfolios { get; set; }
    }
}
