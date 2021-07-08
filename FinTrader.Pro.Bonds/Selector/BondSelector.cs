using System;
using System.Collections.Generic;
using System.Linq;

namespace FinTrader.Pro.Bonds.Selector
{
    public class BondSelector
    {
        private Dictionary<BondSetType, int> sampleSet;
        private Dictionary<BondSetType, int> resultSet;
        public Dictionary<string, int> BondsList {get;} // isin and month

        public BondSelector(Dictionary<BondSetType, int> sampleSet) {
            this.sampleSet = sampleSet;
            resultSet = new Dictionary<BondSetType, int>();
            BondsList = new Dictionary<string, int>();
            foreach (var key in sampleSet.Keys) {
                resultSet[key] = 0;
            }
        }
	
        public void Add(string isin, int month, BondSetType type)
        {
            if (BondsList.Values.Contains(month)) return;
            BondsList.Add(isin, month);
            resultSet[type] += 1;
        }
	
        public bool IsFull() {
            bool result = true;
		
            foreach (var key in sampleSet.Keys){
                if (sampleSet[key] != resultSet[key]) {
                    result = false;
                    break;
                }
            }
		
            return result;
        }

        public bool IsFull(BondSetType type) {
            return sampleSet[type] == resultSet[type];
        }

        public bool HaveKey(BondSetType type) {
            return sampleSet.Keys.Contains(type);
        }
    }
}