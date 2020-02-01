using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Core.Domain.ValidatorRegisters;

namespace BAOZ.Api.ValidationModules
{
    public static class WebApiValidationModule
    {
        public static IServiceCollection AddWebApiValidations(this IServiceCollection services)
        {
            services.AddUserCommandValidator();
            return services;
        }
    }
}
