using FinTrader.Pro.Contracts.Enums;
using System;

namespace FinTrader.Pro.Contracts
{
    /// <summary>
    /// Параметры отбора облигаций
    /// </summary>
    public class BondsPickerParams
    {
        public bool IsFederalAccepted { get; set; }

        public bool IsCorporateAccepted { get; set; }

        public BondClass Class { get; set; }

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
