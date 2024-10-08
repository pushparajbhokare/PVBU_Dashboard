using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetWebService.Authentication
{
    public class LoginRequest
    {
        public string? userId {  get; set; }
        public string? password { get; set; }
    }

    public class AddUserRequest
    {
        public string AdminPassword { get; set; }
        public string AdminUserId { get; set; }
        public string UserId { get; set; }
    }
}
