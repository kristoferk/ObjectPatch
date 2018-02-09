using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ExampleApiWeb.Framework.Models;
using Xunit;

namespace UnitTest
{
    public class IntegrationTests
    {
        [Fact]
        public async Task TestPostPatch()
        {
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:1995/") };
            var message = new HttpRequestMessage(HttpMethod.Post, new Uri("/api/v1/customers/", UriKind.Relative));
            var json = JsonConvert.SerializeObject(new { Description = "Desc", Alias = "Desc2" });
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(message);
            Assert.True(response.IsSuccessStatusCode);
            var customer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());

            //Description is patchable and should be changed
            Assert.True(customer.Description == "Desc");

            //Description is NOT patchable and should not be changed
            Assert.True(customer.Alias != "Desc2");
        }

        [Fact]
        public async Task TestPutPatch()
        {
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:1995/") };
            var message = new HttpRequestMessage(HttpMethod.Put, new Uri("/api/v1/customers/2", UriKind.Relative));
            var json = JsonConvert.SerializeObject(new { Id=2, Description = "Desc", Page = 2 });
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(message);
            Assert.True(response.IsSuccessStatusCode);
            var customer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
            Assert.True(customer.Id == 2);
            Assert.True(customer.Description == "Desc");
            Assert.True(customer.Page == 1);
        }
    }
}