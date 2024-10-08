using App.RouteBindings;
using dotnetWebService.RouteBindings;
using Microsoft.AspNetCore.Builder; //for classType:WebApplication 
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using dotnetWebService.Core.DB;
using dotnetWebService.Authentication;
using System;
using System.Linq;
using dotnetWebService.Model;
//using App.Configurations;

namespace App.Routings
{
    public static class Routes
    {
        public static WebApplication AddRouting(this WebApplication app)
        {
            app.MapGet("/", RouteMethodsExample.MoveToHomeScreen).AllowAnonymous();

            app.MapPost("/login", (LoginRequest loginRequest, IAuthentication IAuthentication, TokenService toeknService) =>
            {
                AuthUser authUser = IAuthentication.Login(loginRequest.userId.Trim(), loginRequest.password);
                
                if(authUser == null)
                {
                    return Results.Unauthorized();
                }
                var token = toeknService.GenerateToken(authUser);
                var userDetail = UserManagement.GetUserDetailsByUsername(loginRequest.userId);
                
                return Results.Ok(new { token = token, name= authUser.DisplayName, landing_page = userDetail.landing_page });
            }).AllowAnonymous();

            app.MapGet("/favicon.ico", () => { }).AllowAnonymous();

            app.MapPost("/addUserByAdmin", (IAuthentication auth, AddUserRequest request) =>
            {
                var adminUser = auth.AddUserByAdmin(request.AdminPassword, request.AdminUserId, request.UserId);

                if (adminUser == null)
                {
                    return Results.NotFound();
                }
                var userDetail = UserManagement.GetUserDetailsByUsername(request.UserId);

                return Results.Ok(new { name = adminUser.DisplayName, userId = adminUser.UserId, mail=adminUser.Mail }); ;
            });

            // 

            app.MapGet("/img/icons", () => { }).AllowAnonymous();

            app.MapGet("/userDetails", (ClaimsPrincipal user) =>
            {
                string name = user.Identity.Name;
                string userId = user.Claims.First(c => "UserId".Equals(c.Type)).Value;
                Console.WriteLine($"Claim for user id = {userId}");
                if (name == null || userId == null)
                {
                    return Results.Unauthorized();
                }

                User userDetail = UserManagement.GetUserDetailsByUsername(userId);
                userDetail.full_name = name;
                return Results.Ok(userDetail);
            });


            app.MapGet("/TML/dashboard", RouteMethods.pageRedirect);
            app.MapGet("/TML/2x2matrix", RouteMethods.pageRedirect);
            app.MapGet("/TML/plantStatus", RouteMethods.pageRedirect);
            app.MapGet("/TML/AreaExplorer", RouteMethods.pageRedirect);
            app.MapGet("/TML/ModalExplorer", RouteMethods.pageRedirect);
            app.MapGet("/TML/CharacteristicExplorer", RouteMethods.pageRedirect);
            app.MapGet("/TML/TargetInspection", RouteMethods.pageRedirect);            
            app.MapGet("/GetCharCharts", CharCharts.GetCharCharts);
            app.MapGet("/GetPartReport", Part_Report.GetPartReport);
            app.MapGet("/GetPlantData", InsCVBU_Target.GetPlantData);
            app.MapGet("/AddPlantsFromConfiguration", InsCVBU_Target.AddPlantsFromConfiguration);
            app.MapGet("/Test_char_info", AllApi.Test_char_info);
            app.MapGet("/Test_quadrant", AllApi.Test_quadrant);
            app.MapGet("/Test_area_explorer", AllApi.Test_area_explorer);
            app.MapGet("/Test_model_explorer", AllApi.Test_model_explorer);
            app.MapGet("/Test_plant_quadrant", AllApi.Test_plant_quadrant);
            app.MapGet("/Test_CVBU_quadrant", AllApi.Test_CVBU_quadrant);
            app.MapGet("/Test_waterfall_dia", AllApi.Test_waterfall_dia);
            app.MapGet("/Crit_Diffrence", AllApi.Crit_Diffrence);
            //app.MapGet("/SetTarget", SetTargetValue.SetTarget);
            app.MapGet("/Test_Inspection", AllApi.Test_Inspection);
            //app.MapGet("/SetConfigValue", SetTargetValue.SetTarget);  
            app = app.Add_Authentication_Routing();
            app = app.Add_QQD_Routing();
            app = app.Add_RoleManagement_Routing();


            return app;
        }
    }
}