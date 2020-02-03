using BAOZ.Common;
using BAOZ.Common.ValidationHelpers;
using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Linq;
using User.Core.Domain.Aggregates;

namespace User.Core.Domain.Commands
{

    [Validator(typeof(CreateUserCommandValidator))]
    public class CreateUserCommand : BaseCommand<UserAggregate, BaozId, CommandResult>
    {
        public CreateUserCommand(Guid userId, string firstName, string password, string lastName, string phoneNumber, string countryCode) : base(new BaozId(userId.ToString()))
        {
            UserId = userId;
            FirstName = firstName;
            Password = password;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            CountryCode = countryCode;
        }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
    }
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {

            RuleFor(user => user.FirstName).NotNull().NotEmpty();
            RuleFor(user => user.LastName).NotNull().NotEmpty();
            RuleFor(user => user.PhoneNumber).Must(PhoneNumberValidation.IsNumeric).Length(10).WithMessage("Invalid Phone Number");
            RuleFor(user => user.CountryCode).Must(PhoneNumberValidation.IsNumeric).Length(2);

        }
    }
}
