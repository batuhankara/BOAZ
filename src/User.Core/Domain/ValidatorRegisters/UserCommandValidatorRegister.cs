using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using User.Core.Domain.Commands;

namespace User.Core.Domain.ValidatorRegisters
{
    public static class UserCommandValidatorRegister
    {
        public static IServiceCollection AddUserCommandValidator(this IServiceCollection service)
        {
            service.AddTransient<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
            return service;
        }
    }
}
