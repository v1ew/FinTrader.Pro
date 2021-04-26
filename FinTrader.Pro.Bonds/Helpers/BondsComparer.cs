using System.Collections.Generic;
using FinTrader.Pro.DB.Models;

namespace FinTrader.Pro.Bonds.Helpers
{
    /// <summary>
    /// Класс для сравнения бондов по ISIN, чтобы исключить повторяюшиеся
    /// TODO: Выяснить, какие режимы торгов можно исключить
    /// </summary>
    public class BondsComparer : IEqualityComparer<Bond>
    {
        public bool Equals(Bond x, Bond y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Isin == y.Isin;
        }

        public int GetHashCode(Bond obj)
        {
            var isin = IsinHashCode(obj);
            var name = ShortNameHashCode(obj);
            return (isin != 0 && name != 0 ? isin * name : (isin == 0 ? name : isin));
        }

        private int IsinHashCode(Bond obj)
        {
            return (obj.Isin != null ? obj.Isin.GetHashCode() : 0);
        }
        
        private int ShortNameHashCode(Bond obj)
        {
            return (obj.ShortName != null ? obj.ShortName.GetHashCode() : 0);
        }
    }
}