using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueBooksMVC.Models;
using System.Data.SqlClient;
using System.Data;
using DAL;


namespace TrueBooksMVC.Models
{
    public class RegistrationModel
    {
        SHIPPING_FinalEntities Context1 = new SHIPPING_FinalEntities();

        public int AddUser(UserRegistration UR)
        {
            int query = Convert.ToInt32(Context1.SP_InsertUserDetails(UR.UserName, UR.Password));
            return query;
        }

        public int EditUser(UserRegistration UR)
        {
            int query = Convert.ToInt32(Context1.SP_EditUserDetails(UR.UserID, UR.UserName, UR.Password));
            return query;
        }

        public int DeleteUser(int id)
        {
            int query = Convert.ToInt32(Context1.SP_DeleteUser(id));
            return query;
        }

        public UserRegistration GetUsetDetailByID(int id)
        {
            UserRegistration UR1 = new UserRegistration();

            var query = Context1.SP_GetUserDetailForEdit(id).ToList();

            foreach (var item in query)
            {
                UR1.UserID = item.UserID;
                UR1.UserName = item.Username;
                UR1.Password = item.Password;

                
            }

            return UR1;
        }

        public List<SP_GetUserDetails_Result> GetUserDetails()
        {
            
            var query = Context1.SP_GetUserDetails().ToList();

            return query;
        }

        public List<SP_LoginUser_Result> LoginUser(UserRegistration UR)
        {

            var query = Context1.SP_LoginUser(UR.UserName, UR.Password).ToList();

            return query;
        }

    }
}