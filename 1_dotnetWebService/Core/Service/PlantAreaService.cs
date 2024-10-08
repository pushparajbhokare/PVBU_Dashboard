using App.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetWebService.Core.Service
{
    public static class PlantAreaService
    {
        public static string GetPlantList()
        {
            runTimeConfiguration configManager = new runTimeConfiguration();

            return configManager.getParticularConfig("plant_List", "Plant");
        }

        public static string GetAreaList()
        {
            runTimeConfiguration configManager = new runTimeConfiguration();

            return configManager.getParticularConfig("Area_List", "Area");
        }
    }
}
