using FinTrader.Pro.DB.Models;
using System;
using System.Collections.Generic;
using FinTrader.Pro.Contracts.Bonds;

namespace FinTrader.Pro.Contracts
{
    public class BondSet
    {
        public SelectedBond[] Bonds { get; set; }
    }
}
