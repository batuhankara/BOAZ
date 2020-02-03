using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAOZ.Api.Models.ExceptionModels
{
    public class ModelValidationException : Exception
    {
        public ModelValidationException()
        {
            apiValidationErrors = new List<ApiValidationError>();
        }

        public bool IsValidationError { get; set; }
        public object Exception { get; set; }
        public List<ApiValidationError> apiValidationErrors { get; set; }
    }
}
