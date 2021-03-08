using System;
using System.Collections.Generic;
using System.Text;

namespace FinTrader.Pro.Bonds.Extensions
{
    public static class NullableValue
    {
        public static double? TryDoubleParse(string input)
        {
            double result;
            var success = double.TryParse(input, out result);
            return success ? result as double? : null;
        }

        public static DateTime? TryDateParse(string input)
        {
            DateTime result;
            var success = DateTime.TryParse(input, out result);
            return success ? result as DateTime? : null;
        }

        public static int? TryIntParse(string input)
        {
            int result;
            var success = int.TryParse(input, out result);
            return success ? result as int? : null;
        }

        public static long? TryLongParse(string input)
        {
            long result;
            var success = long.TryParse(input, out result);
            return success ? result as long? : null;
        }
    }
}
