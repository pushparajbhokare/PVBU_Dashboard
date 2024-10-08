namespace dotnetWebService.Model
{
    public class RolePermission
    {
        public int rp_id { get; set; }

        public int role_id { get; set; }

        public string role_code { get; set; }

        public string role_name { get; set; }

        public int pr_id { get; set; }

        public string code { get; set; }

        public string permission_name { get; set; }

        public string page_code { get; set; }

        public string page_name { get; set; }

        public string description { get; set; }

        public string permission_code { get; set; }

        public bool pr_granted { get; set; }

        public int type { get; set; }
    }
}
