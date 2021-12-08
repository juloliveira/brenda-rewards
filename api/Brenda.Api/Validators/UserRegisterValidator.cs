using Brenda.Contracts.V1.Requests;
using FluentValidation;
using System;

namespace Brenda.Api.Validators
{
    public class UserRegisterValidator : AbstractValidator<UserRegister>
    {
        public UserRegisterValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.MobilePhone)
                .NotEmpty()
                .MinimumLength(13);

            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Birthdate)
                .NotNull()
                .GreaterThan(new DateTime(1930, 1, 1))
                .LessThan(p => DateTime.Now);

            RuleFor(x => x.Password).Password();
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
            RuleFor(x => x.Sex).InclusiveBetween(1, 2);
            RuleFor(x => x.GenderIdentityId).NotNull();
            RuleFor(x => x.IncomeId).NotNull();
            RuleFor(x => x.Latitude).Latitude();
            RuleFor(x => x.Longitude).Longitude();
        }
    }
}
