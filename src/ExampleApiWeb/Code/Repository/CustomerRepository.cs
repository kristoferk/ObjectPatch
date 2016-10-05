using System.Threading.Tasks;
using ExampleApiWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApiWeb.Code.Repository
{
    public class CustomerRepository 
    {
        public Customer Get(int id)
        {
            return new Customer {
                Id = 3,
                Name = "Test"
            };
        }


        public Customer Create(Customer value)
        {
            return value;
        }

        public Customer Update([FromBody]Customer value)
        {
            return value;
        }


        public void Delete(int id, bool hardDelete = false)
        {
        }

        public Task<Customer> GetAsync(int id)
        {
            return Task.FromResult(new Customer {
                Id = 3,
                Name = "Test"
            });
        }


        public Task<Customer> CreateAsync(Customer value)
        {
            return Task.FromResult(value);
        }

        public Task<Customer> UpdateAsync([FromBody]Customer value)
        {
            return Task.FromResult(value);
        }


        public Task DeleteAsync(int id, bool hardDelete = false)
        {
            return Task.FromResult(0);
        }
    }
}