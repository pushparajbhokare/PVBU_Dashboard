using App.Configurations;
using dotnetWebService.RouteBindings;
using Microsoft.AspNetCore.Http;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace dotnetWebService.BackendServices
{ 
    public class WebServiceManager
    {
        public Qdas_Web_ServiceClient ws;
        public int mainHandle;
        public WebConnectResponse response;
        
        public WebServiceManager(string username, string pass, Qdas_Web_ServiceClient.EndpointConfiguration configuration)
        
        {
            try
            {
                WebConnectRequest request = new WebConnectRequest(20, 44, username, pass, "");
                ws = new Qdas_Web_ServiceClient(configuration);

                runTimeConfiguration config = new runTimeConfiguration();
                var Time = config.getParticularConfig("Close_ApiTime", "Time");

                var securityMode = BasicHttpSecurityMode.None;

                var binding = new BasicHttpBinding(securityMode);
                binding.CloseTimeout = TimeSpan.FromMinutes(Convert.ToInt32(Time));
                binding.OpenTimeout = TimeSpan.FromMinutes(Convert.ToInt32(Time));
                binding.ReceiveTimeout = TimeSpan.FromMinutes(Convert.ToInt32(Time));
                binding.SendTimeout = TimeSpan.FromMinutes(Convert.ToInt32(Time));
                ws.Endpoint.Binding = binding; // Apply the binding configuration

                response = ws.WebConnectAsync(request).GetAwaiter().GetResult();
                mainHandle = response.Handle;
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
           

        }
        public void CloseConnection()
        {
            ws.ClientDisconnectAsync(mainHandle);
            Console.WriteLine("Connection close...");
        }

        public void Dispose()
        {
            CloseConnection();
        }      
        
    }

    
}
