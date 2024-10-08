using App.Configurations;
using dotnetWebService.BackendServices;
using dotnetWebService.Model;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static dotnetWebService.Model.Cp_ID;

namespace dotnetWebService.RouteBindings
{
    public class Inspection
    {
        public async static Task<Tuple<List<inspection_target>>> GetInspection(WebServiceManager webx, string plant)
        {
            //try
            //{
            List<inspection_target> test = new List<inspection_target>();
            List<Cplan> Cp_obj = new List<Cplan>();
            Console.WriteLine("GetInspection started..");

            runTimeConfiguration config = new runTimeConfiguration();
            string Pune_plant = config.getParticularConfig("Pune_plant", "Plant");
            string PantN_plant = config.getParticularConfig("PantN_plant", "Plant");
            string Jmr_plant = config.getParticularConfig("Jamshedpur_plant", "Plant");
            string day = config.getParticularConfig("add_days", "day");

            var ws = webx.ws;
            var response = webx.response;
            if (response == null)
            {
                FileWriter.WriteToFile(plant + "Plant response in Null ");
                return Tuple.Create(test);
            }
            if (response.Handle == 0)
            {
                FileWriter.WriteToFile(plant + "Plant handle is zero " + response.Handle);
                Console.WriteLine(plant + "Plant handle is zero " + response.Handle);
                return Tuple.Create(test);
            }

            if (response.Handle <= 0)
            {
                return Tuple.Create(test);
            }
            Console.WriteLine(ws.Endpoint.Name);
            Console.WriteLine(webx.mainHandle);
            string partID1 = "";
            string charID1 = "";
            int handle = webx.mainHandle;

            List<Cplan> Cp_Id_Obj = new List<Cplan>();
            if (plant == Pune_plant)
            {
                Cp_Id_Obj.Clear();
                foreach (Cp_ID.Cplan cpPlan in Cp_ID.CP_ID_PNE)
                {
                    Cp_Id_Obj.Add(new Cplan { Cplan_ID = cpPlan.Cplan_ID });
                }
            }
            else if (plant == PantN_plant)
            {
                Cp_Id_Obj.Clear();
                foreach (Cp_ID.Cplan cpPlan in Cp_ID.CP_ID_PNT)
                {
                    Cp_Id_Obj.Add(new Cplan { Cplan_ID = cpPlan.Cplan_ID });
                }
            }
            else if (plant == Jmr_plant)
            {
                Cp_Id_Obj.Clear();
                foreach (Cp_ID.Cplan cpPlan in Cp_ID.CP_ID_JMS)
                {
                    Cp_Id_Obj.Add(new Cplan { Cplan_ID = cpPlan.Cplan_ID });
                }
            }

            foreach (Cplan cpPlan in Cp_Id_Obj)
            {
                string currentCpId = cpPlan.Cplan_ID;
                DateTime scheduledTime = DateTime.Today;//DateTime.Today.AddDays(-1);
                int day1 = Convert.ToInt32(day);
                DateTime lastMonday = scheduledTime.AddDays(-day1);
                DateTime lastSunday = scheduledTime.AddMilliseconds(-1);

                string formattedLastMonday = lastMonday.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string formattedLastSunday = lastSunday.ToString("yyyy-MM-dd HH:mm:ss.fff");

                string partListStr = "<Part key = '" + partID1 + "'><Char key='" + charID1 + "'/></Part>";
                CreateQueryRequest requestChart1 = new CreateQueryRequest(response.Handle);
                var graphicQR = await ws.CreateQueryAsync(requestChart1);
                int queryHandle = graphicQR.QueryHandle;
                int result = graphicQR.Result;

                CreateFilterRequest requestChart3 = new CreateFilterRequest(response.Handle, 1, 1100, plant, 0);
                var resultChart3 = await ws.CreateFilterAsync(requestChart3);
                var filterHandleforplant = resultChart3.FilterHandle;
                result = resultChart3.Result;

                var resultChart4 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, filterHandleforplant, 0, 0, 0);
                result = resultChart4.Result;

                CreateFilterRequest requestChart212 = new CreateFilterRequest(response.Handle, 1, 1000, currentCpId, 0);
                var resultChart212 = await ws.CreateFilterAsync(requestChart212);
                var cplan_filter = resultChart212.FilterHandle;
                result = resultChart212.Result;

                AddFilterToQueryRequest requestChar312 = new AddFilterToQueryRequest();
                var resultChar312 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, cplan_filter, 0, 0, 0);
                result = resultChar312.Result;

                CreateFilterRequest requestChart31 = new CreateFilterRequest(response.Handle, 1, 4, formattedLastMonday, 4);
                var resultChart31 = await ws.CreateFilterAsync(requestChart31);
                var filterHandleforformdate = resultChart31.FilterHandle;
                result = resultChart31.Result;

                var resultChart41 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, filterHandleforformdate, 2, 0, 0);
                result = resultChart41.Result;

                CreateFilterRequest requestChart32 = new CreateFilterRequest(response.Handle, 1, 4, formattedLastSunday, 3);
                var resultChart32 = await ws.CreateFilterAsync(requestChart32);
                var filterHandlefortodate = resultChart32.FilterHandle;
                result = resultChart32.Result;

                var resultChart42 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, filterHandlefortodate, 2, 0, 0);
                result = resultChart42.Result;

                CreateFilterRequest request4 = new CreateFilterRequest(response.Handle, 1, 2005, "0", 2);
                var resultChar4 = await ws.CreateFilterAsync(request4);
                var Class_Handle = resultChar4.FilterHandle;
                result = resultChar4.Result;

                AddFilterToQueryRequest request5 = new AddFilterToQueryRequest();
                var resultChar5 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, Class_Handle, 1, 0, 0);
                result = resultChar5.Result;

                ExecuteQueryRequest requestChart7 = new ExecuteQueryRequest();
                var resultChart7 = await ws.ExecuteQueryAsync(response.Handle, queryHandle, partListStr);
                try
                {
                    if (resultChart7 != null)
                    {
                        result = resultChart7.Result;
                    }
                    else
                    {
                        FileWriter.WriteToFile(plant + "Plant Inspection page ExecuteQueryRequest is null");
                        Console.WriteLine(plant + "Plant Inspection page ExecuteQueryRequest is null");
                    }
                }
                catch (Exception ex)
                {
                    FileWriter.WriteToFile(plant + "Plant Inspection page ExecuteQueryRequest Exception :" + ex.Message);
                    Console.WriteLine(plant + "Plant Inspection page ExecuteQueryRequest Exception :" + ex.Message);
                }

                FreeQueryRequest freeQueryRequest1 = new FreeQueryRequest();
                var resultChart51 = await ws.FreeQueryAsync(response.Handle, queryHandle);
                result = resultChart51.Result;

                CreateFilterRequest requestChart77 = new CreateFilterRequest();
                var resultChart77 = await ws.EvaluateAllCharsAsync(response.Handle);
                var result2 = resultChart77.Result;

                GetGlobalInfoRequest requestChart8 = new GetGlobalInfoRequest(response.Handle, 1, 0, 1);
                var resultChart8 = await ws.GetGlobalInfoAsync(requestChart8);
                result = resultChart8.Result;
                var partCount = resultChart8.ret;

                GetPartInfoRequest requestChart8231 = new GetPartInfoRequest(response.Handle, 1100, 1, 0);
                var resultChart8231 = await ws.GetPartInfoAsync(requestChart8231);
                var plantname = resultChart8231.KFieldValue;

                Console.WriteLine("Part Count......" + partCount);
                int i;
                Console.WriteLine(plant + " plant Inspection page i loop start partCount: " + partCount);
                FileWriter.WriteToFile(plant + " plant Inspection page i loop start partCount: " + partCount);
                for (i = 1; i <= partCount; i++)
                {
                    Console.WriteLine(plant + " plant Inspection page i loop Running part: " + i);
                    GetPartInfoRequest requestChart80 = new GetPartInfoRequest(response.Handle, 1003, 1, 0);
                    var resultChart80 = await ws.GetPartInfoAsync(requestChart80);
                    var area = resultChart80.KFieldValue;

                    GetPartInfoRequest requestChart823 = new GetPartInfoRequest(response.Handle, 1000, 1, 0);
                    var resultChart823 = await ws.GetPartInfoAsync(requestChart823);
                    var part_id = resultChart823.KFieldValue;

                    GetPartInfoRequest requestChart56 = new GetPartInfoRequest(response.Handle, 1005, 1, 0);
                    var resultChart56 = await ws.GetPartInfoAsync(requestChart56);
                    var Component = resultChart56.KFieldValue;

                    GetPartInfoRequest requestChart57 = new GetPartInfoRequest(response.Handle, 1008, 1, 0);
                    var resultChart57 = await ws.GetPartInfoAsync(requestChart57);
                    var Model = resultChart57.KFieldValue;

                    GetPartInfoRequest requestChart58 = new GetPartInfoRequest(response.Handle, 1086, 1, 0);
                    var resultChart58 = await ws.GetPartInfoAsync(requestChart58);
                    var Operation = resultChart58.KFieldValue;

                    GetGlobalInfoRequest requestChart91 = new GetGlobalInfoRequest(response.Handle, 1, 0, 3);
                    var resultChart91 = await ws.GetGlobalInfoAsync(requestChart91);
                    result = resultChart91.Result;
                    var char_count = resultChart91.ret;

                    GetStatResultExRequest requestChartE = new GetStatResultExRequest(response.Handle, 6301, 0, 1, 1, 1, 4, 0, 0, 0);
                    var resultChartE = await ws.GetStatResultExAsync(requestChartE);
                    var charResultE = resultChartE.Result;
                    double Total = resultChartE.StatResult_dbl1;

                    GetGlobalInfoRequest requestChart88 = new GetGlobalInfoRequest(response.Handle, 1, 0, 3);
                    var resultChart88 = await ws.GetGlobalInfoAsync(requestChart88);
                    result = resultChart88.Result;
                    var char_count1 = resultChart88.ret;

                    GetPartInfoRequest requestChart571 = new GetPartInfoRequest(response.Handle, 1002, 1, 0);
                    var resultChart571 = await ws.GetPartInfoAsync(requestChart571);
                    var part_desc = resultChart571.KFieldValue;

                    Console.WriteLine("part desc...  " + part_desc);

                    GetPartInfoRequest requestChart572 = new GetPartInfoRequest(response.Handle, 1033, 1, 0);
                    var resultChart572 = await ws.GetPartInfoAsync(requestChart572);
                    var target_value = resultChart572.KFieldValue;

                    Console.WriteLine("Target value...  " + target_value);

                    GetPartInfoRequest requestChart573 = new GetPartInfoRequest(response.Handle, 1031, 1, 0);
                    var resultChart573 = await ws.GetPartInfoAsync(requestChart573);
                    var base_value = resultChart573.KFieldValue;
                    Console.WriteLine("base value...  " + base_value);

                    double dividedValue = 0;
                    double actual_dividedValue = 0;
                    string base_value1 = "";

                    if (day1 != 0)
                    {
                        if (base_value == "0")
                        {
                            actual_dividedValue = 0; // no target value
                            Console.WriteLine("actual_dividedValue base 0 : " + actual_dividedValue);
                            base_value1 = "N/A";
                        }
                        else
                        {
                            if (base_value == "1")
                            {
                                dividedValue = day1 / 1; // one day
                                actual_dividedValue = Total / dividedValue;
                                if (double.IsInfinity(actual_dividedValue) || double.IsNaN(dividedValue) || dividedValue == 0)
                                {
                                    actual_dividedValue = 0;
                                }

                                base_value1 = "D";
                            }
                            else if (base_value == "2")
                            {
                                dividedValue = day1 / 7; // week day
                                actual_dividedValue = Total / dividedValue;
                                if (double.IsInfinity(actual_dividedValue) || double.IsNaN(dividedValue) || dividedValue == 0)
                                {
                                    actual_dividedValue = 0; 
                                }
                                base_value1 = "W";
                            }
                            else if (base_value == "3")
                            {
                                dividedValue = day1 / 30;// month                                 
                                actual_dividedValue = Total / dividedValue;
                                if (double.IsInfinity(actual_dividedValue) || double.IsNaN(dividedValue) || dividedValue == 0)
                                {
                                    actual_dividedValue = 0;
                                }
                                base_value1 = "M";
                            }
                            else if (base_value == "4")
                            {
                                dividedValue = day1 / 15;// quarterly                                 
                                actual_dividedValue = Total / dividedValue;
                                if (double.IsInfinity(actual_dividedValue) || double.IsNaN(dividedValue) || dividedValue == 0)
                                {
                                    Console.WriteLine("Error: Division result is infinite");
                                    actual_dividedValue = 0;
                                }
                                base_value1 = "Q";
                            }
                            else if (base_value == "5")
                            {
                                dividedValue = day1 / 365;// year                                 
                                actual_dividedValue = Total / dividedValue;
                                if (double.IsInfinity(actual_dividedValue) || double.IsNaN(dividedValue) || dividedValue == 0)
                                {
                                    Console.WriteLine("Error: Division result is infinite");
                                    actual_dividedValue = 0;
                                }
                                base_value1 = "Y";
                            }
                            if (dividedValue != 0 && !double.IsInfinity(actual_dividedValue))
                            {
                                actual_dividedValue = Math.Round(actual_dividedValue);
                            }
                            else
                            {
                                Console.WriteLine("Error: In Division");
                                FileWriter.WriteToFile("Error: In Division");
                            }
                        }
                    }

                    if (partCount > 0)
                    {
                        inspection_target obj = new inspection_target
                        {
                            area = area,
                            component = Component,
                            model = Model,
                            operations = Operation,
                            actual_value = actual_dividedValue,
                            base_value = base_value1,
                            targeted_value = target_value,
                            part_desc = part_desc,
                            plant = plantname,
                            partnr = i,
                            from_date = formattedLastMonday,
                            to_date = formattedLastSunday
                        };
                        test.Add(obj);
                    }
                }
                Console.WriteLine(plant + " plant Inspection page i loop end partCount: " + i);
                FileWriter.WriteToFile(plant + " plant Inspection page i loop end partCount: " + i);
            }
            return Tuple.Create(test);
        }
        //catch (Exception ex)
        //{
        //    FileWriter.WriteToFile(ex.ToString());
        //    Console.WriteLine(ex.ToString());
        //}

    }
}
