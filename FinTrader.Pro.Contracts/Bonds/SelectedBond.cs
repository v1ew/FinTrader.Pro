using System;

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
        public int AmountToBye { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public double Sum { get; set; }
        
        public string Isin { get; set; }
        
        public double Cost { get; set; }
        
        public double K { get; set; }
    }
}
