using Microsoft.AspNetCore.Builder; //for classType:WebApplication 
using App.RouteBindings;
using dotnetWebService.RouteBindings;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using dotnetWebService.Core.CheckAuthorization;

namespace App.Routings
{
    public static class Routes_Authentication
    {
        
       
        public static WebApplication Add_Authentication_Routing(this WebApplication app)
        {
            app.MapPost("/CheckUserAuthorization", CheckAuthorization.CheckUserAuthorization);
            app.MapPost("/GetUserDetailsByUsername", UserManagement.GetUserDetailsByUsername);

            return app;
        }
    }
}
