using App.Configurations;
using dotnetWebService.BackendServices;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static dotnetWebService.Model.Cp_ID;

namespace dotnetWebService.RouteBindings
{
    public class DwnStucture
    {
        public async static Task<List<Cplan>> DwnEmptStucture(WebServiceManager webx, string plant)
        {
            List<Cplan> Cp_obj = new List<Cplan>();
            runTimeConfiguration config = new runTimeConfiguration();
            string Pune_plant = config.getParticularConfig("Pune_plant", "Plant");
            string PantN_plant = config.getParticularConfig("PantN_plant", "Plant");
            string Jmr_plant = config.getParticularConfig("Jamshedpur_plant", "Plant");
            //try
            //{
            var ws = webx.ws;
            var response = webx.response;
            if (response == null)
            {
                FileWriter.WriteToFile(plant + "Plant response in Null ");
                return Cp_obj;
            }
            if (response.Handle == 0)
            {
                FileWriter.WriteToFile(plant + "Plant handle is zero " + response.Handle);
                Console.WriteLine(plant + "Plant handle is zero " + response.Handle);
            }

            if (response.Handle <= 0)
            {
                return Cp_obj;
            }
            Console.WriteLine(ws.Endpoint.Name);
            Console.WriteLine(response.Handle);
            string partID1 = "";
            string charID1 = "";
            string Emt_partListStr = "<Part key = '" + partID1 + "'><Char key='" + charID1 + "'/></Part>";
            CreateQueryRequest Emt_requestChart1 = new CreateQueryRequest(response.Handle);
            var Emt_graphicQR = await ws.CreateQueryAsync(Emt_requestChart1);
            int queryHandle = Emt_graphicQR.QueryHandle;
            int result = Emt_graphicQR.Result;

            CreateFilterRequest Emt_requestChart3 = new CreateFilterRequest(response.Handle, 1, 1100, plant, 0);
            var Emt_resultChart3 = await ws.CreateFilterAsync(Emt_requestChart3);
            var Emt_filterHandleforPlant = Emt_resultChart3.FilterHandle;
            result = Emt_resultChart3.Result;

            AddFilterToQueryRequest Emt_requestChart5 = new AddFilterToQueryRequest();
            var Emt_resultChart5 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, Emt_filterHandleforPlant, 0, 0, 0);
            result = Emt_resultChart5.Result;

            CreateFilterRequest Emt_requestChart312 = new CreateFilterRequest(response.Handle, 1, 0004, "0", 5);
            var Emt_resultChart312 = await ws.CreateFilterAsync(Emt_requestChart312);
            var Emt_filterHandleforformdate = Emt_resultChart312.FilterHandle;
            result = Emt_resultChart312.Result;

            var Emt_resultChart412 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, Emt_filterHandleforformdate, 2, 0, 0);
            result = Emt_resultChart412.Result;

            CreateFilterRequest Emt_request4 = new CreateFilterRequest(response.Handle, 1, 2005, "0", 2);
            var Emt_resultChar4 = await ws.CreateFilterAsync(Emt_request4);
            var Emt_Class_Handle = Emt_resultChar4.FilterHandle;
            result = Emt_resultChar4.Result;

            AddFilterToQueryRequest Emt_request5 = new AddFilterToQueryRequest();
            var Emt_resultChar5 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, Emt_Class_Handle, 1, 0, 0);
            result = Emt_resultChar5.Result;

            CreateFilterRequest Emt_requestChart31 = new CreateFilterRequest(response.Handle, 1, 0, "0", 129);
            var Emt_resultChart31 = await ws.CreateFilterAsync(Emt_requestChart31);
            var Emt_Filter_Handle = Emt_resultChart31.FilterHandle;

            var Emt_resultChart41 = await ws.AddFilterToQueryAsync(response.Handle, queryHandle, Emt_Filter_Handle, 2, 0, 0);
            result = Emt_resultChart41.Result;

            ExecuteQueryRequest Emt_requestChart7 = new ExecuteQueryRequest();
            var Emt_resultChart7 = await ws.ExecuteQueryAsync(response.Handle, queryHandle, Emt_partListStr);
            try
            {
                if (Emt_resultChart7 != null)
                {
                    result = Emt_resultChart7.Result;
                }
                else
                {
                    FileWriter.WriteToFile(plant + "Plant DwnStructure page ExecuteQueryRequest is null");
                    Console.WriteLine(plant + "Plant DwnStructure page ExecuteQueryRequest is null");
                    //return Tuple.Create(test);
                }
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(plant + "Plant DwnStructure page ExecuteQueryRequest Exception :" + ex.Message);
                Console.WriteLine(plant + "Plant DwnStructure page ExecuteQueryRequest Exception :" + ex.Message);
            }

            FreeQueryRequest Emt_freeQueryRequest1 = new FreeQueryRequest();
            var Emt_resultChart51 = await ws.FreeQueryAsync(response.Handle, queryHandle);
            result = Emt_resultChart51.Result;

            CreateFilterRequest Emt_requestChart77 = new CreateFilterRequest();
            var Emt_resultChart77 = await ws.EvaluateAllCharsAsync(response.Handle);
            var Emt_result2 = Emt_resultChart77.Result;

            GetGlobalInfoRequest Emt_requestChart8 = new GetGlobalInfoRequest(response.Handle, 1, 0, 1);
            var Emt_resultChart8 = await ws.GetGlobalInfoAsync(Emt_requestChart8);
            result = Emt_resultChart8.Result;
            var Emt_partCount = Emt_resultChart8.ret;
            Console.WriteLine(plant + " plant loaded partCount: " + Emt_partCount);
            FileWriter.WriteToFile(plant + " plant loaded partCount: " + Emt_partCount);

            for (int l = 1; l <= Emt_partCount; l++)
            {
                GetPartInfoRequest Emt_requestChar823 = new GetPartInfoRequest(response.Handle, 1000, l, 0);
                var Emt_resultChar823 = await ws.GetPartInfoAsync(Emt_requestChar823);
                var Cp_Id = Emt_resultChar823.KFieldValue;

                //Console.WriteLine(Cp_Id);

                Cp_obj.Add(new Cplan { Cplan_ID = Cp_Id });
            }

            if (plant == Pune_plant)
            {
                CP_ID_PNE = Cp_obj;
            }
            if (plant == PantN_plant)
            {
                CP_ID_PNT = Cp_obj;
            }
            if (plant == Jmr_plant)
            {
                CP_ID_JMS = Cp_obj;
            }
            return Cp_obj;

        }
    }
}
