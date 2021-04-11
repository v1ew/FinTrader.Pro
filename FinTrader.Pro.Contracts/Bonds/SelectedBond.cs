using System;
using System.Collections.Generic;
using System.Text;

namespace FinTrader.Pro.Contracts.Bonds
{
    public class SelectedBond
    {
        public string ShortName { get; set; }
        
        /// <summary>
        /// Дата погашения
        /// </summary>
        public DateTime MatDate { get; set; }
        
        /// <summary>
        /// Сумма купона, в валюте номинала
        /// </summary>
        public double CouponValue { get; set; }

        /// <summary>
        /// Купить штук
        /// </summary>
        public int AmountToBuy { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public double Sum { get; set; }
    }
}
