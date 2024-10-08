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
    public class CharacteristicsExplorer
    {
        public async static Task<Tuple<List<Characteristic>, List<Diffrence_crit>, List<Diagram>>> GetCharDetailsPlantwise(WebServiceManager webx, string plant)
        {
            List<Diagram> wtrobj1 = new List<Diagram>();
            List<month_record> LastMonthRecords = new List<month_record>();
            List<month_record> CurrentMonthRecords = new List<month_record>();
            List<Characteristic> CharacteristicsList = new List<Characteristic>();
            List<Characteristic> AllCharacteristicsList = new List<Characteristic>();
            List<Diffrence_crit> Difference = new List<Diffrence_crit>();
            List<Diffrence_crit> Top5Difference = new List<Diffrence_crit>();


            runTimeConfiguration config = new runTimeConfiguration();
            string Pune_plant = config.getParticularConfig("Pune_plant", "Plant");
            string PantN_plant = config.getParticularConfig("PantN_plant", "Plant");
            string Jmr_plant = config.getParticularConfig("Jamshedpur_plant", "Plant");
            //try
            //{
            double Last_count = 0;
            double Decr_count = 0;
            double Inc_count = 0;
            double Cur_count = 0;
            Console.WriteLine("Get_char started..");
            var ws = webx.ws;
            var response = webx.response;
            if (response == null)
            {
                FileWriter.WriteToFile(plant + "Plant response in Null ");
                return Tuple.Create(CharacteristicsList, Top5Difference, wtrobj1);
            }
            if (response.Handle == 0)
            {
                FileWriter.WriteToFile(plant + "Plant handle is zero " + response.Handle);
                Console.WriteLine(plant + "Plant handle is zero " + response.Handle);
                return Tuple.Create(CharacteristicsList, Top5Difference, wtrobj1);
            }

            if (response.Handle <= 0)
            {
                return Tuple.Create(CharacteristicsList, Top5Difference, wtrobj1);
            }
            Console.WriteLine(ws.Endpoint.Name);
            Console.WriteLine(response.Handle);
            string partList1 = string.Empty;
            string partID1 = "";
            string charID1 = "";
            string lastNValues = "30";
            int handle = response.Handle;
            string lastMonth = "";
            string currentMonth = "";
            string Month = "";
            string PlantName = "";


            List<Cplan> Cp_Id_Obj = new List<Cplan>();
            DateTime currentDate = DateTime.Now.AddMonths(-1);

            DateTime currentMonthEndDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
            for (int k = 0; k < 2; k++)// 0 = last month, 1= current month
            {
                Console.WriteLine(plant + " plant " + 0 + " month loop start");

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

                    DateTime lastMonthEndDate = currentDate.AddMonths(k);
                    lastMonthEndDate = new DateTime(lastMonthEndDate.Year, lastMonthEndDate.Month, DateTime.DaysInMonth(lastMonthEndDate.Year, lastMonthEndDate.Month));
                    string monthName = lastMonthEndDate.ToString("MMM", CultureInfo.InvariantCulture);
                    Month = monthName;
                    if (k == 0)
                    {
                        lastMonth = monthName;

                    }
                    if (k == 1)
                    {
                        currentMonth = monthName;
                    }

                    string formattedStartDate = lastMonthEndDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string formattedEndDate = lastMonthEndDate.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    Console.WriteLine($"{plant} plantLast {k} Month End Date: " + formattedEndDate);
                    FileWriter.WriteToFile($"{plant} plant Last {k} Month End Date: " + formattedEndDate);

                    string partListStr = "<Part key = '" + partID1 + "'><Char key='" + charID1 + "'/></Part>";
                    CreateQueryRequest requestChart1 = new CreateQueryRequest(response.Handle);
                    var graphicQR = await ws.CreateQueryAsync(requestChart1);
                    int queryHandle = graphicQR.QueryHandle;
                    int result = graphicQR.Result;

                    CreateFilterRequest requestChart3 = new CreateFilterRequest(response.Handle, 1, 1100, plant, 0);
                    var resultChart3 = await ws.CreateFilterAsync(requestChart3);
                    var filterHandleforPlant = resultChart3.FilterHandle;
                    result = resultChart3.Result;

                    AddFilterToQueryRequest requestChart5 = new AddFilterToQueryRequest();
                    var resultChart5 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, filterHandleforPlant, 0, 0, 0);
                    result = resultChart5.Result;

                    CreateFilterRequest requestChart212 = new CreateFilterRequest(response.Handle, 1, 1000, currentCpId, 0);
                    var resultChart212 = await ws.CreateFilterAsync(requestChart212);
                    var cplan_filter = resultChart212.FilterHandle;
                    result = resultChart212.Result;

                    AddFilterToQueryRequest requestChar312 = new AddFilterToQueryRequest();
                    var resultChar312 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, cplan_filter, 0, 0, 0);
                    result = resultChar312.Result;

                    CreateFilterRequest requestChart312 = new CreateFilterRequest(response.Handle, 1, 0004, formattedEndDate, 5);
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
                            FileWriter.WriteToFile(plant + "Plant Characteristics page ExecuteQueryRequest is null");
                            Console.WriteLine(plant + "Plant Characteristics page ExecuteQueryRequest is null");
                        }
                    }
                    catch (Exception ex)
                    {
                        FileWriter.WriteToFile(plant + "Plant Characteristics page ExecuteQueryRequest Exception : " + ex.Message);
                        Console.WriteLine(plant + "Plant Characteristics page ExecuteQueryRequest Exception : " + ex.Message);
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

                    GetGlobalInfoRequest requestChart91 = new GetGlobalInfoRequest(response.Handle, 0, 0, 2);
                    var resultChart91 = await ws.GetGlobalInfoAsync(requestChart91);
                    result = resultChart91.Result;
                    var char_count1 = resultChart91.ret;

                    string xBar = "";
                    string stdDev = "";
                    string potIndex = "";
                    string criticalIndex = "";
                    string unit = "";
                    int i;

                    Console.WriteLine(plant + " plant Characteristics page i loop start partCount: " + partCount);
                    FileWriter.WriteToFile(plant + " plant Characteristics page i loop start partCount: " + partCount);
                    for (i = 1; i <= partCount; i++)
                    {
                        Console.WriteLine(plant + " plant Characteristics page i loop Running part: " + i);
                        GetPartInfoRequest requestChart82 = new GetPartInfoRequest(response.Handle, 1000, 1, 0);
                        var resultChart82 = await ws.GetPartInfoAsync(requestChart82);
                        var resrr = resultChart82.KFieldValue;

                        GetGlobalInfoRequest requestChart88 = new GetGlobalInfoRequest(response.Handle, 1, 0, 3);
                        var resultChart88 = await ws.GetGlobalInfoAsync(requestChart88);
                        result = resultChart88.Result;
                        var char_count = resultChart88.ret;

                        for (int j = 0; j < char_count; j++)
                        {
                            int charNo = j + 1;

                            GetCharInfoRequest requestChart75 = new GetCharInfoRequest(response.Handle, 2001, 1, charNo);
                            var resultchart75 = await ws.GetCharInfoAsync(requestChart75);
                            var res3 = resultchart75.KFieldValue;

                            GetCharInfoRequest requestChart751 = new GetCharInfoRequest(response.Handle, 2000, 1, charNo);
                            var resultchart751 = await ws.GetCharInfoAsync(requestChart751);
                            var CHAR_NO = resultchart751.KFieldValue;

                            GetPartInfoRequest requestChart823 = new GetPartInfoRequest(response.Handle, 1000, 1, 0);
                            var resultChart823 = await ws.GetPartInfoAsync(requestChart823);
                            var part_id = resultChart823.KFieldValue;

                            GetPartInfoRequest requestChart8231 = new GetPartInfoRequest(response.Handle, 1100, 1, 0);
                            var resultChart8231 = await ws.GetPartInfoAsync(requestChart8231);
                            var plantname = resultChart8231.KFieldValue;

                            PlantName = plantname;

                            GetCharInfoRequest requestChart758 = new GetCharInfoRequest(response.Handle, 2002, 1, charNo);
                            var resultchart758 = await ws.GetCharInfoAsync(requestChart758);
                            var char_desc = resultchart758.KFieldValue;

                            GetCharInfoRequest requestChart752 = new GetCharInfoRequest(response.Handle, 2005, 1, charNo);
                            var resultchart752 = await ws.GetCharInfoAsync(requestChart752);
                            var Mclass = resultchart752.KFieldValue;

                            GetStatResultExRequest requestChartE = new GetStatResultExRequest(response.Handle, 15100, 4, 1, charNo, 1, 4, 0, 0, 0);
                            var resultChartE = await ws.GetStatResultExAsync(requestChartE);
                            var charResultE = resultChartE.Result;
                            var status = resultChartE.OutputCount;

                            GetPartInfoRequest requestChart84 = new GetPartInfoRequest(response.Handle, 1002, 1, charNo);
                            var resultChart84 = await ws.GetPartInfoAsync(requestChart84);
                            var part_desc = resultChart84.KFieldValue;

                            GetPartInfoRequest requestChart8221 = new GetPartInfoRequest(response.Handle, 1003, 1, charNo);
                            var resultChart8221 = await ws.GetPartInfoAsync(requestChart8221);
                            var ENG = resultChart8221.KFieldValue;

                            GetPartInfoRequest requestChart83 = new GetPartInfoRequest(response.Handle, 1086, 1, charNo);
                            var resultChart83 = await ws.GetPartInfoAsync(requestChart83);
                            var OP = resultChart83.KFieldValue;

                            GetPartInfoRequest requestChart821 = new GetPartInfoRequest(response.Handle, 1005, 1, charNo);
                            var resultChart821 = await ws.GetPartInfoAsync(requestChart821);
                            var Component = resultChart821.KFieldValue;

                            GetPartInfoRequest requestChart822 = new GetPartInfoRequest(response.Handle, 1008, 1, charNo);
                            var resultChart822 = await ws.GetPartInfoAsync(requestChart822);
                            var Model = resultChart822.KFieldValue;

                            GetStatResultRequest requestChart18 = new GetStatResultRequest(response.Handle, 1000, 1, charNo, 0);
                            var resultChart18 = await ws.GetStatResultAsync(requestChart18);
                            var charResult = resultChart18.Result;
                            double resultValue = resultChart18.StatResult_dbl;
                            double roundedValue = Math.Round(resultValue, 2);
                            var charInfo = roundedValue.ToString("F2");
                            var charDbl = resultChart18.StatResult_dbl;
                            xBar = charResult == 0 ? charInfo : "N/A";

                            GetStatResultRequest requestChart181 = new GetStatResultRequest(response.Handle, 2142, i, charNo, 0);
                            var resultChart181 = await ws.GetStatResultAsync(requestChart181);
                            charResult = resultChart181.Result;
                            charInfo = resultChart181.StatResult_str;
                            //roundedValue = Math.Round(resultValue, 2);
                            //charInfo = roundedValues.ToString("F2");
                            charDbl = resultChart181.StatResult_dbl;
                            unit = charResult == 0 ? charInfo : "-";

                            string unitResult = $"{xBar} {unit}";

                            GetStatResultRequest requestChart19 = new GetStatResultRequest(response.Handle, 2100, 1, charNo, 0);
                            var resultChart19 = await ws.GetStatResultAsync(requestChart19);
                            charResult = resultChart19.Result;

                            resultValue = resultChart19.StatResult_dbl;
                            roundedValue = Math.Round(resultValue, 2);
                            charInfo = roundedValue.ToString("F2");
                            charDbl = resultChart19.StatResult_dbl;
                            stdDev = charResult == 0 ? charInfo : "N/A";

                            GetStatResultRequest requestChart22 = new GetStatResultRequest(response.Handle, 2300, 1, charNo, 0);
                            var resultChart22 = await ws.GetStatResultAsync(requestChart22);
                            charResult = resultChart22.Result;

                            resultValue = resultChart22.StatResult_dbl;
                            roundedValue = Math.Round(resultValue, 2);
                            charInfo = roundedValue.ToString("F2");
                            charDbl = resultChart22.StatResult_dbl;
                            var Range_1 = charResult == 0 ? charInfo : "N/A";

                            GetStatResultRequest requestChart20 = new GetStatResultRequest(response.Handle, 5210, 1, charNo, 0);
                            var resultChart20 = await ws.GetStatResultAsync(requestChart20);
                            charResult = resultChart20.Result;
                            resultValue = resultChart20.StatResult_dbl;
                            roundedValue = Math.Round(resultValue, 2);
                            charInfo = roundedValue.ToString("F2");
                            charDbl = resultChart20.StatResult_dbl;
                            potIndex = charResult == 0 ? charInfo : "N/A";

                            GetStatResultRequest requestChart21 = new GetStatResultRequest(response.Handle, 5220, 1, charNo, 0);
                            var resultChart21 = await ws.GetStatResultAsync(requestChart21);
                            charResult = resultChart21.Result;
                            resultValue = resultChart21.StatResult_dbl;
                            roundedValue = Math.Round(resultValue, 2);
                            charInfo = roundedValue.ToString("F2");
                            charDbl = resultChart21.StatResult_dbl;
                            criticalIndex = charResult == 0 ? charInfo : "N/A";

                            GetStatResultExRequest requestChartE1 = new GetStatResultExRequest(response.Handle, 20034, 0, 1, charNo, 1, 4, 0, 0, 0);
                            var resultChartE1 = await ws.GetStatResultExAsync(requestChartE1);
                            var charResultE1 = resultChartE1.Result;
                            var quadrant_value = resultChartE1.StatResult_str1;
                            string quadrant = "";

                            if (quadrant_value == "CC ok, Cpk ok")
                            {
                                quadrant = "Stable & Capable";
                                if (k == 0)
                                {
                                    Last_count++;
                                }
                                if (k == 1)
                                {
                                    Cur_count++;
                                }

                            }
                            else if (quadrant_value == "CC ok, Cpk nok")
                            {
                                quadrant = "Stable & Un-Capable";
                            }
                            else if (quadrant_value == "CC nok, Cpk ok")
                            {
                                quadrant = "Un-Stable & Capable";
                            }
                            else if (quadrant_value == "CC nok, Cpk nok")
                            {
                                quadrant = "Un-Stable & Un-Capable";
                            }

                            if (partCount > 0)
                            {
                                if (k == 0 && criticalIndex != "N/A")
                                {
                                    month_record lastmomth = new month_record
                                    {
                                        plant_name = plantname,
                                        parameter_desc = char_desc,
                                        Cplan = resrr,
                                        char_id = Convert.ToInt32(CHAR_NO),
                                        criticalIndex = criticalIndex,
                                        component = Component,
                                        model = Model,
                                        month = Month,
                                        Date = formattedEndDate

                                    };
                                    LastMonthRecords.Add(lastmomth);
                                }

                                if (k == 1 && criticalIndex != "N/A")
                                {
                                    month_record currentmonth = new month_record
                                    {
                                        plant_name = plantname,
                                        parameter_desc = char_desc,
                                        Cplan = resrr,
                                        char_id = Convert.ToInt32(CHAR_NO),
                                        criticalIndex = criticalIndex,
                                        component = Component,
                                        model = Model,
                                        month = Month,
                                        Date = formattedEndDate
                                    };
                                    CurrentMonthRecords.Add(currentmonth);
                                }

                                Characteristic obj = new Characteristic
                                {
                                    part_id = part_id,
                                    char_id = CHAR_NO,
                                    plant_name = plantname,
                                    parameter = res3,
                                    parameter_desc = char_desc,
                                    part_desc = part_desc,
                                    quadrant = quadrant,
                                    quadrant_value = quadrant_value,
                                    mclass = Mclass,
                                    status = status,
                                    operation = OP,
                                    area = ENG,
                                    component = Component,
                                    model = Model,
                                    xBar = unitResult,
                                    unit = unit,
                                    stdDev = stdDev,
                                    range = Range_1,
                                    potIndex = potIndex,
                                    criticalIndex = criticalIndex,

                                };

                                if (k == 1 && obj.operation != null && obj.area != null && obj.component != null && obj.model != null)
                                {
                                    if (Mclass == "1" || Mclass == "2")
                                    {
                                        obj.mclass = "CTQ-E";
                                        CharacteristicsList.Add(obj);
                                    }
                                    else if (Mclass == "3")
                                    {
                                        obj.mclass = "CTQ-M";
                                        CharacteristicsList.Add(obj);
                                    }
                                    else if (Mclass == "0")
                                    {
                                        obj.mclass = "Other";
                                        CharacteristicsList.Add(obj);
                                    }
                                    else if (Mclass == "4")
                                    {
                                        obj.mclass = "CTQ-C";
                                        CharacteristicsList.Add(obj);
                                    }
                                    AllCharacteristicsList = CharacteristicsList;
                                }
                            }
                        }

                    }
                    Console.WriteLine(plant + " plant Characteristics page i loop end partCount: " + i);
                    FileWriter.WriteToFile(plant + " plant Characteristics page i loop end partCount: " + i);
                }

            }
            var groupedRecords = LastMonthRecords
    .GroupBy(record => new
    {
        record.Cplan,
        record.char_id,
    });

            foreach (var group in groupedRecords)
            {
                var key = group.Key;
                var lastMonthRecords = group.ToList();
                var currentMonthRecords = CurrentMonthRecords
                    .Where(record =>
                        record.Cplan == key.Cplan &&
                        record.char_id == key.char_id
                        )
                    .ToList();

                foreach (var last_crit in lastMonthRecords)
                {
                    var current_critRecord = currentMonthRecords.FirstOrDefault(x =>
                        x.Cplan == last_crit.Cplan &&
                        x.char_id == key.char_id
                        );

                    double current_crit = current_critRecord == null ? 0 : Convert.ToDouble(current_critRecord.criticalIndex);

                    double diff = Convert.ToDouble(last_crit.criticalIndex) - current_crit;

                    Difference.Add(new Diffrence_crit
                    {
                        Cplan = key.Cplan,
                        char_id = key.char_id,
                        parameter_desc = last_crit.parameter_desc,
                        plant = last_crit.plant_name,
                        model = last_crit.model,
                        component = last_crit.component,
                        DiffrenceMonthcriticalIndex = diff,
                        LastMonthcriticalIndex = Convert.ToDouble(last_crit.criticalIndex),
                        CurrentMonthcriticalIndex = current_crit
                    });
                }
            }

            Top5Difference = Difference.OrderByDescending(record => record.DiffrenceMonthcriticalIndex).Take(5).ToList();

            if (Last_count > Cur_count)
            {
                Decr_count = Last_count - Cur_count;
            }
            if (Cur_count > Last_count)
            {
                Inc_count = Cur_count - Last_count;
            }

            Diagram Dia_obj = new Diagram
            {
                last_count = Last_count,
                cur_count = Cur_count,
                decr_count = Decr_count,
                inc_count = Inc_count,
                lastMonth = lastMonth,
                currentMonth = currentMonth,
                plant = PlantName,
            };
            wtrobj1.Add(Dia_obj);
            return Tuple.Create(CharacteristicsList, Top5Difference, wtrobj1);
        }
    }
}



