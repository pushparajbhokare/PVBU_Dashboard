using dotnetWebService.Core.Service;
using dotnetWebService.Model;
using Microsoft.AspNetCore.Mvc;
using System;

namespace dotnetWebService.RouteBindings
{

    public class UserManagement
    {
        #region Variable
        ///<summary>
        ///Specify the Database variable
        ///<summary>
        private static UserMngmtService userManagementService = new UserMngmtService();
        private static RolesService rolesService = new RolesService();
        ///<summary>
        #endregion
        [HttpPost("GetUserDetailsByUsername")]
        public static User GetUserDetailsByUsername(string username)
        {
            User user = new User();
            try
            {
                user = userManagementService.GetUserDetailsByUsername(username);
                user.us_permissions = rolesService.GetRolePermissionList(user.role_id);
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
            return user;
        }

    }
}
