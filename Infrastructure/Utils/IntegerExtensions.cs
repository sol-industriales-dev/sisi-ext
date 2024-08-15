using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    public static class IntegerExtensions
    {
        public static int ParseInt(this string value, int defaultIntValue = 0)
        {
            int parsedInt;
            if (int.TryParse(value, out parsedInt))
            {
                return parsedInt;
            }

            return defaultIntValue;
        }

        public static int? ParseNullableInt(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return value.ParseInt();
        }
        public static int ParseInt(this Object value, int defaultIntValue = 0)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch (Exception)
            {
                return defaultIntValue;
            }
        }

        public static int? ParseNullableInt(this Object value)
        {
            if (value.Equals("") || value.Equals(null))
                return null;

            return value.ParseInt();
        }
        public static decimal ParseDecimal(this Object value, decimal defaultIntValue = 0)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch (Exception)
            {
                return defaultIntValue;
            }
        }
        public static decimal ParseDecimal(this string value, decimal defaultDecValue = 0)
        {
            decimal parsedDec;
            if (decimal.TryParse(value, out parsedDec))
            {
                return parsedDec;
            }

            return defaultDecValue;
        }
        public static decimal? ParseNullableDecimal(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return value.ParseDecimal();
        }
        public static long ParseLong(this string value, int defaultIntValue = 0)
        {
            long parsedLong;
            if (long.TryParse(value, out parsedLong))
            {
                return parsedLong;
            }
            return defaultIntValue;
        }
        public static long ParseLong(this Object value, long defaultIntValue = 0)
        {
            try
            {
                return long.Parse(value.ToString());
            }
            catch (Exception)
            {
                return defaultIntValue;
            }
        }

        public static long? ParseNullableLong(this Object value)
        {
            if (value.Equals("") || value.Equals(null))
                return null;

            return value.ParseLong();
        }
        public static decimal unmaskDinero(this string result2)
        {
            result2 = result2.Replace("(", "-");
            result2 = result2.Replace(")", "");
            result2 = result2.Replace("$", "");
            result2 = result2.Replace(",", "");
            return result2.ParseDecimal();
        }
        public static decimal DivideA(this decimal divisor, decimal dividendo = 0, decimal respuestError = 0)
        {
            try { return decimal.Divide(divisor, dividendo); }
            catch (DivideByZeroException) { return respuestError; }
            catch (Exception) { return respuestError; }
        }
    }
}