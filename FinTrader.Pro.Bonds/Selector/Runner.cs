using System.Collections.Generic;
using System.Linq;
using FinTrader.Pro.Contracts;
using FinTrader.Pro.DB.Models;

namespace FinTrader.Pro.Bonds.Selector
{
    public class Runner
    {
        private const int NumBondsPerYear = 6;
        private readonly BondsPickerParams _pickerParams;
        private readonly List<int?> _emittersList;
        private readonly List<string> _firstPortfolio;

        public Runner(BondsPickerParams pickerParams)
        {
            _pickerParams = pickerParams;
            _emittersList = new List<int?>();
            _firstPortfolio = new List<string>();
        }
        
        public BondSelector Select(IQueryable<Bond> bonds)
        {
            var ranges = InitRanges();
            var sets = InitBondSets();

            //На вход получаем отобранные, упорядоченные по убыванию релевантности облигации
            BondSelector mySet = null;
            foreach(var bond in bonds) {
                BondSetType setType = BondSetType.None;
                if (!bond.CouponPeriod.HasValue || !bond.NextCoupon.HasValue) continue;
                // Если собираем второй портфель, то исключаем содержимое первого
                if (_firstPortfolio.Any() && _firstPortfolio.Contains(bond.Isin)) continue;
                
                // Определили тип облигации по периоду
                setType = ranges.Where(r => r.ThisRange(bond.CouponPeriod.Value)).Select(r => r.SetType).FirstOrDefault();
		
                // Добавили в нужные наборы
                foreach (var st in sets.Where(st => st.HaveKey(setType) && !st.IsFull(setType)))
                {
                    if (!(_pickerParams.OneBondByIssuer && _emittersList.Contains(bond.EmitterId)))
                    {
                        _emittersList.Add(bond.EmitterId);
                        st.Add(bond, setType);
                    }
                }
                // Как только один набор наполнится, поиск прекращаем
                if (sets.Any(st => st.IsFull()))
                {
                    mySet = sets.First(st => st.IsFull());
                    break;
                }
            }

            // Сохраняем на случай, если надо будет собирать второй портфель
            foreach (var bondIsin in mySet.BondsList.Keys)
            {
                _firstPortfolio.Add(bondIsin);
            }
            
            return mySet;
        }
        
        private List<BondSelector> InitBondSets() {
            var sets = new List<BondSelector> ();
            sets.Add(new BondSelector(new Dictionary<BondSetType, int> {
                {BondSetType.T31, NumBondsPerYear},
            }));
            sets.Add(new BondSelector(new Dictionary<BondSetType, int> {
                {BondSetType.T91, NumBondsPerYear},
            }));
            sets.Add(new BondSelector(new Dictionary<BondSetType, int> {
                {BondSetType.T182, NumBondsPerYear},
            }));

            return sets;
        }

        private List<DaysRange> InitRanges()
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