using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace TrueBooksMVC.Models
{
    public class EDIModel
    {
        SHIPPING_FinalEntities Context = new SHIPPING_FinalEntities();


        public object GetVoy(string rotationNo)
        {
            var VoyData = "";
            foreach (var item in Context.SP_GetEDIFile_Details(rotationNo).ToList())
            {
                if (item.Voy != null)
                {

                    VoyData = VoyData + System.Environment.NewLine + item.Voy;
                }

                if (item.BOL != null)
                {
                    VoyData = VoyData + System.Environment.NewLine + item.BOL;
                }

                if (item.CTR != null)
                {

                    VoyData = VoyData + System.Environment.NewLine + item.CTR;
                }

                if (item.CON != null)
                {
                    VoyData = VoyData + System.Environment.NewLine + item.CON;
                }
            }

            return VoyData;
        }
    }

    
}