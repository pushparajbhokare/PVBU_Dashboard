using System.Collections.Generic;

namespace dotnetWebService.Model
{
    public class Role
    {
        public int role_id { get; set; }
        public string role_code { get; set; }
        public string role_name { get; set; }
        public string landing_page { get; set; }
        public int type { get; set; }
        public int status { get; set; }
        public List<RolePermission> role_permissions { get; set; }
    }
}
