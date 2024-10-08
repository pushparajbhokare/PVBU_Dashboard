using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetWebService.Authentication
{
    public class LdapConfig
    {
        public string? Path { get; set; }
        public string? UseServerCredentials { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? AuthenticationType { get; set; }
        public string? Filter { get; set; }
        public bool EnableAuth { get; set; }
        public string? AuthTitle { get; set; }
        public string? AuthMail { get; set; }
        public string? AuthGivenName { get; set; }
        public string? AuthSn { get; set; }
        public string? AuthCn { get; set; }
        public string? AuthDisplayName { get; set; }
    }
}
