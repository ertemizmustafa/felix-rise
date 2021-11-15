using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Felix.Common.Helpers
{
    public class ValidationHelper : IValidatorInterceptor
    {
        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {
            var projection = result.Errors.Select(failure => new ValidationFailure(failure.PropertyName, $"Property: {failure.PropertyName}, AttemptedValue: {failure.AttemptedValue}, ErrorMessage = {failure.ErrorMessage}"));
            return new ValidationResult(projection);

        }

        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            return commonContext;
        }
    }
}
