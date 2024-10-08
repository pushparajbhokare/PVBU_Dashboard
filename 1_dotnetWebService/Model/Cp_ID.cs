using System.Collections.Generic;

namespace dotnetWebService.Model
{
    public class Cp_ID
    {
        public static List<Cplan> CP_ID_PNE = new List<Cplan>();
        public static List<Cplan> CP_ID_PNT = new List<Cplan>();
        public static List<Cplan> CP_ID_JMS = new List<Cplan>();
        public class Cplan 
        {
            public string Cplan_ID { get; set; }
        }
    }
}
