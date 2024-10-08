using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Services.Description;

namespace dotnetWebService.Model
{
    public class DataStorage
    {
        public List<inspection_target> InspectionData { get; set; }
        public List<Characteristic> CharInfoData { get; set; }
        public List<Characteristicsqudrant> QuadrantData { get; set; }
        public List<TrendCount> PlantQuadrantData { get; set; }
        public List<Diagram> WaterfallData { get; set; }
        public List<Diffrence_crit> CritDifferenceData { get; set; }
        public List<CVBUTrendCount> CVBUQuadrantData { get; set; }
        public List<AllAreaExplorer> AreaExplorerData { get; set; }
        public List<ModelExplorer_Model> ModelExplorerData { get; set; }
        
        public bool isListCharacteristicsAreaNotNullOrCountGreaterThanZero()
        {
            return CharInfoData != null && CharInfoData.Count > 0 &&
            AreaExplorerData != null && AreaExplorerData.Count > 0;
        }
    }
}
