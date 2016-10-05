using ExampleApiWeb.Model;
using Microsoft.AspNetCore.Mvc;
using SimpleObjectPatch;
using System.Threading.Tasks;

namespace ExampleApiWeb.Controllers
{
    [Route("api/v1/[controller]")]
    public class CustomersController : Controller
    {
        // GET api/customers/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/customers
        [HttpPost]
        public async Task<Customer> Post([FromBody] PatchObject<Customer> value)
        {
            var original = await value.CreateAsync();
            return original;
        }

        //PUT api/customers/5
        [HttpPut("{id}")]
        public async Task<Customer> Put(int id, [FromBody]PatchObject<Customer> value)
        {
            var original = new Customer {
                Id = 3,
                Name = "Test"
            };

            original = await value.PatchAsync(original);
            return original;
        }

        // DELETE api/customers/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}