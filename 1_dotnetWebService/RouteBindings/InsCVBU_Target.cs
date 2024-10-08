using App.Configurations;
using System;
using System.Collections.Generic;
using System.IO;

namespace dotnetWebService.RouteBindings
{
    public class InsCVBU_Target
    {
        public static string dataFilePath;

        public static Dictionary<string, int> GetPlantData()
        {
            Dictionary<string, int> plantData = LoadPlantData();
            try
            {
                runTimeConfiguration obj = new runTimeConfiguration();
                dataFilePath = obj.getParticularConfig("TargetTxt_filepath", "path");

                if (string.IsNullOrWhiteSpace(File.ReadAllText(dataFilePath)))
                {
                    List<string> plantNames = new List<string>
            {
                obj.getParticularConfig("Pune_plant", "Plant"),
                obj.getParticularConfig("PantN_plant", "Plant"),
                obj.getParticularConfig("Jamshedpur_plant", "Plant")
            };

                    int targetValues = 0;

                    foreach (var plantName in plantNames)
                    {
                        if (!string.IsNullOrEmpty(plantName))
                        {
                            AddOrUpdatePlantValue(plantName, targetValues);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
            return plantData;
        }

        public static void AddOrUpdatePlantValue(string plantName, int value)
        {

            try
            {
                Dictionary<string, int> plantData = LoadPlantData();
                plantData[plantName] = value;
                SavePlantValues(plantData);
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
        }

        private static Dictionary<string, int> LoadPlantData()
        {
            Dictionary<string, int> plantData = new Dictionary<string, int>();
            try
            {
                if (File.Exists(dataFilePath))
                {
                    string[] lines = File.ReadAllLines(dataFilePath);
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(':');
                        if (parts.Length == 2)
                        {
                            string storedPlantName = parts[0].Trim();
                            int storedPlantValue;

                            if (int.TryParse(parts[1].Trim(), out storedPlantValue))
                            {
                                plantData[storedPlantName] = storedPlantValue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
            return plantData;
        }

        private static void SavePlantValues(Dictionary<string, int> plantData)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(dataFilePath))
                {
                    foreach (var kvp in plantData)
                    {
                        writer.WriteLine($"{kvp.Key}: {kvp.Value}");
                    }
                }
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
        }

        public static void AddPlantsFromConfiguration(string plant, int targetValue)
        {
            try
            {
                runTimeConfiguration config = new runTimeConfiguration();
                dataFilePath = config.getParticularConfig("TargetTxt_filepath", "path");

                string Pune_plant = config.getParticularConfig("Pune_plant", "Plant");
                string PantN_plant = config.getParticularConfig("PantN_plant", "Plant");
                string Jmr_plant = config.getParticularConfig("Jamshedpur_plant", "Plant");

                if (!string.IsNullOrEmpty(Pune_plant) && plant == Pune_plant)
                {
                    AddOrUpdatePlantValue(Pune_plant, targetValue);
                }
                if (!string.IsNullOrEmpty(PantN_plant) && plant == PantN_plant)
                {
                    AddOrUpdatePlantValue(PantN_plant, targetValue);
                }
                if (!string.IsNullOrEmpty(Jmr_plant) && plant == Jmr_plant)
                {
                    AddOrUpdatePlantValue(Jmr_plant, targetValue);
                }
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
