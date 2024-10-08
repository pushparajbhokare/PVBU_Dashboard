using System.Collections.Generic;

namespace dotnetWebService.Model
{
    public class User 
    {

        public int id { get; set; }
        public string username { get; set; }
        public string full_name { get; set; }
        public string role_name { get; set; }
        public string email { get; set; }
        public int role_id { get; set; }
        public string plant_location { get; set; }
        public string landing_page { get; set; }
        public string area { get; set; }
        public List<RolePermission> us_permissions { get; set; }

    }
}
