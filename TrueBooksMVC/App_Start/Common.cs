using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace TrueBooksMVC
{
    public class Common
    {
        public static string GetConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
        }

        public static int ParseInt(string str)
        {
            int k = 0;
            if (Int32.TryParse(str, out k))
            {
                return k;
            }
            return 0;
        }
        public static Decimal ParseDecimal(string str)
        {
            Decimal k = 0;
            if (Decimal.TryParse(str, out k))
            {
                return k;
            }
            return 0;
        }
      
        public static DateTime ParseDate(string str,string Format = "dd-MMM-yyyy")
        {
            DateTime dt = DateTime.MinValue;
            if (DateTime.TryParseExact(str, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return dt;
            }
            return dt;
        }

    }
}