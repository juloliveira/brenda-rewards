using FluentValidation;

namespace Brenda.Web.Validators
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 8)
        {
            var options = ruleBuilder
                .NotEmpty().WithMessage("A senha não pode ser vazia")
                .MinimumLength(minimumLength).WithMessage($"Sua tenha deve conter pelo menos {minimumLength} caracteres")
                .Matches("[A-Z]").WithMessage("Sua senha deve conter no mínimo uma letra maiúscula")
                .Matches("[a-z]").WithMessage("Sua senha deve conter no mínimo uma letra minúscula")
                .Matches("[0-9]").WithMessage("Sua senha deve conter no mínimo um número");

            return options;
        }

        public static IRuleBuilder<T, double?> Latitude<T>(this IRuleBuilder<T, double?> ruleBuilder)
        {
            var options = ruleBuilder
                .NotNull()
                .InclusiveBetween(-90, 90);
            return options;
        }
        public static IRuleBuilder<T, double?> Longitude<T>(this IRuleBuilder<T, double?> ruleBuilder)
        {
            var options = ruleBuilder
                .NotNull()
                .InclusiveBetween(-180, 180);
            return options;
        }
    }
}
