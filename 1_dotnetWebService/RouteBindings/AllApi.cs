using dotnetWebService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace dotnetWebService.RouteBindings
{
    public class AllApi
    {
        public static List<inspection_target> Test_Inspection()
        {
            List<inspection_target> allInspectionData = new List<inspection_target>();
            try
            {
                foreach (var dataStorage in ManualServiceRunner.PlantData)
                {
                    if (dataStorage != null && dataStorage.InspectionData != null)
                    {
                        allInspectionData.AddRange(dataStorage.InspectionData);
                    }
                    else
                    {
                        Console.WriteLine("List is null");
                    }
                }
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
            return allInspectionData;
        }

        public static async Task<List<AllAreaExplorer>> Test_area_explorer()
        {
            List<AllAreaExplorer> allAreaData = new List<AllAreaExplorer>();
            try
            {
                foreach (var dataStorage in ManualServiceRunner.PlantData)
                {
                    if (dataStorage != null && dataStorage.AreaExplorerData != null)
                    {
                        allAreaData.AddRange(dataStorage.AreaExplorerData);
                    }
                    else
                    {
                        Console.WriteLine("List is null");
                    }
                }
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }

            return allAreaData;
        }

        public static async Task<List<Characteristic>> Test_char_info()
        {

            List<Characteristic> allCharData = new List<Characteristic>();
            try
            {
                foreach (var dataStorage in ManualServiceRunner.PlantData)
                {
                    if (dataStorage != null && dataStorage.CharInfoData != null)
                    {
                        allCharData.AddRange(dataStorage.CharInfoData);
                    }
                    else
                    {
                        Console.WriteLine("List is null");
                    }
                }
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }

            return allCharData;
        }

        public static async Task<List<TrendCount>> Test_plant_quadrant()
        {
            List<TrendCount> allPlantTrendCount = new List<TrendCount>();
            try
            {
                foreach (var dataStorage in ManualServiceRunner.PlantData)
                {
                    if (dataStorage != null && dataStorage.PlantQuadrantData != null)
                    {
                        allPlantTrendCount.AddRange(dataStorage.PlantQuadrantData);
                    }
                    else
                    {
                        Console.WriteLine("List is null");
                    }
                }
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
            return allPlantTrendCount;
        }
        public static async Task<TotalCounts> Test_waterfall_dia()
        {
            List<Diagram> allWaterfalDiaData = new List<Diagram>();
            TotalCounts totalCounts = new TotalCounts();
            try
            {
                double totalCurCount = 0;
                double totalLastCount = 0;
                double totalDecrCount = 0;
                double totalIncCount = 0;
                string lastMonth = "";
                string currentMonth = "";

                foreach (var dataStorage in ManualServiceRunner.PlantData)
                {
                    if (dataStorage != null && dataStorage.WaterfallData != null)
                    {
                        allWaterfalDiaData.AddRange(dataStorage.WaterfallData);

                        foreach (var waterfallData in dataStorage.WaterfallData)
                        {
                            totalCurCount += waterfallData.cur_count;
                            totalLastCount += waterfallData.last_count;
                            totalDecrCount += waterfallData.decr_count;
                            totalIncCount += waterfallData.inc_count;
                            currentMonth = waterfallData.currentMonth;
                            lastMonth = waterfallData.lastMonth;
                        }
                    }
                    else
                    {
                        Console.WriteLine("List is null");
                    }
                }

                totalCounts = new TotalCounts
                {
                    cur_count = totalCurCount,
                    last_count = totalLastCount,
                    decr_count = totalDecrCount,
                    inc_count = totalIncCount,
                    lastMonth = lastMonth,
                    currentMonth = currentMonth,
                };
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }

            return totalCounts;
        }


        public static async Task<List<Diffrence_crit>> Crit_Diffrence()
        {
            List<Diffrence_crit> allTopCrit = new List<Diffrence_crit>();
            try
            {
                foreach (var dataStorage in ManualServiceRunner.PlantData)
                {
                    if (dataStorage != null && dataStorage.CritDifferenceData != null)
                    {
                        allTopCrit.AddRange(dataStorage.CritDifferenceData);
                    }
                    else
                    {
                        Console.WriteLine("List is null");
                    }
                }
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }

            return allTopCrit;
        }
        public static async Task<List<CVBUTrendCount>> Test_CVBU_quadrant()
        {
            List<CVBUTrendCount> allCVBUTrendCount = new List<CVBUTrendCount>();
            List<CVBUTrendCount> groupedCharDemoPlant = new List<CVBUTrendCount>();

            try
            {
                foreach (var dataStorage in ManualServiceRunner.PlantData)
                {
                    if (dataStorage != null && dataStorage != null && dataStorage.CVBUQuadrantData != null)
                    {
                        allCVBUTrendCount.AddRange(dataStorage.CVBUQuadrantData);
                    }
                    else
                    {
                        Console.WriteLine("List is null");
                    }
                }
                groupedCharDemoPlant = allCVBUTrendCount
                    .GroupBy(item => new { item.Month })
                    .Select(group => new CVBUTrendCount
                    {
                        PlantQuad1 = group.Sum(item => item.PlantQuad1),
                        PlantQuad2 = group.Sum(item => item.PlantQuad2),
                        PlantQuad3 = group.Sum(item => item.PlantQuad3),
                        PlantQuad4 = group.Sum(item => item.PlantQuad4),
                        Month = group.Key.Month,
                        PlantTotal = group.Sum(item => item.PlantTotal),
                    }).ToList();
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }

            return groupedCharDemoPlant;
        }

        public static async Task<List<ModelExplorer_Model>> Test_model_explorer()
        {
            List<ModelExplorer_Model> allModelExplorerData = new List<ModelExplorer_Model>();
            try
            {
                foreach (var dataStorage in ManualServiceRunner.PlantData)
                {
                    if (dataStorage != null && dataStorage.ModelExplorerData != null)
                    {
                        allModelExplorerData.AddRange(dataStorage.ModelExplorerData);
                    }
                    else
                    {
                        Console.WriteLine("List is null");
                    }
                }
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }

            return allModelExplorerData;
        }
        public static async Task<List<Characteristicsqudrant>> Test_quadrant()
        {
            List<Characteristicsqudrant> allCharQudrantData = new List<Characteristicsqudrant>();
            try
            {
                foreach (var dataStorage in ManualServiceRunner.PlantData)
                {
                    if (dataStorage != null && dataStorage.QuadrantData != null)
                    {
                        allCharQudrantData.AddRange(dataStorage.QuadrantData);
                    }
                    else
                    {
                        Console.WriteLine("List is null");
                    }
                }
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
            return allCharQudrantData;
        }
    }
}
