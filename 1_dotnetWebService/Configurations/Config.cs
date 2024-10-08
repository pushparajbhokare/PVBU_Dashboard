using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.IO;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using dotnetWebService.Authentication;
using dotnetWebService.RouteBindings;

namespace App.Configurations
{

    public class SystemConfigurations
    {

        public WebApplicationOptions options { get; }

        public string configFile { get; }
        public string? resultFile { get; }
        public string? dataFile { get; }

        public string? qdasConfig { get; }

        public string? jwt_url { get; }


        public SystemConfigurations()
        {
            options = new WebApplicationOptions
            {
                WebRootPath = "AssetStorage"
            };
            //configFile = "ServerConfig.json"; //using INI file instead of json
            configFile = "QdasConfig.ini";
            /* not required anymore
            //resultFile = "result.dat";
            //dataFile="DataStorage/data.json";
            //qdasConfig="QdasConfig.toml"; //window background service issue is being handled at service initiation
            */
            //archived & alternative way of configuration 
            //qdasConfig=System.AppDomain.CurrentDomain.BaseDirectory+"QdasConfig.toml"; // BaseDirectory usage become essential during running webservice as window's background service 
            //read about in the ref here : 
            //https://stackoverflow.com/questions/2714262/relative-path-issue-with-net-windows-service;
            //https://haacked.com/archive/2004/06/29/current-directory-for-windows-service-is-not-what-you-expect.aspx/
            jwt_url = System.Environment.GetEnvironmentVariable("JWT_URL")!;
            //System.Environment.GetEnvironmentVariable("JWT_TEST_URL",System.EnvironmentVariableTarget.User);
            if (jwt_url == null)
            {
                jwt_url = "Jwt";
            }
        }


    }

    public static class ExtensionMethods
    {
        public static WebApplication AddListentingPort(this WebApplication app)
        {

            //get values from the Environment Variable [replaced with reading from IConfigurator]
            //string? httpPort = Environment.GetEnvironmentVariable("httpPort");           
            string? httpPort = app.Services.CreateScope()
                            .ServiceProvider
                            .GetRequiredService<IConfiguration>()["WebService_Config:AppPort-http"];
            app.Urls.Add($"http://0.0.0.0:{httpPort}");
            //string? httpsPort=Environment.GetEnvironmentVariable("httpsPort");
            string? httpsPort = app.Services.CreateScope()
                            .ServiceProvider
                            .GetRequiredService<IConfiguration>()["WebService_Config:AppPort-https"];
            app.Urls.Add($"https://0.0.0.0:{httpsPort}");
            return app;
        }
    }

    public class runTimeConfiguration
    {

        IConfigurationRoot Configuration;

        string? configFile;
        public string? jwt_url { get; }

        public runTimeConfiguration()
        {
            var systemConfiguration = new SystemConfigurations();
            configFile = systemConfiguration.configFile;
            Configuration = new ConfigurationBuilder().AddIniFile(configFile, false, true).Build();
            jwt_url = systemConfiguration.jwt_url;
        }

        public string getTokenHandlerSecret()
        {

            return Configuration["TokenHandlerSecret"]!;

        }

        public void getAllConfigSections()
        {
            var list = Configuration.GetChildren();
            foreach (var item in list)
            {
                System.Console.WriteLine(item.Key.ToString());
            }
        }
        public string getParticularConfig(string section, string key)
            {
            try
            {
                if (section != string.Empty)
                {
                    return Configuration.GetSection(section).GetSection(key).Value!.ToString();
                }
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
            
            return Configuration.GetSection(key).Value!.ToString();
        }

    }


    public class LdapConfiguration
    {

        IConfigurationRoot Configuration;

        string? configFile;
        public LdapConfig config { get; }

        public LdapConfiguration()
        {
            var systemConfiguration = new SystemConfigurations();
            configFile = systemConfiguration.configFile;
            Configuration = new ConfigurationBuilder().AddIniFile(configFile, false, true).Build();
            config = new();
            config.Path = getParticularConfig("authentication", "Path");
            config.UseServerCredentials = getParticularConfig("authentication", "UseServerCredentials");
            config.UserName = getParticularConfig("authentication", "UserName");
            config.Password = getParticularConfig("authentication", "Password");
            config.AuthenticationType = getParticularConfig("authentication", "AuthenticationType");
            config.Filter = getParticularConfig("authentication", "Filter");
            config.EnableAuth = "true".Equals(getParticularConfig("authentication", "EnableAuth"), StringComparison.OrdinalIgnoreCase) ? true : false;
            config.AuthTitle = getParticularConfig("authentication", "AuthTitle");
            config.AuthMail = getParticularConfig("authentication", "AuthMail");
            config.AuthGivenName = getParticularConfig("authentication", "AuthGivenName");
            config.AuthSn = getParticularConfig("authentication", "AuthSn");
            config.AuthCn = getParticularConfig("authentication", "AuthCn");
            config.AuthDisplayName = getParticularConfig("authentication", "AuthDisplayName");
        }

        public string getEnableAuth()
        {
            return Configuration.GetSection("authentication").GetSection("EnableAuth").Value!.ToString();
        }

        public string getParticularConfig(string section, string key)
        {
            if (section != string.Empty)
            {
                return Configuration.GetSection(section).GetSection(key).Value!.ToString();
            }
            return Configuration.GetSection(key).Value!.ToString();
        }
    }

    public class ConfigurationHelper

    {
        private string configFile;

        public ConfigurationHelper(string configFile)
        {
            this.configFile = configFile;
        }

        public string GetPlant()
        {
            string plant = null;

            if (configFile.EndsWith(".ini", System.StringComparison.OrdinalIgnoreCase))
            {
                // Read the .ini file and parse its content
                string[] lines = File.ReadAllLines(configFile);

                foreach (string line in lines)
                {
                    if (line.StartsWith("Plant="))
                    {
                        plant = line.Substring("Plant=".Length).Trim();
                        break;
                    }
                }
            }

            return plant;
        }
    }

    public class ConfigManager
    {
        private readonly string configFile;
        private readonly Dictionary<string, string> configValues = new Dictionary<string, string>();

        public ConfigManager(string configFile)
        {
            this.configFile = configFile;
            LoadConfig();
        }

        private void LoadConfig()
        {
            string[] lines = File.ReadAllLines(configFile);

            foreach (string line in lines)
            {
                if (line.Contains("="))
                {
                    string[] keyValue = line.Split('=');
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();
                    configValues[key] = value;
                }
            }
        }

        public string GetValue(string key)
        {
            return configValues.TryGetValue(key, out string value) ? value : null;
        }
    }

    public class WebServiceConfig
    {
        private readonly ConfigManager configManager;

        public WebServiceConfig(ConfigManager configManager)
        {
            this.configManager = configManager;
        }

        public string Username => configManager.GetValue("username");
        public string Password => configManager.GetValue("pass");
    }





}