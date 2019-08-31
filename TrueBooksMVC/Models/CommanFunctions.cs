using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public static class CommanFunctions
    {
       
            public static string GetShortDateFormat(object iInputDate)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(iInputDate)))
                {
                    return String.Format("{0:dd MMM yyyy}", Convert.ToDateTime(iInputDate));
                }
                return "";
            }

      
    }
}