using BAOZ.Common;
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
        public CreateUserCommand(Guid userId, string firstName) : base(new BaozId(userId.ToString()))
        {
            UserId = userId;
            FirstName = firstName;
        }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public override ValidationResult Validate()
        {
            var validator = new CreateUserCommandValidator();

            var result = validator.Validate(this);

            return new ValidationResult(result.Errors.Select(e => new ValidationFailure(e.PropertyName, e.ErrorMessage, e.ErrorMessage, e.AttemptedValue)));
        }
    }
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {

            RuleFor(user => user.FirstName).NotNull().NotEmpty().WithMessage("can not be null");
        }
    }
}
