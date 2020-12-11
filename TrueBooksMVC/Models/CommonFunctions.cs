using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using DAL;
//using System.Data.Objects;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
namespace TrueBooksMVC.Models
{
    public class CommonFunctions
    {
        public static string GetConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            }
        }
        public static DateTime ParseDate(string str, string Format = "dd-MMM-yyyy")
        {
            DateTime dt = DateTime.MinValue;
            if (DateTime.TryParseExact(str, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return dt;
            }
            return dt;
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

        public static string GetMinFinancialDate()
        {
            SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();

            int fyearid = Convert.ToInt32(HttpContext.Current.Session["fyearid"].ToString());

            DateTime startdate = Convert.ToDateTime(db.AcFinancialYears.Find(fyearid).AcFYearFrom);

            string ss = "";
            if (startdate != null)
                ss = startdate.Year + "/" + startdate.Month + "/" + startdate.Day; // string.Format("{0:YYYY MM dd}", (object)startdate.ToString());

            return ss;
        }
        public static string GetMaxFinancialDate()
        {
            SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();

            int fyearid = Convert.ToInt32(HttpContext.Current.Session["fyearid"].ToString());

            DateTime startdate = Convert.ToDateTime(db.AcFinancialYears.Find(fyearid).AcFYearTo);
            string ss = "";
            if (startdate != null)
                ss = startdate.Year + "/" + startdate.Month + "/" + startdate.Day; // string.Format("{0:YYYY MM dd}", (object)startdate.ToString());

            return ss;
        }

        public static string GetShortDateFormat(object iInputDate)
        {
            if (iInputDate != null)
                return string.Format("{0:dd MMM yyyy}", (object)Convert.ToDateTime(iInputDate));
            return "";
        }
        public static bool CheckCreateEntryValid()
        {
            SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
            int currentfyearid = db.AcFinancialYears.FirstOrDefault().AcFinancialYearID;
            int fyearid = Convert.ToInt32(HttpContext.Current.Session["fyearid"].ToString());
            if (currentfyearid != fyearid)
                return false;
            return true;
        }

        public static DateTime GetFirstDayofMonth()
        {
            SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();

            int fyearid = Convert.ToInt32(HttpContext.Current.Session["fyearid"].ToString());
            DateTime startdate = Convert.ToDateTime(db.AcFinancialYears.Find(fyearid).AcFYearFrom);
            DateTime enddate = Convert.ToDateTime(db.AcFinancialYears.Find(fyearid).AcFYearTo);

            string vdate = "01" + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();
            DateTime todaydate = DateTime.Now.Date;

            return Convert.ToDateTime(vdate);

            //if (todaydate>=startdate && todaydate <=enddate ) //current date between current financial year
            //    return Convert.ToDateTime(vdate);
            //else
            //{
            //    vdate = "01" + "-" + enddate.Month.ToString() + "-" + enddate.Year.ToString();
            //    return Convert.ToDateTime(vdate);
            //}

        }

        public static DateTime GetLastDayofMonth()
        {
            SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();

            int fyearid = Convert.ToInt32(HttpContext.Current.Session["fyearid"].ToString());
            DateTime startdate = Convert.ToDateTime(db.AcFinancialYears.Find(fyearid).AcFYearFrom);
            DateTime enddate = Convert.ToDateTime(db.AcFinancialYears.Find(fyearid).AcFYearTo);

            DateTime todaydate = DateTimeOffset.Now.Date; // DateTime.Now.Date;            
            return todaydate;
            //if (todaydate >= startdate && todaydate <= enddate) //current date between current financial year
            //    return todaydate;
            //else
            //{                
            //    return enddate;
            //}

        }
        public static string GetLongDateFormat(object iInputDate)
        {
            if (iInputDate != null)
                return string.Format("{0:dd MMM yyyy hh:mm}", (object)Convert.ToDateTime(iInputDate));
            return "";
        }

        public static string GetDecimalFormat(object iInputValue, string Decimals)
        {
            if (Decimals == "2")
            {
                if (iInputValue != null)
                    return String.Format("{0:0.00}", (object)Convert.ToDecimal(iInputValue));
            }
            else if (Decimals == "3")
            {
                if (iInputValue != null)
                    return String.Format("{0:0.000}", (object)Convert.ToDecimal(iInputValue));
            }
            return "";
        }

        public static string GetCurrencyId(int CurrencyId)
        {
            SHIPPING_FinalEntities db = new SHIPPING_FinalEntities();
            try
            {
                string currencyname = db.CurrencyMasters.Find(CurrencyId).CurrencyName;

                return currencyname;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static string GetFormatNumber(object iInputValue, string Decimals)
        {
            if (Decimals == "2")
            {

                if (iInputValue != null)
                {
                    decimal v = 0;
                    v = Decimal.Parse(((object)Convert.ToDecimal(iInputValue)).ToString());
                    if (v > 0)
                        return String.Format("{0:#,0.00}", (object)Convert.ToDecimal(iInputValue));
                    else
                        return "";
                }
            }
            else if (Decimals == "3")
            {
                if (iInputValue != null)
                    return String.Format("{0:#,0.000}", (object)Convert.ToDecimal(iInputValue));
            }
            return "";

        }
    }
}