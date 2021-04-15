using System;
using System.Collections.Generic;
using System.Text;

namespace FinTrader.Pro.Contracts.Bonds
{
    public class SelectedCoupon
    {
        public string Isin { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
}
