// See https://aka.ms/new-console-template for more information
using Microsoft.AspNetCore.Builder; //for Api:WebApplication provider
using App.Configurations; //container of all configurations for applications
using Microsoft.Extensions.Configuration; //to access the AddJsonfile extension
using App.Routings; // to publish the routes to app
using BackendServices; //to add the services to the Webservice
using App.Middleware; // to add custom Middleware 
using Microsoft.AspNetCore.Hosting; // to use the extension ConfigureKestrel; 
using Microsoft.AspNetCore.Server.Kestrel.Core; // to use Enum of HttpProtocols;
using BackgroundHostingService; // to run the application as background service;
using dotnetWebService.RouteBindings;
using System;
using System.Threading;
using System.Runtime.InteropServices.ObjectiveC;
using dotnetWebService.BackendServices;
using Microsoft.Extensions.DependencyInjection;
using dotnetWebService.Model;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace WebService
{
    public static class lockHolder
    {
        public readonly static object Lock = new object();
    }
    class MainWebService
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Loading the configurations");
            var appConfigs = new SystemConfigurations();
            var builder = WebApplication.CreateBuilder(appConfigs.options);

            builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddIniFile(appConfigs.configFile,
                    optional: false,
                    reloadOnChange: true);
            });
            builder.WebHost.ConfigureKestrel(options => //apply certificate details of https connection 
            {
                options.ConfigureHttpsDefaults(httpsOptions =>
                {
                    var configMgr = new runTimeConfiguration();
                    var fileName= configMgr.getParticularConfig("WebService_Config","certName");
                    var filePassword= configMgr.getParticularConfig("WebService_Config","certPassword");
                    var cert = new X509Certificate2(fileName, filePassword);
                    httpsOptions.ServerCertificate = cert;
                    //second option to use pem format 
                        //var certPath = Path.Combine(builder.Environment.ContentRootPath, "localhost.pem");
                        //var keyPath = Path.Combine(builder.Environment.ContentRootPath, "localhost.key");
                        //httpsOptions.ServerCertificate = X509Certificate2.CreateFromPemFile(certPath, keyPath);
                });
                //ref : https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/webapplication?view=aspnetcore-8.0#use-the-certificate-apis
                //ref :  https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-dev-certs
                //command : dotnet dev-certs https --format pem -ep /home/user/localhost.pem -p $CREDENTIAL_PLACEHOLDER$
                //ref : https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel/endpoints?view=aspnetcore-5.0#replace-the-default-certificate-from-configuration
            });
            builder.RunAsWindowService();

            builder.AddServices(appConfigs);

            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = options.DefaultPolicy;
            });

            var app = builder.Build();
            try
            {
                Thread thread = new Thread(async () =>
                {
                    while (true)
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();

                        await ManualServiceRunner.ManualService();
                        //Thread.Sleep(1000*3600);

                        stopwatch.Stop();

                        TimeSpan executionTime = stopwatch.Elapsed;

                        Console.WriteLine($"ManualService execution time: {executionTime}");
                        FileWriter.WriteToFile($"ManualService execution time: {executionTime}");
                        await Task.Delay(TimeSpan.FromMinutes(120)); //needed as breather to webservice
                    }
                });

                thread.Start();

            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }

            app = app.AddListentingPort();

            app.AddCustomMiddleware();

            app.UseCors();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.AddRouting();

            app.Run();
            //serviceScope.Dispose();
        }
    }
}

