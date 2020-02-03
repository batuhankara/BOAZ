using BAOZ.Api.Models;
using BAOZ.Api.Models.ExceptionModels;
using BAOZ.Common;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAOZ.Api.Filters
{

    public class ModelStateFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var apiException = new ModelValidationException();
                foreach (var modelStateKey in context.ModelState.Keys)
                {
                    var modelStateVal = context.ModelState[modelStateKey];


                    var validationExceptions = new ApiValidationError()
                    {
                        Key = modelStateKey,
                        Values = modelStateVal.Errors.Select(x => x.ErrorMessage).ToArray(),
                    };
                    apiException.apiValidationErrors.Add(validationExceptions);

                }
                throw apiException;

            }
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
