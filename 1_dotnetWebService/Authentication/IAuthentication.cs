using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetWebService.Authentication
{
    public interface IAuthentication
    {
        AuthUser Login(string userName, string password);
        AuthUser AddUserByAdmin(string adminPassword,string adminUserId, string userId);  
    }
}
