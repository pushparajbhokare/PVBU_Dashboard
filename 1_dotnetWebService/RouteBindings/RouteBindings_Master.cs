using Microsoft.AspNetCore.Http; //to Use Results,IResult Type

namespace App.RouteBindings
{
    public static class RouteMethods
    {
        public static IResult pageRedirect(HttpRequest request)
        {
            return Results.LocalRedirect($"~{request.Path}/index.html", false, true);
        }
    }
}