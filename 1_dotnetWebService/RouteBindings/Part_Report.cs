using App.Configurations;
using dotnetWebService.Model;
using ServiceReference1;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml;

namespace dotnetWebService.RouteBindings
{
    public class Part_Report
    {
        private static BasicHttpSecurityMode securityMode;
        public async static Task<part_report_model> GetPartReport(string c_plan, string operation, string serial_no, string Plant)
        {
            part_report_model part_report_model = new part_report_model();
            try
            {
                var binding = new BasicHttpBinding(securityMode);
                binding.MaxReceivedMessageSize = 1048576;
                WebConnectRequest? request= null;
                Qdas_Web_ServiceClient? ws = null;
                WebConnectResponse? response = null;
                CharModel charModel = new CharModel();

                runTimeConfiguration config = new runTimeConfiguration();
                string Pune_Plant_name = config.getParticularConfig("Pune_plant", "Plant");
                string Pune_plantUser = config.getParticularConfig("Pune_plant", "username");
                string Pune_plantPass = config.getParticularConfig("Pune_plant", "pass");

                string PantN_plant_name = config.getParticularConfig("PantN_plant", "Plant");
                string PantN_plantUser = config.getParticularConfig("PantN_plant", "username");
                string PantN_plantPass = config.getParticularConfig("PantN_plant", "pass");

                string Jmr_plant_name = config.getParticularConfig("Jamshedpur_plant", "Plant");
                string Jmr_plantUser = config.getParticularConfig("Jamshedpur_plant", "username");
                string Jmr_plantPass = config.getParticularConfig("Jamshedpur_plant", "pass");

                if (Plant == Pune_Plant_name)
                {
                    WebConnectRequest request_plant = new WebConnectRequest(20, 44, Pune_plantUser, Pune_plantPass, "");
                    Qdas_Web_ServiceClient ws_plant = new Qdas_Web_ServiceClient(Qdas_Web_ServiceClient.EndpointConfiguration.IQdas_Web_ServicePort);
                    WebConnectResponse response_plant = ws_plant.WebConnectAsync(request_plant).GetAwaiter().GetResult();
                    request = request_plant;
                    ws = ws_plant;
                    response = response_plant;
                }
                if (Plant == PantN_plant_name)
                {
                    WebConnectRequest request_pune = new WebConnectRequest(20, 44, PantN_plantUser, PantN_plantPass, "");
                    Qdas_Web_ServiceClient ws_pune = new Qdas_Web_ServiceClient(Qdas_Web_ServiceClient.EndpointConfiguration.IQdas_Web_Pantnagar_ServicePort);
                    WebConnectResponse response_pune = ws_pune.WebConnectAsync(request_pune).GetAwaiter().GetResult();
                    request = request_pune;
                    ws = ws_pune;
                    response = response_pune;
                }
                if (Plant == Jmr_plant_name)
                {
                    WebConnectRequest request_pune = new WebConnectRequest(20, 44, Jmr_plantUser, Jmr_plantPass, "");
                    Qdas_Web_ServiceClient ws_pune = new Qdas_Web_ServiceClient(Qdas_Web_ServiceClient.EndpointConfiguration.IQdas_Web_Jamshedpur_ServicePort);
                    WebConnectResponse response_pune = ws_pune.WebConnectAsync(request_pune).GetAwaiter().GetResult();
                    request = request_pune;
                    ws = ws_pune;
                    response = response_pune;
                }

                if (response == null)
                {
                    FileWriter.WriteToFile(Plant + "Plant response in Null ");
                    return part_report_model;
                }

                if (c_plan == "" || operation == "" || Plant == "" || serial_no == "")
                {
                    FileWriter.WriteToFile(Plant + "PartId is: " + c_plan + " Plant operation is: " + operation + " serial_no is: " + serial_no);
                }
                if (response.Handle == 0)
                {
                    FileWriter.WriteToFile(Plant + "Plant handle is zero " + response.Handle);
                    Console.WriteLine(Plant + "Plant handle is zero " + response.Handle);
                }
                if (response.Handle <= 0 && serial_no != "")
                {
                    return part_report_model;
                }
                ws.Endpoint.Binding = binding;
                Console.WriteLine(ws.Endpoint.Name);
                Console.WriteLine(response.Handle);

                string partID1 = "";
                string charID1 = "";
                int handle = response.Handle;

                string partListStr = "<Part key = '" + partID1 + "'><Char key='" + charID1 + "'/></Part>";
                CreateQueryRequest requestChart1 = new CreateQueryRequest(response.Handle);
                var graphicQR = await ws.CreateQueryAsync(requestChart1);
                int queryHandle = graphicQR.QueryHandle;
                int result = graphicQR.Result;

                CreateFilterRequest requestChart2 = new CreateFilterRequest(response.Handle, 1, 1100, Plant, 0);
                var resultChart2 = await ws.CreateFilterAsync(requestChart2);
                var filterHandleforPLANT = resultChart2.FilterHandle;
                result = resultChart2.Result;

                AddFilterToQueryRequest requestChart3 = new AddFilterToQueryRequest();
                var resultChart3 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, filterHandleforPLANT, 0, 0, 0);
                result = resultChart3.Result;

                CreateFilterRequest requestChart212 = new CreateFilterRequest(response.Handle, 1, 1000, c_plan, 0);
                var resultChart212 = await ws.CreateFilterAsync(requestChart212);
                var cplan_filter = resultChart212.FilterHandle;
                result = resultChart212.Result;

                AddFilterToQueryRequest requestChart312 = new AddFilterToQueryRequest();
                var resultChart312 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, cplan_filter, 0, 0, 0);
                result = resultChart312.Result;

                CreateFilterRequest requestChart211 = new CreateFilterRequest(response.Handle, 1, 1086, operation, 0);
                var resultChart211 = await ws.CreateFilterAsync(requestChart211);
                var opearation_filter = resultChart211.FilterHandle;
                result = resultChart211.Result;

                AddFilterToQueryRequest requestChart311 = new AddFilterToQueryRequest();
                var resultChart311 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, opearation_filter, 0, 0, 0);
                result = resultChart311.Result;

                CreateFilterRequest requestChart21 = new CreateFilterRequest(response.Handle, 1, 0014, serial_no, 0);
                var resultChart21 = await ws.CreateFilterAsync(requestChart21);
                var partserial_number = resultChart21.FilterHandle;
                result = resultChart21.Result;

                AddFilterToQueryRequest requestChart31 = new AddFilterToQueryRequest();
                var resultChart31 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, partserial_number, 0, 0, 0);
                result = resultChart31.Result;
                ExecuteQueryRequest requestChart4 = new ExecuteQueryRequest();
                var resultChart4 = await ws.ExecuteQueryAsync(response.Handle, queryHandle, partListStr);
                if (resultChart4 != null)
                {
                    result = resultChart4.Result;
                }
                else
                {
                    return part_report_model;
                }

                FreeQueryRequest freeQueryRequest1 = new FreeQueryRequest();
                var resultChart51 = await ws.FreeQueryAsync(response.Handle, queryHandle);
                result = resultChart51.Result;

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
                var char_count1 = resultChart7.ret;

                for (int j = 1; j <= partCount; j++)
                {

                    GetGlobalInfoRequest requestChart71 = new GetGlobalInfoRequest(response.Handle, j, 0, 2);
                    var resultChart71 = await ws.GetGlobalInfoAsync(requestChart71);
                    result = resultChart71.Result;
                    var char_count11 = resultChart7.ret;

                    Console.WriteLine("CHAR_COUNT......" + char_count11);
                    GetPartInfoRequest requestchart8 = new GetPartInfoRequest(response.Handle, 1000, j, 0);
                    var resultchart8 = await ws.GetPartInfoAsync(requestchart8);
                    var cplan = resultchart8.KFieldValue;

                    GetPartInfoRequest requestChart9 = new GetPartInfoRequest(response.Handle, 1003, j, 0);
                    var resultChart9 = await ws.GetPartInfoAsync(requestChart9);
                    var Area = resultChart9.KFieldValue;

                    GetPartInfoRequest requestChart57 = new GetPartInfoRequest(response.Handle, 1008, j, 0);
                    var resultChart57 = await ws.GetPartInfoAsync(requestChart57);
                    var Model = resultChart57.KFieldValue;

                    GetPartInfoRequest requestChart91 = new GetPartInfoRequest(response.Handle, 1100, j, 0);
                    var resultChart91 = await ws.GetPartInfoAsync(requestChart91);
                    var plants = resultChart91.KFieldValue;

                    GetPartInfoRequest requestChart811 = new GetPartInfoRequest(response.Handle, 1005, j, 0);
                    var resultChart811 = await ws.GetPartInfoAsync(requestChart811);
                    var component = resultChart811.KFieldValue;

                    GetPartInfoRequest requestChart10 = new GetPartInfoRequest(response.Handle, 1086, j, 0);
                    var resultChart10 = await ws.GetPartInfoAsync(requestChart10);
                    var Operation = resultChart10.KFieldValue;

                    GetStatResultExRequest requestChartE = new GetStatResultExRequest(response.Handle, 6301, 0, j, 1, 1, 4, 0, 0, 0);
                    var resultChartE = await ws.GetStatResultExAsync(requestChartE);
                    var charResultE = resultChartE.Result;
                    double Total = resultChartE.StatResult_dbl1;

                    GetGraphicExtRequest requestChart2321 = new GetGraphicExtRequest(response.Handle, 7410, 3, j, 0, 1, 0, 1, 1, 1, 0, 0, 1, 600, 1800, 0, char_count11, 0, 1);
                    var resultChart2321 = await ws.GetGraphicExtAsync(requestChart2321);
                    var charResult1 = resultChart2321.Result;
                    string partGraphicStr1 = resultChart2321.GraphicStr;
                    if (charResult1 == 0)
                    {
                        var xmlDocument = new XmlDocument();
                        xmlDocument.LoadXml(partGraphicStr1);
                        part_report_model.part_report_img = xmlDocument.SelectSingleNode("/Test/Image").InnerText;
                    }
                }
                ws.ClientDisconnectAsync(handle);
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
            return part_report_model;
        }
    }
}
