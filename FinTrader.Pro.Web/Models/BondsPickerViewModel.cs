using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinTrader.Pro.Web.Models
{
    /// <summary>
    /// Модель для формы подбора облигаций
    /// </summary>
    [ModelMetadataType(typeof(BondsPickerViewModelMetadata))]
    public class BondsPickerViewModel
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
