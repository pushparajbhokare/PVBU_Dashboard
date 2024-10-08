//using App.Configurations;
//using System;
//using System.Net.Http;
//using System.Threading.Tasks;

//class TargetApiService
//{
//    public static async Task mainAsync(int partnr, int charnr, string SetKey, runTimeConfiguration configManager)
//    {

//        //runTimeConfiguration configManager = new runTimeConfiguration();

//        var baseadd = configManager.getParticularConfig("WebService_Config", "AppPort-http");
//        string baseUrl = $"http://localhost:{baseadd}";
//        string url = $"{baseUrl}/ins_target_value?partnr={partnr}&charnr={charnr}&SetKey={SetKey}";

//        try
//        {
//            using (var httpClient = new HttpClient())
//            {
//                httpClient.Timeout = TimeSpan.FromMinutes(10);
//                HttpResponseMessage response = await httpClient.GetAsync(url);

//                if (response.IsSuccessStatusCode)
//                {
//                    string responseBody = await response.Content.ReadAsStringAsync();
//                    Console.WriteLine("Response:");
//                    Console.WriteLine(responseBody);
//                }
//                else
//                {
//                    Console.WriteLine($"Error: {response.StatusCode}");
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"An error occurred: {ex.Message}");
//        }
//    }
//}
