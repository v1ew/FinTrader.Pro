using FinTrader.Pro.DB.Models;
using System;
using System.Collections.Generic;
using FinTrader.Pro.Contracts.Bonds;

namespace FinTrader.Pro.Contracts.Bonds
{
    /// <summary>
    /// Подборка облигаций с купонным доходом
    /// </summary>
    public class BondSet
    {
        /// <summary>
        /// Список облигаций
        /// </summary>
        public SelectedBond[] Bonds { get; set; }

        /// <summary>
        /// Список всех купонов с датами выплат
        /// </summary>
        public SelectedCoupon[] Coupons { get; set; }
    }
}
