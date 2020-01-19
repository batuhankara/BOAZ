using BAOZ.Common;
using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Linq;
using User.Core.Domain.Aggregates;

namespace User.Core.Domain.Commands
{
    [Validator(typeof(UpdateUserCommandValidator))]
    public class UpdateUserCommand : BaseCommand<UserAggregate, BaozId, CommandResult>
    {
        public UpdateUserCommand(Guid id, string firstName) : base(new BaozId(id.ToString()))
        {
            Id = id;
            FirstName = firstName;
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public override ValidationResult Validate()
        {
            var validator = new UpdateUserCommandValidator();

            var result = validator.Validate(this);

            return new ValidationResult(result.Errors.Select(e => new ValidationFailure(e.PropertyName, e.ErrorMessage, e.ErrorMessage, e.AttemptedValue)));
        }
    }
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {

            RuleFor(user => user.FirstName).NotNull().NotEmpty().WithMessage("can not be null");
        }
    }

}
