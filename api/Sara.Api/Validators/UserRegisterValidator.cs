using FluentValidation;
using Sara.Contracts.Security;
using System;
using System.Data;

namespace Sara.Api.Validators
{
    public class UserSignInValidator : AbstractValidator<UserPass>
    {
        public UserSignInValidator()
        {
            RuleFor(x => x.Username).EmailAddress();
            RuleFor(x => x.Password).Password();
        }
    }

    public class UserRegisterValidator : AbstractValidator<UserRegister>
    {
        public UserRegisterValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MinimumLength(13);

            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Birthdate)
                .NotNull()
                .GreaterThan(new DateTime(1930, 1, 1))
                .LessThan(p => DateTime.UtcNow);

            RuleFor(x => x.Document).Length(14);
            
            RuleFor(x => x.Sex).InclusiveBetween(1, 2);
            RuleFor(x => x.GenderIdentityId).NotNull();
            RuleFor(x => x.IncomeId).NotNull();
            RuleFor(x => x.Latitude).Latitude();
            RuleFor(x => x.Longitude).Longitude();
        }
    }
}
