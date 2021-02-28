using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinTrader.Pro.Web.Models
{
    internal abstract class BondsPickerViewModelMetadata
    {
        [Display(Name = "Государственные облигации (ОФЗ)", Description = "Государственные облигации (ОФЗ)", Prompt = "Государственные облигации (ОФЗ)")]
        public bool IsFederalAccepted { get; set; }

        [Display(Name = "Облигации компаний", Description = "", Prompt = "")]
        public bool IsCorporateAccepted { get; set; }

        [Display(Name = "", Description = "", Prompt = "")]
        public BondClass Class { get; set; }

        [DisplayFormat(NullDisplayText = "No date supplied", DataFormatString = "{0:yyyyMMdd}")]
        [DataType(DataType.Date)]
        public DateTime? RepaymentDate { get; set; }

        [Display(Name = "Строго до даты", Description = "", Prompt = "")]
        public bool StrictlyUpToDate { get; set; }

        [Display(Name = "", Description = "", Prompt = "")]
        public CalculationMethod Method { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        [Display(Name = "Сумма", Description = "", Prompt = "")]
        public double Amount { get; set; }

        /// <summary>
        /// Облигации без оферты
        /// </summary>
        [Display(Name = "Облигации без оферты", Description = "", Prompt = "")]
        public bool WithoutOffer { get; set; }

        /// <summary>
        /// Одна облигация от эмитента
        /// </summary>
        [Display(Name = "Одна облигация от эмитента", Description = "", Prompt = "")]
        public bool OneBondByIssuer { get; set; }

        /// <summary>
        /// Разделить сумму на два портфеля
        /// </summary>
        [Display(Name = "Разделить сумму на два портфеля", Description = "", Prompt = "")]
        public bool TwoPortfolios { get; set; }
    }
}
