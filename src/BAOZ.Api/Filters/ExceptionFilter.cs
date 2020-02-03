using BAOZ.Api.Models;
using BAOZ.Api.Models.ExceptionModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BAOZ.Api.Filters
{

    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var badRequest =(int) HttpStatusCode.BadRequest;
            String message = String.Empty;
            var exceptionType = context.Exception.GetType();
            HttpResponse response = context.HttpContext.Response;


            if (exceptionType == typeof(ModelValidationException))
            {
                context.ExceptionHandled = true;

                response.StatusCode = badRequest;
                response.ContentType = "application/json";
                var convertedEx = ((ModelValidationException)context.Exception);
                var err = new ApiResponse()
                {
                    IsError = true,
                    Data = null,
                    ApiException = new ApiException()
                    {
                        apiValidationErrors = convertedEx.apiValidationErrors,
                        IsValidationError = true,

                    }
                };
                var json = JsonConvert.SerializeObject(err);
                response.WriteAsync(json);
            }

        }
    }
}

