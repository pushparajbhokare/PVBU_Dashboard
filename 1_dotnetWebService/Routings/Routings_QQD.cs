using Microsoft.AspNetCore.Builder; //for classType:WebApplication 
using App.RouteBindings;
//using Kneo.Database.Single.Tenant;

namespace App.Routings
{
    public static class Routes_QQD
    {
        public static WebApplication Add_QQD_Routing(this WebApplication app)
        {
            app.MapGet("/Dashboard", RouteMethodsExample.testJsonData);
            app.MapGet("/TestINI", RouteMethodsExample.testConfig);            
            app.MapGet("/GetPlants", RouteMethodsExample.GetPlants);            
            return app;
        }
    }
}
