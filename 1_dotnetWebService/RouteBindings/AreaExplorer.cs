using App.Configurations;
using dotnetWebService.BackendServices;
using dotnetWebService.Model;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static dotnetWebService.Model.Cp_ID;

namespace dotnetWebService.RouteBindings
{
    public class AreaExplorer
    {
        public async static Task<Tuple<List<AllAreaExplorer>>> GetAreaExplorer(WebServiceManager webx, string plant)
        {
            List<AllAreaExplorer> test2 = new List<AllAreaExplorer>();
            List<Cplan> Cp_obj = new List<Cplan>();
            runTimeConfiguration config = new runTimeConfiguration();
            string Pune_plant = config.getParticularConfig("Pune_plant", "Plant");
            string PantN_plant = config.getParticularConfig("PantN_plant", "Plant");
            string Jmr_plant = config.getParticularConfig("Jamshedpur_plant", "Plant");
            //try
            //{
            Console.WriteLine("GetArea started..");
            var ws = webx.ws;
            var response = webx.response;
            if (response == null)
            {
                FileWriter.WriteToFile(plant + "Plant response in Null ");
                return Tuple.Create(test2);
            }
            if (response.Handle == 0)
            {
                FileWriter.WriteToFile(plant + "Plant handle is zero " + response.Handle);
                Console.WriteLine(plant + "Plant handle is zero " + response.Handle);
                return Tuple.Create(test2);
            }

            if (response.Handle <= 0)
            {
                return Tuple.Create(test2);
            }
            Console.WriteLine(ws.Endpoint.Name);
            Console.WriteLine(response.Handle);

            string partID1 = "";
            string charID1 = "";
            int handle = response.Handle;

            List<Cplan> Cp_Id_Obj = new List<Cplan>();

            DateTime today = DateTime.Today.AddDays(-1);

            int Days = 0;
            string day = "";
            for (int i = 0; i < 7; i++)
            {


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

                    DateTime startOfDay = today.AddDays(-i);
                    DateTime endOfDay = startOfDay.AddDays(1).AddSeconds(-1);

                    string formattedStartDate = startOfDay.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string formattedEndDate = endOfDay.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    Console.WriteLine($"Day {i + 1}: {formattedStartDate} to {formattedEndDate}");

                    Days = i + 1;
                    day = Days + "D";
                    Console.WriteLine(day);

                    string partListStr = "<Part key = '" + partID1 + "'><Char key='" + charID1 + "'/></Part>";
                    CreateQueryRequest requestChart1 = new CreateQueryRequest(response.Handle);
                    var graphicQR = await ws.CreateQueryAsync(requestChart1);
                    int queryHandle = graphicQR.QueryHandle;
                    int result = graphicQR.Result;

                    CreateFilterRequest requestChart2 = new CreateFilterRequest(response.Handle, 1, 1100, plant, 0);
                    var resultChart2 = await ws.CreateFilterAsync(requestChart2);
                    var filterHandleforPLANT = resultChart2.FilterHandle;
                    result = resultChart2.Result;

                    AddFilterToQueryRequest requestChart3 = new AddFilterToQueryRequest();
                    var resultChart3 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, filterHandleforPLANT, 0, 0, 0);
                    result = resultChart3.Result;

                    CreateFilterRequest requestChart212 = new CreateFilterRequest(response.Handle, 1, 1000, currentCpId, 0);
                    var resultChart212 = await ws.CreateFilterAsync(requestChart212);
                    var cplan_filter = resultChart212.FilterHandle;
                    result = resultChart212.Result;

                    AddFilterToQueryRequest requestChar312 = new AddFilterToQueryRequest();
                    var resultChar312 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, cplan_filter, 0, 0, 0);
                    result = resultChar312.Result;

                    CreateFilterRequest requestChart31 = new CreateFilterRequest(response.Handle, 1, 0004, formattedStartDate, 2);
                    var resultChart31 = await ws.CreateFilterAsync(requestChart31);
                    var filterHandleforformdate = resultChart31.FilterHandle;
                    result = resultChart31.Result;

                    var resultChart41 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, filterHandleforformdate, 2, 0, 0);
                    result = resultChart41.Result;

                    CreateFilterRequest requestChart311 = new CreateFilterRequest(response.Handle, 1, 0004, formattedEndDate, 3);
                    var resultChart311 = await ws.CreateFilterAsync(requestChart311);
                    var filterHandlefortodate = resultChart311.FilterHandle;
                    result = resultChart311.Result;

                    var resultChart411 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, filterHandlefortodate, 2, 0, 0);
                    result = resultChart411.Result;

                    CreateFilterRequest request4 = new CreateFilterRequest(response.Handle, 1, 2005, "0", 2);
                    var resultChar4 = await ws.CreateFilterAsync(request4);
                    var Class_Handle = resultChar4.FilterHandle;
                    result = resultChar4.Result;

                    AddFilterToQueryRequest request5 = new AddFilterToQueryRequest();
                    var resultChar5 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, Class_Handle, 1, 0, 0);
                    result = resultChar5.Result;

                    ExecuteQueryRequest requestChart4 = new ExecuteQueryRequest();
                    var resultChart4 = await ws.ExecuteQueryAsync(response.Handle, queryHandle, partListStr);
                    try
                    {
                        if (resultChart4 != null)
                        {
                            result = resultChart4.Result;
                        }
                        else
                        {
                            FileWriter.WriteToFile(plant + "Plant Area page ExecuteQueryRequest is null");
                            Console.WriteLine(plant + "Plant Area page ExecuteQueryRequest is null");
                        }
                    }
                    catch (Exception ex)
                    {
                        FileWriter.WriteToFile(plant + "Plant Area page ExecuteQueryRequest Exception : " + ex.Message);
                        Console.WriteLine(plant + "Plant Area page ExecuteQueryRequest Exception : " + ex.Message);
                    }

                    FreeQueryRequest freeQueryRequest = new FreeQueryRequest();
                    var resultChart71 = await ws.FreeQueryAsync(response.Handle, queryHandle);
                    var abc = resultChart71.Result;

                    CreateFilterRequest requestChart5 = new CreateFilterRequest();
                    var resultChart5 = await ws.EvaluateAllCharsAsync(response.Handle);
                    var result2 = resultChart5.Result;


                    GetGlobalInfoRequest requestChart6 = new GetGlobalInfoRequest(response.Handle, 1, 0, 1);
                    var resultChart6 = await ws.GetGlobalInfoAsync(requestChart6);
                    result = resultChart6.Result;
                    var partCount = resultChart6.ret;
                    Console.WriteLine("PART_COUNT......" + partCount);


                    GetGlobalInfoRequest requestChart7 = new GetGlobalInfoRequest(response.Handle, partCount, 0, 2);
                    var resultChart7 = await ws.GetGlobalInfoAsync(requestChart7);
                    result = resultChart7.Result;
                    var char_count = resultChart7.ret;
                    int j;
                    Console.WriteLine(plant + " plant AreaExplorer page j loop start partCount: " + partCount);
                    FileWriter.WriteToFile(plant + " plant AreaExplorer page j loop start partCount: " + partCount);
                    for (j = 1; j <= partCount; j++)
                    {
                        Console.WriteLine(plant + " plant AreaExplorer page j loop Running part: " + j);

                        GetPartInfoRequest requestChart8 = new GetPartInfoRequest(response.Handle, 1000, 1, 0);
                        var resultChart8 = await ws.GetPartInfoAsync(requestChart8);
                        var cplan = resultChart8.KFieldValue;

                        GetPartInfoRequest requestChart9 = new GetPartInfoRequest(response.Handle, 1003, 1, 0);
                        var resultChart9 = await ws.GetPartInfoAsync(requestChart9);
                        var area = resultChart9.KFieldValue;


                        GetPartInfoRequest requestChart811 = new GetPartInfoRequest(response.Handle, 1005, 1, 0);
                        var resultChart811 = await ws.GetPartInfoAsync(requestChart811);
                        var component = resultChart811.KFieldValue;

                        GetPartInfoRequest requestChart57 = new GetPartInfoRequest(response.Handle, 1008, 1, 0);
                        var resultChart57 = await ws.GetPartInfoAsync(requestChart57);
                        var Model = resultChart57.KFieldValue;

                        GetPartInfoRequest requestChart81 = new GetPartInfoRequest(response.Handle, 1100, 1, 0);
                        var resultChart81 = await ws.GetPartInfoAsync(requestChart81);
                        var plants = resultChart81.KFieldValue;

                        GetPartInfoRequest requestChart10 = new GetPartInfoRequest(response.Handle, 1086, 1, 0);
                        var resultChart10 = await ws.GetPartInfoAsync(requestChart10);
                        var operation = resultChart10.KFieldValue;


                        GetStatResultExRequest requestChartE = new GetStatResultExRequest(response.Handle, 6301, 0, 1, 1, 1, 4, 0, 0, 0);
                        var resultChartE = await ws.GetStatResultExAsync(requestChartE);
                        var charResultE = resultChartE.Result;
                        double total = resultChartE.StatResult_dbl1;

                        GetStatResultExRequest requestChartF = new GetStatResultExRequest(response.Handle, 10008, 0, 1, 1, 1, 4, 0, 0, 0);
                        var resultChartF = await ws.GetStatResultExAsync(requestChartF);
                        var charResultF = resultChartF.Result;
                        double nok = resultChartF.StatResult_dbl1;

                        GetStatResultExRequest requestChartG = new GetStatResultExRequest(response.Handle, 10006, 0, 1, 1, 1, 4, 0, 0, 0);
                        var resultChartG = await ws.GetStatResultExAsync(requestChartG);
                        var charResultG = resultChartG.Result;
                        double ok = resultChartG.StatResult_dbl1;

                        if (partCount > 0)
                        {
                            AllAreaExplorer obj = new AllAreaExplorer
                            {
                                operation = operation,
                                cplan = cplan,
                                area = area,
                                component = component,
                                model = Model,
                                total = total,
                                ok = ok,
                                nok = nok,
                                plant = plants,
                                Day = day,
                                Date = formattedStartDate
                            };
                            test2.Add(obj);
                        }
                    }
                    Console.WriteLine(plant + " plant AreaExplorer page j loop end partCount: " + j);
                    FileWriter.WriteToFile(plant + " plant AreaExplorer page j loop end partCount: " + j);

                }
            }
            return Tuple.Create(test2);
        }
        //catch (Exception ex)
        //{

        //    FileWriter.WriteToFile(ex.ToString());
        //    Console.WriteLine(ex.ToString());
        //}
    }
}

