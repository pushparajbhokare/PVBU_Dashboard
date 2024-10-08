using dotnetWebService.Core.CheckAuthorization;
using dotnetWebService.Core.Service;
using dotnetWebService.RouteBindings;
using Microsoft.AspNetCore.Builder;

namespace App.Routings
{
    public static class Routings_RoleManagement
    {
        public static WebApplication Add_RoleManagement_Routing(this WebApplication app)
        {
            app.MapGet("/GetRoleList",RoleManagement.GetRoleList);
            app.MapGet("/GetUserList", RoleManagement.GetUserList);
            app.MapPost("/GetRoleDetailsByID", RoleManagement.GetRoleDetailsByID);
            app.MapPost("/AddUpdateRoleInfo", RoleManagement.AddUpdateRoleInfo);
            app.MapPost("/DeleteRoleInfo", RoleManagement.DeleteRoleInfo);
            app.MapGet("/GetRolePermissionList", RoleManagement.GetRolePermissionList);
            app.MapPost("/SaveRolePermissionInfo", RoleManagement.SaveRolePermissionInfo);            
            app.MapPost("/DeleteUser", RoleManagement.DeleteUser);            
            app.MapPost("/UpdateUser", RoleManagement.UpdateUser);            
            app.MapGet("/GetPlantList", PlantAreaService.GetPlantList);            
            app.MapGet("/GetAreaList", PlantAreaService.GetAreaList);


            return app;
        }
    }
}
