using System.Collections.Generic;
using System.Linq;
using FinTrader.Pro.DB.Models;

namespace FinTrader.Pro.Bonds.Selector
{
    public static class Runner
    {
        private static readonly int numBondsPerYear = 6;

        public static BondSelector Select(IQueryable<Bond> bonds)
        {
            var ranges = InitRanges();
            var sets = InitBondSets();

            //На вход получаем отобранные, упорядоченные по убыванию релевантности облигации
            BondSelector mySet = null;
            foreach(var bond in bonds) {
                BondSetType setType = BondSetType.None;
                if (!bond.CouponPeriod.HasValue || !bond.NextCoupon.HasValue) continue;
                // Определили тип облигации по периоду
                setType = ranges.Where(r => r.ThisRange(bond.CouponPeriod.Value)).Select(r => r.SetType).FirstOrDefault();
		
                // Добавили в нужные наборы
                foreach (var st in sets.Where(st => st.HaveKey(setType) && !st.IsFull(setType)))
                {
                    st.Add(bond.Isin, bond.NextCoupon.Value.Month, setType);
                }
                // Как только один набор наполнится, поиск прекращаем
                if (sets.Any(st => st.IsFull()))
                {
                    mySet = sets.First(st => st.IsFull());
                    break;
                }
            }

            return mySet;
        }
        
        private static List<BondSelector> InitBondSets() {
            var sets = new List<BondSelector> ();
            sets.Add(new BondSelector(new Dictionary<BondSetType, int> {
                {BondSetType.T31, numBondsPerYear},
            }));
            sets.Add(new BondSelector(new Dictionary<BondSetType, int> {
                {BondSetType.T91, numBondsPerYear},
            }));
            sets.Add(new BondSelector(new Dictionary<BondSetType, int> {
                {BondSetType.T182, numBondsPerYear},
            }));

            return sets;
        }

        private static List<DaysRange> InitRanges()
        {
            var ranges = new List<DaysRange> {
                new DaysRange {MinValue = 28, MaxValue = 31, SetType = BondSetType.T31},
                new DaysRange {MinValue = 91, MaxValue = 92, SetType = BondSetType.T91},
                new DaysRange {MinValue = 180, MaxValue = 184, SetType = BondSetType.T182},
            };

            return ranges;
        }
    }
}