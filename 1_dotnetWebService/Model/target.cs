namespace dotnetWebService.Model
{
    public class inspection_target
    {
        public string area { get; set; }
        public string component { get; set; }
        public string model { get; set; }
        public double actual_value { get; set; }
        public string base_value { get; set; }
        public string targeted_value { get; set; }
        public string operations { get; set; }
        public string part_desc { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }
        public string plant { get; set; }
        public int partnr { get; set; }
    }
}
