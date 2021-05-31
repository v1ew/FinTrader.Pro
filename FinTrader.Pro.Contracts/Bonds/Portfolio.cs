using System;
using System.Collections.Generic;

namespace FinTrader.Pro.Contracts.Bonds
{
    public class Portfolio
    {
        public string Includes { get; set; }
        public int Sum { get; set; }
        public int Pay { get; set; }
        public double Yields { get; set; }
        public DateTime MatDate { get; set; }
        public List<BondSet> BondSets { get; set; }
    }
}