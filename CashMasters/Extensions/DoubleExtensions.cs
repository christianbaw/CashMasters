using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMasters.Extensions
{
    public static class DoubleExtensions
    {
        public static double RoundUpValue(this double value)
        {
            var result = Math.Round(value, 1);
            if (result < value)
            {
                result += Math.Pow(10, -1);
            }
            return result;
        }
    }
}
