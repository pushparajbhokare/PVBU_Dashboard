using App.Configurations;
using dotnetWebService.Model;
using ServiceReference1;
using System;
using System.Threading.Tasks;
using System.Xml;

namespace dotnetWebService.RouteBindings
{
    public class CharCharts
    {
        public async static Task<CharModel> GetCharCharts(string partList, string partID, string charID, string Plant)
        {
            CharModel charModel = new CharModel();
            try
            {
                WebConnectRequest? request = null;
                Qdas_Web_ServiceClient? ws = null;
                WebConnectResponse? response = null;
                string lastNValues = "125";
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
                    return charModel;
                }
                if (partID == "" || charID == "" || Plant == "")
                {
                    FileWriter.WriteToFile(Plant + "PartId is: " + partID + " Plant charId is: " + charID);
                }
                if (response.Handle == 0)
                {
                    FileWriter.WriteToFile(Plant + "Plant handle is zero " + response.Handle);
                    Console.WriteLine(Plant + "Plant handle is zero " + response.Handle);
                }

                if (response.Handle <= 0)
                {
                    return charModel;
                }

                Console.WriteLine(ws.Endpoint.Name);
                Console.WriteLine(response.Handle);

                int handle = response.Handle;
                string partListStr = "<Part key = '" + partID + "'><Char key='" + charID + "'/></Part>";
                CreateQueryRequest requestChart1 = new CreateQueryRequest(response.Handle);
                var graphicQR = await ws.CreateQueryAsync(requestChart1);
                int queryHandle = graphicQR.QueryHandle;
                int result = graphicQR.Result;

                CreateFilterRequest requestChart2 = new CreateFilterRequest(response.Handle, 1, 0, lastNValues, 129);
                var resultChart2 = await ws.CreateFilterAsync(requestChart2);
                int filterHandle = resultChart2.FilterHandle;
                result = resultChart2.Result;

                if (resultChart2.Result == 0)
                {
                    var resultChart3 = await ws.AddFilterToQueryAsync(response.Handle, graphicQR.QueryHandle, resultChart2.FilterHandle, 2, 0, 0);
                    result = resultChart3.Result;

                    if (resultChart3.Result == 0)
                    {
                        String partListXML = "<PartCharList>" + partListStr + "</PartCharList>";

                        var resultChart4 = await ws.ExecuteQuery_ExtAsync(response.Handle, graphicQR.QueryHandle, partListXML, false, true);
                        if (resultChart4 != null)
                        {
                            result = resultChart4.Result;
                        }
                        else
                        {
                            return charModel;
                        }

                        FreeQueryRequest freeQueryRequest1 = new FreeQueryRequest();
                        var resultChart51 = await ws.FreeQueryAsync(response.Handle, queryHandle);
                        result = resultChart51.Result;
                        var resultChart5 = await ws.EvaluateAllCharsAsync(response.Handle);
                        result = resultChart5.Result;

                        int partCount = -1;
                        int charCount = -1;

                        GetGlobalInfoRequest requestChart6 = new GetGlobalInfoRequest(response.Handle, 0, 0, 1);
                        var resultChart6 = await ws.GetGlobalInfoAsync(requestChart6);
                        result = resultChart6.Result;
                        partCount = resultChart6.ret;       //???

                        string StatResult_str1 = "";

                        if (partCount == 1)
                        {
                            GetGlobalInfoRequest requestChart7 = new GetGlobalInfoRequest(response.Handle, 0, 0, 1);
                            var resultChart7 = await ws.GetGlobalInfoAsync(requestChart7);
                            result = resultChart7.Result;
                            charCount = resultChart7.ret;       //???

                            GetGlobalInfoRequest requestChart8 = new GetGlobalInfoRequest(response.Handle, 1, 0, 2);
                            var resultChart8 = await ws.GetGlobalInfoAsync(requestChart8);
                            result = resultChart8.Result;
                            charCount = resultChart8.ret;       //???

                            GetPartInfoRequest requestChart9 = new GetPartInfoRequest(response.Handle, 1001, 1, 0);
                            var resultChart9 = await ws.GetPartInfoAsync(requestChart9);
                            result = resultChart9.Result;
                            string partNr = resultChart9.KFieldValue;       //???

                            GetPartInfoRequest requestChart10 = new GetPartInfoRequest(response.Handle, 1002, 1, 0);
                            var resultChart10 = await ws.GetPartInfoAsync(requestChart10);
                            result = resultChart10.Result;
                            string partDesc = resultChart10.KFieldValue;       //???

                            GetPartInfoRequest requestChart11 = new GetPartInfoRequest(response.Handle, 1005, 1, 0);
                            var resultChart11 = await ws.GetPartInfoAsync(requestChart11);
                            result = resultChart11.Result;
                            string productType = resultChart11.KFieldValue;       //???

                            GetPartInfoRequest requestChart12 = new GetPartInfoRequest(response.Handle, 1008, 1, 0);
                            var resultChart12 = await ws.GetPartInfoAsync(requestChart12);
                            result = resultChart12.Result;
                            string modelType = resultChart12.KFieldValue;       //???

                            GetPartInfoRequest requestChart13 = new GetPartInfoRequest(response.Handle, 1086, 1, 0);
                            var resultChart13 = await ws.GetPartInfoAsync(requestChart13);
                            result = resultChart13.Result;
                            string partOp = resultChart13.KFieldValue;       //???

                            if (charCount == 1)
                            {
                                GetCharInfoRequest requestChart14 = new GetCharInfoRequest(response.Handle, 2005, 1, 1);
                                var resultChart14 = await ws.GetCharInfoAsync(requestChart14);
                                double charResult = resultChart14.Result;
                                string charInfo = resultChart14.KFieldValue;       //???
                                string classStr = charResult == 0 ? charInfo : "ERROR";

                                GetStatResultRequest requestChart15 = new GetStatResultRequest(response.Handle, 20030, 2, 1, 0);
                                var resultChart15 = await ws.GetStatResultAsync(requestChart15);
                                charResult = resultChart15.Result;
                                charInfo = resultChart15.StatResult_str;
                                double charDbl = resultChart15.StatResult_dbl;
                                string riskStr = charResult == 0 ? charInfo : "ERROR";


                                GetCharInfoRequest requestChart16 = new GetCharInfoRequest(response.Handle, 2001, 1, 1);
                                var resultChart16 = await ws.GetCharInfoAsync(requestChart16);
                                charResult = resultChart16.Result;
                                charInfo = resultChart16.KFieldValue;
                                string charNr = charResult == 0 ? charInfo : "ERROR";

                                GetCharInfoRequest requestChart17 = new GetCharInfoRequest(response.Handle, 2002, 1, 1);
                                var resultChart17 = await ws.GetCharInfoAsync(requestChart17);
                                charResult = resultChart16.Result;
                                charInfo = resultChart16.KFieldValue;
                                string charDesc = charResult == 0 ? charInfo : "ERROR";

                                GetStatResultRequest requestChart18 = new GetStatResultRequest(response.Handle, 1000, 1, 1, 0);
                                var resultChart18 = await ws.GetStatResultAsync(requestChart18);
                                charResult = resultChart18.Result;
                                charInfo = resultChart18.StatResult_str;
                                charDbl = resultChart18.StatResult_dbl;
                                string xBar = charResult == 0 ? charInfo : "ERROR";

                                GetStatResultRequest requestChart19 = new GetStatResultRequest(response.Handle, 2100, 1, 1, 0);
                                var resultChart19 = await ws.GetStatResultAsync(requestChart19);
                                charResult = resultChart19.Result;
                                charInfo = resultChart19.StatResult_str;
                                charDbl = resultChart19.StatResult_dbl;
                                string stdDev = charResult == 0 ? charInfo : "ERROR";

                                GetStatResultRequest requestChart20 = new GetStatResultRequest(response.Handle, 5210, 1, 1, 0);
                                var resultChart20 = await ws.GetStatResultAsync(requestChart20);
                                charResult = resultChart20.Result;
                                charInfo = resultChart20.StatResult_str;
                                charDbl = resultChart20.StatResult_dbl;
                                string potIndex = charResult == 0 ? charInfo : "ERROR";

                                GetStatResultRequest requestChart21 = new GetStatResultRequest(response.Handle, 5220, 1, 1, 0);
                                var resultChart21 = await ws.GetStatResultAsync(requestChart21);
                                charResult = resultChart21.Result;
                                charInfo = resultChart21.StatResult_str;
                                charDbl = resultChart21.StatResult_dbl;
                                string criticalIndex = charResult == 0 ? charInfo : "ERROR";

                                GetGraphicRequest requestChart22 = new GetGraphicRequest(response.Handle, 3100, 1, 1, 500, 300);
                                try
                                {
                                    var resultChart22 = await ws.GetGraphicAsync(requestChart22);
                                    charResult = resultChart22.Result;
                                    string valGraphicStr = resultChart22.GraphicStr;
                                    Console.WriteLine("charResult....." + charResult);
                                    if (charResult == 0)
                                    {
                                        var xmlDocument = new XmlDocument();
                                        xmlDocument.LoadXml(valGraphicStr);
                                        charModel.valueChartImg = xmlDocument.SelectSingleNode("/Test/Image").InnerText;
                                    }
                                }
                                catch (Exception E)
                                {
                                    Console.WriteLine("exce....." + E.Message);
                                }

                                GetGraphicRequest requestChart23 = new GetGraphicRequest(response.Handle, 6110, 1, 1, 500, 300);
                                var resultChart23 = await ws.GetGraphicAsync(requestChart23);
                                charResult = resultChart23.Result;
                                string qccGraphicStr = resultChart23.GraphicStr;

                                if (charResult == 0)
                                {
                                    var xmlDocument = new XmlDocument();
                                    xmlDocument.LoadXml(qccGraphicStr);
                                    charModel.qccChartImg = xmlDocument.SelectSingleNode("/Test/Image").InnerText;
                                }

                                GetGraphicRequest requestChart24 = new GetGraphicRequest(response.Handle, 3300, 1, 1, 250, 300);
                                var resultChart24 = await ws.GetGraphicAsync(requestChart24);
                                charResult = resultChart24.Result;
                                string histGraphicStr = resultChart24.GraphicStr;

                                if (charResult == 0)
                                {
                                    var xmlDocument = new XmlDocument();
                                    xmlDocument.LoadXml(histGraphicStr);
                                    charModel.histChartImg = xmlDocument.SelectSingleNode("/Test/Image").InnerText;

                                }

                                charModel.modelType = modelType;
                                charModel.productNr = partNr;
                                charModel.productType = productType;
                                charModel.description = partDesc;
                                charModel.partList = partList;
                                charModel.OpNo = partOp;
                                charModel.partID = partID;
                                charModel.potIndex = potIndex;
                                charModel.criticalIndex = criticalIndex;
                                charModel.xBar = xBar;
                                charModel.stdDev = stdDev;
                                charModel.riskLevel = riskStr;
                                charModel.CharClass = classStr;
                                charModel.CharDesc = charDesc;
                                charModel.charNr = charNr;
                            }
                        }
                    }
                }
                ws.ClientDisconnectAsync(handle);
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
            return charModel;
        }
    }
}
