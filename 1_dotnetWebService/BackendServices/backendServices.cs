using App.Configurations;
using dotnetWebService.Authentication;
using dotnetWebService.BackendServices;
using dotnetWebService.Core.DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; // to access type hostoption; BackgroundServiceExceptionBehavior  
using Microsoft.IdentityModel.Tokens;
using System;
//using Kneo.Service.QDas; // to acces teilService definition;
namespace BackendServices
{

    public static class Services{

        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder, SystemConfigurations configs ){
            //not to stop due to failure of background services
            builder.Services.Configure<HostOptions>(
                hostOptions=>
                {
                    hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
                }
            );
            builder.Services.AddCors(options =>
                {
                    options.AddDefaultPolicy(
                        policy =>
                        {
                            policy.AllowAnyOrigin();
                            policy.AllowAnyHeader();
                        });
                });
            builder.Services.AddSingleton<runTimeConfiguration>();
            builder.Services.AddSingleton<WebServiceManager>();

            // Authentication Service
            builder.Services.AddSingleton<TokenService>();
            builder.Services.AddScoped<IAuthentication, LdapAuthentication>();
            builder.Services.AddSingleton<LdapConfiguration>();
            builder.Services.AddSingleton<UserService>();

            var secretKey = Settings.GenerateSecretByte();

            builder.Services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //builder.Services.AddSingleton<ManualServiceRunner>();
            //builder.Services.AddSingleton<FaltuClass>(
            //    sp => {
            //        var webrunner = sp.GetRequiredService<ManualServiceRunner>();
            //        return new FaltuClass();
            //    });
            /*
            builder.Services.AddSingleton<IpartDataHandler,PartDataHandler>(sp=> {
                   var webHostEnvironment = sp.GetRequiredService<IWebHostEnvironment>();
                 return new PartDataHandler(webHostEnvironment.ContentRootPath+"/"+configs.qdasConfig);
               });
            builder.Services.AddSingleton<TEILService>();//Kneo SQL Fetcher
            */
            /* //NOTE: below services are used in another project 
            builder.Services.AddTransient<IFileHandler,ResultHandler>(
                sp=>{
                    var webHostEnvironment = sp.GetRequiredService<IWebHostEnvironment>();
                    return new ResultHandler(configs.storageDirectory, webHostEnvironment);
                    });
            builder.Services.AddTransient< IDataHandler, FormDataHandler>(
                sp=>{
                    var webHostEnvironment = sp.GetRequiredService<IWebHostEnvironment>();
                    return new FormDataHandler(configs.dataDirectory, webHostEnvironment);
                    });

            builder.Services.AddScoped<DbLayer>(sp=>{
                var opt = new dbOptions { //correctionPending
                dataSource = "localhost,1433",
                userID = "id", //qdas-Administrator           
                password = "password", //admin password    
                dbName="dbName" // value db name in qdas 
                    }; 
                return new DbLayer(opt);
            });
            */
            return builder;
        }
    }
}
