namespace dotnetWebService.Model
{
    public class Characteristic
    {
        public string operation { get; set; }
        public string part_id { get; set; }
        public string plant_name { get; set; }
        public string part_desc { get; set; }
        public string char_id { get; set; }
        public string parameter { get; set; }
        public string mclass { get; set; }
        public string quadrant { get; set; }
        public string quadrant_value { get; set; }
        public string parameter_desc { get; set; }
        public int status { get; set; }
        public string area { get; set; }
        public string component { get; set; }
        public string model { get; set; }
        public string xBar { get; set; }
        public string unit { get; set; }
        public string stdDev { get; set; }
        public string range { get; set; }
        public string potIndex { get; set; }
        public string criticalIndex { get; set; }
    }

    public class month_record
    {
        public string plant_name { get; set; }
        public string parameter_desc { get; set; }
        public string Cplan { get; set; }
        public int char_id { get; set; }
        public string criticalIndex { get; set; }
        public string component { get; set; }
        public string model { get; set; }
        public string month { get; set; }
        public string Date { get; set; }
    }

    public class critcal_record
    {
        public string plant_name { get; set; }
        public string parameter_desc { get; set; }
        public string Cplan { get; set; }
        public int char_id { get; set; }
        public string criticalIndex { get; set; }
        public string component { get; set; }
        public string model { get; set; }
        public string month { get; set; }
        public string Date { get; set; }
    }

    public class TotalCounts
    {
        public double cur_count { get; set; }
        public double last_count { get; set; }
        public double decr_count { get; set; }
        public double inc_count { get; set; }
        public string lastMonth { get; set; }
        public string currentMonth { get; set; }
    }
    public class Diagram
    {
        public double last_count { get; set; }
        public double decr_count { get; set; }
        public double inc_count { get; set; }
        public double cur_count { get; set; }
        public string lastMonth { get; set; }
        public string currentMonth { get; set; }
        public string plant { get; set; }
    }


    public class Characteristicsqudrant
    {
        public string plant { get; set; }
        public double Quad1 { get; set; }
        public double Quad2 { get; set; }
        public double Quad3 { get; set; }
        public double Quad4 { get; set; }
        public double Total { get; set; }
        public string Month { get; set; }
    }

    public class TrendCount
    {
        public string plant { get; set; }
        public string cplan { get; set; }
        public double PlantQuad1 { get; set; }
        public double PlantQuad2 { get; set; }
        public double PlantQuad3 { get; set; }
        public double PlantQuad4 { get; set; }
        public double PlantTotal { get; set; }
        public string Month { get; set; }
        public string Date { get; set; }
    }
    public class CVBUTrendCount
    {
        public string plant { get; set; }
        public string cplan { get; set; }
        public double PlantQuad1 { get; set; }
        public double PlantQuad2 { get; set; }
        public double PlantQuad3 { get; set; }
        public double PlantQuad4 { get; set; }
        public double PlantTotal { get; set; }
        public string Month { get; set; }
        public string Date { get; set; }
    }

    public class Diffrence_crit
    {
        public string plant { get; set; }
        public string parameter_desc { get; set; }
        public string component { get; set; }
        public string model { get; set; }
        public string Cplan { get; set; }
        public int char_id { get; set; }
        public double LastMonthcriticalIndex { get; set; }
        public double CurrentMonthcriticalIndex { get; set; }
        public double DiffrenceMonthcriticalIndex { get; set; }
    }
}

