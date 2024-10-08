using App.Configurations;
using dotnetWebService.BackendServices;
using dotnetWebService.Model;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static dotnetWebService.Model.Cp_ID;

namespace dotnetWebService.RouteBindings
{
    public class TestQuadrant
    {
        public async static Task<Tuple<List<TrendCount>, List<Characteristicsqudrant>, List<CVBUTrendCount>>> GetQudrantDetails(WebServiceManager webx, string plant)
        {
            List<TrendCount> trend_count = new List<TrendCount>();
            List<Characteristicsqudrant> test1 = new List<Characteristicsqudrant>();
            List<Characteristicsqudrant> groupedCharDemoPlant = new List<Characteristicsqudrant>();
            List<CVBUTrendCount> CVBU_quadrat = new List<CVBUTrendCount>();
            List<CVBUTrendCount> groupedCharDemo = new List<CVBUTrendCount>();
            List<Cplan> Cp_obj = new List<Cplan>();
            runTimeConfiguration config = new runTimeConfiguration();
            string Pune_plant = config.getParticularConfig("Pune_plant", "Plant");
            string PantN_plant = config.getParticularConfig("PantN_plant", "Plant");
            string Jmr_plant = config.getParticularConfig("Jamshedpur_plant", "Plant");
            //try
            //{
            double CVBUQuad1 = 0;
            double CVBUQuad2 = 0;
            double CVBUQuad3 = 0;
            double CVBUQuad4 = 0;
            double CVBUQuadTotal = 0;

            var ws = webx.ws;
            var response = webx.response;
            if (response == null)
            {
                FileWriter.WriteToFile(plant + "Plant response in Null ");
                return Tuple.Create(trend_count, test1, groupedCharDemo);
            }
            if (response.Handle == 0)
            {
                FileWriter.WriteToFile(plant + "Plant handle is zero " + response.Handle);
                Console.WriteLine(plant + "Plant handle is zero " + response.Handle);
                return Tuple.Create(trend_count, test1, groupedCharDemo);
            }

            string PlantName = "";
            if (response.Handle <= 0)
            {
                return Tuple.Create(trend_count, test1, groupedCharDemo);
            }
            Console.WriteLine(ws.Endpoint.Name);
            Console.WriteLine(response.Handle);


            string partID1 = "";
            string charID1 = "";
            string lastNValues = "30";
            int handle = response.Handle;

            double PlantQuad1 = 0;
            double PlantQuad2 = 0;
            double PlantQuad3 = 0;
            double PlantQuad4 = 0;
            double PlantQuadTotal = 0;
            List<Cplan> Cp_Id_Obj = new List<Cplan>();
            int months = 0;
            string month = "";
            string Cplan = "";
            string formattedEndDate = "";

            for (int j = 0; j < 6; j++)
            {
                double MonthQuad1 = 0;
                double MonthQuad2 = 0;
                double MonthQuad3 = 0;
                double MonthQuad4 = 0;
                double MonthQuadTotal = 0;

                months = j + 1;

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

                    DateTime currentDate = DateTime.Now;

                    // Current month end date
                    DateTime currentMonthEndDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));

                    DateTime lastMonthEndDate = currentDate.AddMonths(-j);
                    lastMonthEndDate = new DateTime(lastMonthEndDate.Year, lastMonthEndDate.Month, DateTime.DaysInMonth(lastMonthEndDate.Year, lastMonthEndDate.Month));
                    string monthName = lastMonthEndDate.ToString("MMM", CultureInfo.InvariantCulture);
                    string formattedStartDate = lastMonthEndDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    formattedEndDate = lastMonthEndDate.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    month = monthName;
                    Console.WriteLine($"Last {j} Month End Date: " + formattedEndDate);

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

                    CreateFilterRequest requestChart312 = new CreateFilterRequest(response.Handle, 1, 0004, formattedEndDate, 3);
                    var resultChart312 = await ws.CreateFilterAsync(requestChart312);
                    var filterHandleforformdate = resultChart312.FilterHandle;
                    result = resultChart312.Result;

                    var resultChart412 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, filterHandleforformdate, 2, 0, 0);
                    result = resultChart412.Result;

                    CreateFilterRequest request4 = new CreateFilterRequest(response.Handle, 1, 2005, "0", 2);
                    var resultChar4 = await ws.CreateFilterAsync(request4);
                    var Class_Handle = resultChar4.FilterHandle;
                    result = resultChar4.Result;

                    AddFilterToQueryRequest request5 = new AddFilterToQueryRequest();
                    var resultChar5 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, Class_Handle, 1, 0, 0);
                    result = resultChar5.Result;

                    CreateFilterRequest requestChart31 = new CreateFilterRequest(response.Handle, 1, 0, lastNValues, 129);
                    var resultChart31 = await ws.CreateFilterAsync(requestChart31);
                    var Filter_Handle = resultChart31.FilterHandle;

                    var resultChart41 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, Filter_Handle, 2, 0, 0);
                    result = resultChart41.Result;

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
                            FileWriter.WriteToFile(plant + "Plant Quadrant page ExecuteQueryRequest is null");
                            Console.WriteLine(plant + "Plant Quadrant page ExecuteQueryRequest is null");
                            //return Tuple.Create(test);
                        }
                    }
                    catch (Exception ex)
                    {
                        FileWriter.WriteToFile(plant + "Plant Quadrant page ExecuteQueryRequest Exception : " + ex.Message);
                        Console.WriteLine(plant + "Plant Quadrant page ExecuteQueryRequest Exception : " + ex.Message);
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
                    //Console.WriteLine("PART_COUNT......" + partCount);

                    GetPartInfoRequest requestChart81 = new GetPartInfoRequest(response.Handle, 1100, 1, 0);
                    var resultChart81 = await ws.GetPartInfoAsync(requestChart81);
                    string plants = resultChart81.KFieldValue;
                    PlantName = plants;

                    int i;
                    Console.WriteLine(plant + " plant Quadrant page i loop start partCount: " + partCount);
                    FileWriter.WriteToFile(plant + " plant Quadrant page i loop start partCount: " + partCount);
                    for (i = 1; i <= partCount; i++)
                    {
                        Console.WriteLine(plant + " plant Quadrant page i loop Running part: " + i);
                        int part_no = i + 1;

                        GetPartInfoRequest requestChart89 = new GetPartInfoRequest(response.Handle, 1000, 1, 0);
                        var resultChart89 = await ws.GetPartInfoAsync(requestChart89);
                        Cplan = resultChart89.KFieldValue;

                        GetStatResultExRequest requestChartE12 = new GetStatResultExRequest(response.Handle, 20035, 22, 1, 0, 1, 4, 0, 0, 0);
                        var resultChartE12 = await ws.GetStatResultExAsync(requestChartE12);
                        var charResultE12 = resultChartE12.Result;
                        var quadrant1 = resultChartE12.StatResult_dbl1; //green

                        GetStatResultExRequest requestChartE13 = new GetStatResultExRequest(response.Handle, 20035, 21, 1, 0, 1, 4, 0, 0, 0);
                        var resultChartE13 = await ws.GetStatResultExAsync(requestChartE13);
                        var charResultE13 = resultChartE13.Result;
                        var quadrant2 = resultChartE13.StatResult_dbl1; //blue

                        GetStatResultExRequest requestChartE14 = new GetStatResultExRequest(response.Handle, 20035, 11, 1, 0, 1, 4, 0, 0, 0);
                        var resultChartE14 = await ws.GetStatResultExAsync(requestChartE14);
                        var charResultE14 = resultChartE14.Result;
                        var quadrant3 = resultChartE14.StatResult_dbl1; //red

                        GetStatResultExRequest requestChartE15 = new GetStatResultExRequest(response.Handle, 20035, 12, 1, 0, 1, 4, 0, 0, 0);
                        var resultChartE15 = await ws.GetStatResultExAsync(requestChartE15);
                        var charResultE15 = resultChartE15.Result;
                        var quadrant4 = resultChartE15.StatResult_dbl1;  // yellow

                        Console.WriteLine(Cplan);
                        Console.WriteLine(quadrant1);
                        Console.WriteLine(quadrant2);
                        Console.WriteLine(quadrant3);
                        Console.WriteLine(quadrant4);
                        Console.WriteLine("--------------------------------");
                        if (j == 0)
                        {
                            PlantQuad1 += quadrant1;
                            PlantQuad2 += quadrant2;
                            PlantQuad3 += quadrant3;
                            PlantQuad4 += quadrant4;
                            PlantQuadTotal += quadrant1 + quadrant2 + quadrant3 + quadrant4;
                        }

                        MonthQuad1 += quadrant1;
                        MonthQuad2 += quadrant2;
                        MonthQuad3 += quadrant3;
                        MonthQuad4 += quadrant4;
                        MonthQuadTotal += quadrant1 + quadrant2 + quadrant3 + quadrant4;

                        if (j == 0)
                        {
                            CVBUQuad1 += quadrant1;
                            CVBUQuad2 += quadrant2;
                            CVBUQuad3 += quadrant3;
                            CVBUQuad4 += quadrant4;
                            CVBUQuadTotal += quadrant1 + quadrant2 + quadrant3 + quadrant4;
                        }
                    }
                    Console.WriteLine(plant + " plant Quadrant page i loop end partCount: " + i);
                    FileWriter.WriteToFile(plant + " plant Quadrant page i loop end partCount: " + i);
                }

                CVBUTrendCount obj2 = new CVBUTrendCount
                {
                    cplan = Cplan,
                    plant = PlantName,
                    PlantQuad1 = MonthQuad1,
                    PlantQuad2 = MonthQuad2,
                    PlantQuad3 = MonthQuad3,
                    PlantQuad4 = MonthQuad4,
                    PlantTotal = MonthQuadTotal,
                    Month = month,
                    Date = formattedEndDate
                };
                CVBU_quadrat.Add(obj2);

                TrendCount obj1 = new TrendCount
                {
                    cplan = Cplan,
                    plant = PlantName,
                    PlantQuad1 = MonthQuad1,
                    PlantQuad2 = MonthQuad2,
                    PlantQuad3 = MonthQuad3,
                    PlantQuad4 = MonthQuad4,
                    PlantTotal = MonthQuadTotal,
                    Month = month,
                    Date = formattedEndDate
                };

                trend_count.Add(obj1);
            }

            Characteristicsqudrant obj = new Characteristicsqudrant
            {
                Quad1 = CVBUQuad1,
                Quad2 = CVBUQuad2,
                Quad3 = CVBUQuad3,
                Quad4 = CVBUQuad4,
                Total = CVBUQuadTotal,
                plant = PlantName,
                Month = month

            };
            test1.Add(obj);

            groupedCharDemo = CVBU_quadrat
               .GroupBy(item => new { item.plant, item.Month })
               .Select(group => new CVBUTrendCount
               {

                   PlantQuad1 = group.Sum(item => item.PlantQuad1),
                   PlantQuad2 = group.Sum(item => item.PlantQuad2),
                   PlantQuad3 = group.Sum(item => item.PlantQuad3),
                   PlantQuad4 = group.Sum(item => item.PlantQuad4),
                   plant = group.Key.plant,
                   Month = group.Key.Month,
                   PlantTotal = group.Sum(item => item.PlantTotal),


               })
               .ToList();
            return Tuple.Create(trend_count, test1, groupedCharDemo);
        }
        //catch (Exception ex)
        //{
        //    FileWriter.WriteToFile(ex.ToString());
        //    Console.WriteLine(ex.ToString());
        //}
        // List<string> plantsList = new List<string>();

        // ws.ClientDisconnectAsync(handle);

        //return StatCharqudrant.CharLists;
    }
}

