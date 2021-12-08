using FluentValidation;

namespace Sara.Api.Validators
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 8)
        {
            var options = ruleBuilder
                .NotEmpty()
                .MinimumLength(minimumLength)
                .Matches("[A-Z]")
                .Matches("[a-z]")
                .Matches("[0-9]");
                //.Matches("[^a-zA-Z0-9]");

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
