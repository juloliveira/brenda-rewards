using Brenda.Contracts.Emails;
using Brenda.Contracts.V1.Requests;
using Brenda.Web.Models;
using FluentValidation;
using System;

namespace Brenda.Web.Validators
{
    public class NewUserConfirmationValidator : AbstractValidator<NewUserConfirmation>
    {
        public NewUserConfirmationValidator()
        {
            RuleFor(x => x.Password).Password();
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("A confirmação da senha incorreto")
                .When(x => !string.IsNullOrWhiteSpace(x.Password));
        }
    }
    public class SignUpValidator : AbstractValidator<SignUpPost>
    {
        public SignUpValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty()
                .MinimumLength(5);
            RuleFor(x => x.CompanyDocument).IsValidCNPJ();
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(5);
            RuleFor(x => x.Email)
                .EmailAddress();
            RuleFor(x => x.Password).Password(minimumLength: 8);
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .When(x => !string.IsNullOrWhiteSpace(x.Password));
        }
    }

}
