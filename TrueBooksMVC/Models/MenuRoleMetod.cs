using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace TrueBooksMVC.Models
{
    public class MenuRoleMetod
    {
        SHIPPING_FinalEntities entity = new SHIPPING_FinalEntities();
        public List<MenuRoleVM> GetAllMenu()
        {

            List<MenuRoleVM> model = new List<MenuRoleVM>();
            var query = (from t in entity.Menus
                         join t1 in entity.AspNetRoles on t.RoleID equals t1.Id
                         select new MenuRoleVM

                         {
                             Title = t.Title,
                             Name = t1.Name

                         }).ToList();


            return query;
        }
    }
}