using System.Net.Http;
using System.Threading.Tasks;
using Api.Application;
using Api.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;

namespace Api.Integration.Test
{
    public class BaseIntegration
    {
        public MyContext myContext { get; set; }
        public HttpClient client { get; set; }
        public string hostApi { get; set; }
        public HttpResponseMessage response { get; set; }

        public BaseIntegration()
        {
            hostApi = "http://localhost:5000/api/v1/order";
            var builder = new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>();
            var server = new TestServer(builder);

            client = server.CreateClient();
        }

        public static async Task<HttpResponseMessage> PostJsonAsync(object dataclass, string url, HttpClient client)
        {
            return await client.PostAsync(url,
                new StringContent(JsonConvert.SerializeObject(dataclass), System.Text.Encoding.UTF8, "application/json"));
        }
    }
}