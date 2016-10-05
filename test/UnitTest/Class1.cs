using ExampleApiWeb.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest
{
    public class Class1
    {
        [Fact]
        public async Task TestPatch()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:1995/");
            var message = new HttpRequestMessage(HttpMethod.Post, new Uri("/api/v1/customers/", UriKind.Relative));
            var json = JsonConvert.SerializeObject(new { Description = "Desc"});
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(message);
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}