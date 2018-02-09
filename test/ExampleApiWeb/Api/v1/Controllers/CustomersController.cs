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
        private readonly CustomerRepository _repository;
        private readonly IMapper _mapper;
        private readonly Validator _validator;
        private readonly IAuthorizationService _authorizationService;

        public CustomersController(CustomerRepository repository, IMapper mapper, Validator validator, IAuthorizationService authorizationService)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
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
            var dto = value.Create();
            _validator.ValidateAndThrow(dto, RuleSet.Add);
            var newCustomer = _mapper.Map<Customer>(dto);
            await _authorizationService.AssertAuthorizeAsync(User, newCustomer, Requirements.Create);
            var dbObject = await _repository.CreateAsync(newCustomer);
            return _mapper.Map<CustomerDto>(dbObject);
        }

        //PUT api/customers/5
        [HttpPut("{id}")]
        public async Task<CustomerDto> Put(int id, [FromBody]PatchObject<CustomerDto> value)
        {
            CheckRequestIdentity.AssertEqualId(id, value.Data.Id, i => value.Data.Id = i);

            Customer original = _repository.Get(value.Data.Id);
            CustomerDto dto = _mapper.Map<CustomerDto>(original);
            dto = value.Patch(dto);
            _validator.ValidateAndThrow(dto, RuleSet.Update);
            original = _mapper.Map<Customer>(dto);
            await _authorizationService.AssertAuthorizeAsync(User, original, Requirements.Update);
            original = await _repository.UpdateAsync(original);
            return _mapper.Map<CustomerDto>(original);
        }

        // DELETE api/customers/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id, [FromBody] CustomerDto value, bool hardDelete = false)
        {
            value = value ?? new CustomerDto();
            CheckRequestIdentity.AssertEqualId(id, value.Id, i => value.Id = i);
            _validator.ValidateAndThrow(value, RuleSet.Delete);
            await _authorizationService.AssertAuthorizeAsync(User, value, Requirements.Delete);
            await _repository.DeleteAsync(id, hardDelete);
            return true;
        }
    }
}