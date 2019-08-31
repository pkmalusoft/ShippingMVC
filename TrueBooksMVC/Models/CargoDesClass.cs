using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueBooksMVC.Models
{
    public class CargoDesClass
    {
        int id;
        string marks;
        string Description;
        string weight;
        string Volume;
        string packages;
        string GrossWeight;

        public CargoDesClass()
        {
            this.id = 0;
            this.marks = null;
            this.Description = null;
            this.weight = null;
            this.Volume = null;
            this.packages = null;
            this.GrossWeight = null;
        }

        public CargoDesClass(int id, string mar, string Descpt, string Wght, string Vol, string pckge, string Gweght)
        {
            this.id = id;
            this.marks = mar;
            this.Description = Descpt;
            this.weight = Wght;
            this.Volume = Vol;
            this.packages = pckge;
            this.GrossWeight = Gweght;
        }



    }
}