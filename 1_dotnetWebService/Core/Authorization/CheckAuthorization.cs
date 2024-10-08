using App.Configurations;
using dotnetWebService.RouteBindings;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace dotnetWebService.Core.CheckAuthorization
{
    public static  class CheckAuthorization
    {
        public static bool CheckUserAuthorization(string permission_code, ClaimsPrincipal user)
        {
            string username = user.Claims.First(c => "UserId".Equals(c.Type)).Value;
            if (username != null)
            {
                var userDetail = UserManagement.GetUserDetailsByUsername(username);
                if (userDetail.us_permissions.Exists(item => item.permission_code == permission_code && item.pr_granted))
                {
                    return true;
                }
            }
            return false;
        }
        
    }


}
