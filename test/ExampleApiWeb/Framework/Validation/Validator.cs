using ExampleApiWeb.Api.v1.Contracts;
using ExampleApiWeb.Code;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExampleApiWeb.Framework.Validation
{
    public class Validator
    {
        private readonly Dictionary<Type, IValidator> _validators;

        public Validator()
            : this(GetValidators())
        {
        }

        public Validator(IEnumerable<IValidator> validators)
        {
            _validators = validators.ToDictionary(validator => validator.GetType(), validator => validator);
        }

        public void ValidateAndThrow<T>(T instance, string ruleSet = null)
        {
            var validator = _validators.SingleOrDefault(v => v.Value is IValidator<T>).Value as IValidator<T>;

            if (validator == null)
            {
                return;
            }

            var validationResult = validator.Validate(instance, ruleSet: ruleSet);

            if (!validationResult.IsValid)
            {
                var error = new Error();
                throw new CustomHttpException(HttpStatusCode.BadRequest, error);
            }
        }

        private static IEnumerable<IValidator> GetValidators()
        {
            return new List<IValidator>();
        }
    }

    public static class RuleSet
    {
        public const string Add = "add";
        public const string Update = "update";
        public const string Delete = "delete";
        public const string UserChangePassword = "userChangePassword";
        public const string AddExisting = "addExisting";
    }
}