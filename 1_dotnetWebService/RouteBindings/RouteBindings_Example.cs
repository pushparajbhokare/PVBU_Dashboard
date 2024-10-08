using App.Configurations; // to test configuration
using Microsoft.AspNetCore.Http; //to Use Results,IResult Type
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace App.RouteBindings
{
    public static class RouteMethodsExample
    {
       public static IResult MoveToHomeScreen(HttpRequest request)
        {
            return Results.LocalRedirect("~/TML/signin/index.html", false, true);
        }
        public static async Task testJsonData(HttpContext context)
        {
            HttpClient client = new HttpClient { BaseAddress = new System.Uri("https://jsonplaceholder.typicode.com/") };
            string posts = await client.GetStringAsync("posts");
            await context.Response.WriteAsync(posts);
            client.Dispose();
        }

        public static async Task testConfig(HttpContext context, runTimeConfiguration configManager)
        {
            var result = configManager.getParticularConfig("qdas_value_db", "dataSource");
            await context.Response.WriteAsync(result);
        }
        public static async Task<string> GetPlants(runTimeConfiguration configManager)
        {
            var plantName = configManager.getParticularConfig("Pune_plant", "Plant");
            return plantName;
        }
    }
}