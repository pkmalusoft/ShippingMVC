using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class DateTimeZoneConversionModel
    {
        public string ConvertDateTimeZone(DateTime Userdate)
        {
            DateTime serverDateTime = Userdate;

            DateTime dbDateTime = serverDateTime.ToUniversalTime();

            DateTimeOffset dbDateTimeOffset = new DateTimeOffset(dbDateTime, TimeSpan.Zero);

            TimeZoneInfo userTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

            DateTimeOffset userDateTimeOffset = TimeZoneInfo.ConvertTime(dbDateTimeOffset, userTimeZone);

            string userDateTimeString = userDateTimeOffset.ToString("dd-MM-yyyy HH:mm:ss");

            return userDateTimeString;

        }
    }
}