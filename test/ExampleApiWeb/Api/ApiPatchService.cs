using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ExampleApiWeb.Code;
using ExampleApiWeb.Framework.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleObjectPatch;

namespace ExampleApiWeb.Api
{
    public class ApiPatchService
    {
        private readonly IMapper _mapper;
        private readonly Validator _validator;
        private readonly IAuthorizationService _authorizationService;

        public ApiPatchService(IMapper mapper, Validator validator, IAuthorizationService authorizationService)
        {
            _mapper = mapper;
            _validator = validator;
            _authorizationService = authorizationService;
        }

        public async Task<TContract> Post<TContract, TModel>([FromBody] PatchObject<TContract> value, ClaimsPrincipal principal, Func<TModel, Task<TModel>> store) where TContract : class, new()
        {
            var dto = value.Create();
            _validator.ValidateAndThrow(dto, RuleSet.Add);
            var newCustomer = _mapper.Map<TModel>(dto);
            await _authorizationService.AssertAuthorizeAsync(principal, newCustomer, Requirements.Create);
            TModel dbObject = await store(newCustomer);
            return _mapper.Map<TContract>(dbObject);
        }

        public async Task<TContract> Put<TContract, TModel>([FromBody]PatchObject<TContract> value, ClaimsPrincipal principal, TModel original, Func<TModel, Task<TModel>> store) where TContract : class, new()
        {
            TContract dto = _mapper.Map<TContract>(original);
            value.Patch(ref dto);
            _validator.ValidateAndThrow(dto, RuleSet.Update);
            original = _mapper.Map<TModel>(dto);
            await _authorizationService.AssertAuthorizeAsync(principal, original, Requirements.Update);
            original = await store(original);
            return _mapper.Map<TContract>(original);
        }

        public async Task<bool> Delete<TContract>([FromBody] TContract value, ClaimsPrincipal principal, Func<Task<bool>> store) where TContract : class, new()
        {
            _validator.ValidateAndThrow(value, RuleSet.Delete);
            await _authorizationService.AssertAuthorizeAsync(principal, value, Requirements.Delete);
            await store();
            return true;
        }
    }
}