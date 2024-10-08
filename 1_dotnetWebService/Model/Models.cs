namespace dotnetWebService.Model
{
    public class CharModel
    {
        public string modelType { get; set; }
        public string productNr { get; set; }
        public string productType { get; set; }
        public string description { get; set; }
        public string partList { get; set; }
        public string OpNo { get; set; }
        public string partID { get; set; }
        public string potIndex { get; set; }
        public string criticalIndex { get; set; }
        public string xBar { get; set; }
        public string stdDev { get; set; }
        public string riskLevel { get; set; }
        public string CharClass { get; set; }
        public string CharDesc { get; set; }
        public string charNr { get; set; }
        public string valueChartImg { get; set; }
        public string qccChartImg { get; set; }
        public string histChartImg { get; set; }
    }

    public class part_report_model
    {
        public string part_report_img { get; set; }
    }

}
