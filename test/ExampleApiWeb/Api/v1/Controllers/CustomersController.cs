using AutoMapper;
using ExampleApiWeb.Api.v1.Contracts;
using ExampleApiWeb.Code;
using ExampleApiWeb.Code.Repository;
using ExampleApiWeb.Framework;
using ExampleApiWeb.Framework.Repositories;
using ExampleApiWeb.Framework.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleObjectPatch;
using System.Threading.Tasks;
using ExampleApiWeb.Framework.Models;

namespace ExampleApiWeb.Api.v1.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/[controller]")]
    public class CustomersController : Controller
    {
        private readonly ApiPatchService _service;
        private readonly CustomerRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public CustomersController(ApiPatchService service, CustomerRepository repository, IMapper mapper, Validator validator, IAuthorizationService authorizationService)
        {
            _service = service;
            _repository = repository;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        // GET api/customers/5
        [HttpGet("{id}")]
        public async Task<CustomerDto> GetById(int id)
        {
            Customer dbObject = await _repository.GetAsync(id);
            await _authorizationService.AuthorizeAsync(User, dbObject, Requirements.Read);
            return _mapper.Map<CustomerDto>(dbObject);
        }

        // GET api/customers/5
        [HttpGet("{id}")]
        public async Task<ApiCollectionDto<CustomerDto>> Get(CustomerFilterDto dto)
        {
            var filter = _mapper.Map<CustomerFilter>(dto);
            ApiCollection<Customer> list = await _repository.GetAsync(filter);
            return _mapper.Map<ApiCollectionDto<CustomerDto>>(list);
        }

        // POST api/customers
        [HttpPost]
        public async Task<CustomerDto> Post([FromBody] PatchObject<CustomerDto> value)
        {
            return await _service.Post<CustomerDto, Customer>(value, User, async c => await _repository.CreateAsync(c));
        }

        //PUT api/customers/5
        [HttpPut("{id}")]
        public async Task<CustomerDto> Put(int id, [FromBody]PatchObject<CustomerDto> value)
        {
            CheckRequestIdentity.AssertEqualId(id, value.Data.Id, i => value.Data.Id = i);
            Customer original = await _repository.GetAsync(value.Data.Id);
            return await _service.Put(value, User, original, async c => await _repository.UpdateAsync(c));
        }

        // DELETE api/customers/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id, [FromBody] CustomerDto value, bool hardDelete = false)
        {
            value = value ?? new CustomerDto();
            CheckRequestIdentity.AssertEqualId(id, value.Id, i => value.Id = i);
            
            return await _service.Delete(value, User, async () => {
                await _repository.DeleteAsync(id, hardDelete);
                return true;
            });
        }
    }
}