using System;
using System.Collections.Generic;
using System.Text;

namespace FinTrader.Pro.Contracts.Bonds
{
    public class SelectedCoupon
    {
        /// <summary>
        /// Порядковый номер
        /// </summary>
        public int Position { get; set; }
        
        public string ShortName { get; set; }
        
        public string Isin { get; set; }
        
        public DateTime Date { get; set; }
        
        public double Value { get; set; }
        
        public string Comment { get; set; }
    }
}
