using App.Configurations;
using dotnetWebService.BackendServices;
using dotnetWebService.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnetWebService.RouteBindings
{
    public class ManualServiceRunner
    {
        public static List<DataStorage> PlantData = new List<DataStorage>();
        public static bool plant1_data_success, plant2_data_success, plant3_data_success;
        public static DataStorage? data1, data2, data3;
        public static async Task ManualService()
        {
            try
            {
                Console.WriteLine("ManualService function");
                runTimeConfiguration config = new runTimeConfiguration();
                Task<DataStorage> task1 = Task.Run(() => PunePlant());
                Task<DataStorage> task2 = Task.Run(() => PNTPlant());
                Task<DataStorage> task3 = Task.Run(() => JMSPlant());
                await Task.WhenAll(task1, task2, task3);

                data1 = task1.Result != null && task1.Result.isListCharacteristicsAreaNotNullOrCountGreaterThanZero() ?
                    task1.Result : data1;
                data2 = task2.Result != null && task2.Result.isListCharacteristicsAreaNotNullOrCountGreaterThanZero() ?
                    task2.Result : data2;
                data3 = task3.Result != null && task3.Result.isListCharacteristicsAreaNotNullOrCountGreaterThanZero() ?
                    task3.Result : data3;

                PlantData.Clear();

                PlantData.AddRange(new[] { data1, data2, data3 });
                 
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }

        }

        private static async Task<DataStorage> PunePlant()
        {
            DataStorage data = new DataStorage();
            WebServiceManager mgr1 = null;
            runTimeConfiguration config = new runTimeConfiguration();

            try
            {
                string Pune_plant = config.getParticularConfig("Pune_plant", "Plant");
                string Pune_plantUser = config.getParticularConfig("Pune_plant", "username");
                string Pune_plantPass = config.getParticularConfig("Pune_plant", "pass");
                mgr1 = new WebServiceManager(Pune_plantUser, Pune_plantPass, ServiceReference1.Qdas_Web_ServiceClient.EndpointConfiguration.IQdas_Web_ServicePort);
                data = await AllPlantData(mgr1, Pune_plant);
                plant1_data_success = true;
            }
            catch (Exception ex)
            {
                plant1_data_success = false;
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (mgr1 != null)
                {
                    mgr1.CloseConnection();
                }

            }
            return data;
        }

        private static async Task<DataStorage> PNTPlant()
        {
            WebServiceManager mgr2 = null;
            DataStorage data = new DataStorage();
            try
            {
                runTimeConfiguration config = new runTimeConfiguration();
                string PantN_plant = config.getParticularConfig("PantN_plant", "Plant");
                string PantN_plantUser = config.getParticularConfig("PantN_plant", "username");
                string PantN_plantPass = config.getParticularConfig("PantN_plant", "pass");
                mgr2 = new WebServiceManager(PantN_plantUser, PantN_plantPass, ServiceReference1.Qdas_Web_ServiceClient.EndpointConfiguration.IQdas_Web_Pantnagar_ServicePort);
                data = await AllPlantData(mgr2, PantN_plant);
                plant2_data_success = true;
            }
            catch (Exception ex)
            {
                plant2_data_success &= false;
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (mgr2 != null)
                {
                    mgr2.CloseConnection();
                }
            }
            return data;
        }

        private static async Task<DataStorage> JMSPlant()
        {
            DataStorage data = new DataStorage();
            WebServiceManager mgr3 = null;
            try
            {
                runTimeConfiguration config = new runTimeConfiguration();
                string Jmr_plant = config.getParticularConfig("Jamshedpur_plant", "Plant");
                string Jmr_plantUser = config.getParticularConfig("Jamshedpur_plant", "username");
                string Jmr_plantPass = config.getParticularConfig("Jamshedpur_plant", "pass");
                mgr3 = new WebServiceManager(Jmr_plantUser, Jmr_plantPass, ServiceReference1.Qdas_Web_ServiceClient.EndpointConfiguration.IQdas_Web_Jamshedpur_ServicePort);
                data = await AllPlantData(mgr3, Jmr_plant);
                plant3_data_success = true;
            }
            catch (Exception ex)
            {
                plant3_data_success = false;
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (mgr3 != null)
                {
                    mgr3.CloseConnection();
                }
            }
            return data;
        }

        public static async Task<DataStorage> AllPlantData(WebServiceManager _mgr, string plant)
        {
            DataStorage dataStorage = new DataStorage();
            //try
            //{
            Console.WriteLine("AllPlant function.....");
            Console.WriteLine("ManualServiceRunner _mgr......");
            try
            {
                Console.WriteLine(plant + " DwnEmptStucture page start......");
                FileWriter.WriteToFile(plant + " plant DwnEmptStucture page start...");
                await DwnStucture.DwnEmptStucture(_mgr, plant);
                Console.WriteLine(plant + " DwnEmptStucture page end......");
                FileWriter.WriteToFile(plant + " plant DwnEmptStucture page end...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                FileWriter.WriteToFile(ex.Message);
            }

            try
            {
                Console.WriteLine(plant + " plant characteristics page start...");
                FileWriter.WriteToFile(plant + " plant characteristics page start...");
                var charDetailsTuple = await CharacteristicsExplorer.GetCharDetailsPlantwise(_mgr, plant);
                Console.WriteLine(plant + " plant characteristics page end...");
                FileWriter.WriteToFile(plant + " plant characteristics page end...");

                dataStorage.CharInfoData = new List<Characteristic>();
                dataStorage.CritDifferenceData = new List<Diffrence_crit>();
                dataStorage.WaterfallData = new List<Diagram>();

                if (charDetailsTuple != null && charDetailsTuple.Item1 != null)
                {
                    dataStorage.CharInfoData.AddRange(charDetailsTuple.Item1);
                }
                if (charDetailsTuple != null && charDetailsTuple.Item2 != null)
                {
                    dataStorage.CritDifferenceData.AddRange(charDetailsTuple.Item2);
                }
                if (charDetailsTuple != null && charDetailsTuple.Item3 != null)
                {
                    dataStorage.WaterfallData.AddRange(charDetailsTuple.Item3);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                FileWriter.WriteToFile(ex.Message);
            }

            try
            {
                Console.WriteLine(plant + " plant Qudrant page start...");
                FileWriter.WriteToFile(plant + " plant Qudrant page start...");
                var QuadrantTuple = await TestQuadrant.GetQudrantDetails(_mgr, plant);
                FileWriter.WriteToFile(plant + " plant Qudrant page end...");
                Console.WriteLine(plant + " plant Qudrant page end...");

                dataStorage.QuadrantData = new List<Characteristicsqudrant>();
                dataStorage.PlantQuadrantData = new List<TrendCount>();
                dataStorage.CVBUQuadrantData = new List<CVBUTrendCount>();

                if (QuadrantTuple != null && QuadrantTuple.Item2 != null)
                {
                    dataStorage.QuadrantData.AddRange(QuadrantTuple.Item2);
                }
                if (QuadrantTuple != null && QuadrantTuple.Item1 != null)
                {
                    dataStorage.PlantQuadrantData.AddRange(QuadrantTuple.Item1);
                }
                if (QuadrantTuple != null && QuadrantTuple.Item3 != null)
                {
                    dataStorage.CVBUQuadrantData.AddRange(QuadrantTuple.Item3);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                FileWriter.WriteToFile(ex.Message);
            }

            try
            {
                FileWriter.WriteToFile(plant + " plant AreaExplorer page start...");
                Console.WriteLine(plant + " plant AreaExplorer page start...");
                var AreaTuple = await AreaExplorer.GetAreaExplorer(_mgr, plant);//.GetAwaiter().GetResult();
                FileWriter.WriteToFile(plant + " plant AreaExplorer page end...");
                Console.WriteLine(plant + " plant AreaExplorer page end...");

                dataStorage.AreaExplorerData = new List<AllAreaExplorer>();

                if (AreaTuple.Item1 != null && AreaTuple != null)
                {
                    dataStorage.AreaExplorerData.AddRange(AreaTuple.Item1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                FileWriter.WriteToFile(ex.Message);
            }

            try
            {
                FileWriter.WriteToFile(plant + " plant ModelExplorer page start...");
                Console.WriteLine(plant + " plant ModelExplorer page start...");
                var ModelExPlorerTuple = await ModelExplorer.CalculateModelExplorer(_mgr, plant);
                FileWriter.WriteToFile(plant + " plant ModelExplorer page end...");
                Console.WriteLine(plant + " plant ModelExplorer page end...");

                dataStorage.ModelExplorerData = new List<ModelExplorer_Model>();

                if (ModelExPlorerTuple.Item1 != null && ModelExPlorerTuple != null)
                {
                    dataStorage.ModelExplorerData.AddRange(ModelExPlorerTuple.Item1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                FileWriter.WriteToFile(ex.Message);
            }

            try
            {
                Console.WriteLine(plant + " plant Inspection page start...");
                FileWriter.WriteToFile(plant + " plant Inspection page start...");
                var InspectionTuple = await Inspection.GetInspection(_mgr, plant);//.GetAwaiter().GetResult(); //WebServiceManager webx
                Console.WriteLine(plant + " plant Inspection page end...");
                FileWriter.WriteToFile(plant + " plant Inspection page end...");

                dataStorage.InspectionData = new List<inspection_target>();

                if (InspectionTuple.Item1 != null && InspectionTuple != null)
                {
                    dataStorage.InspectionData.AddRange(InspectionTuple.Item1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                FileWriter.WriteToFile(ex.Message);
            }
            return dataStorage;
        }
    }
}
