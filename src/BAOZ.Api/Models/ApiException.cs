using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BAOZ.Api.Models
{
   
    public class ApiException
    {
        public ApiException()
        {
            apiValidationErrors = new List<ApiValidationError>();
        }

        public bool IsValidationError { get; set; }
        public object Exception { get; set; }
        public List<ApiValidationError> apiValidationErrors { get; set; }

    }
}
