using Brenda.Core;
using Brenda.Core.Identifiers;
using Brenda.Core.Interfaces;
using Brenda.Core.Services;
using Brenda.Core.Validations;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Brenda.Services
{
    public class CampaignValidation : ICampaignValidator
    {
        private PublishValidation _resultValidation;
        private readonly IErrorMessages _errorMessages;

        public CampaignValidation(IErrorMessages errorMessages)
        {
            _resultValidation = new PublishValidation();
            _errorMessages = errorMessages;
        }

        public async Task<PublishValidation> ValidateAsync(Campaign campaign)
        {
            if (campaign == null)
                throw new ArgumentNullException(nameof(campaign));

            await ValidateAsync(campaign, x => x.Title);
            await ValidateAsync(campaign, x => x.Balance);
            await ValidateAsync(campaign, x => x.Reward);
            await ValidateAsync(campaign.Definitions, x => x.ValidationStart, operation: Compare.LessThanOrEqual);
            await ValidateAsync(campaign.Definitions, x => x.ValidationEnd, operation: Compare.GreaterThanOrEqual);

            if (campaign.Definitions.ValidateGeoLocation)
                await ValidateCollectionAsync(campaign.Definitions, x => x.CoordinatesAllowed);

            if (Actions.IsChallenge(campaign))
            { 
                if (campaign.Campaigns == null || campaign.Campaigns.Count < 2)
                    await AddErroMessage("Challenge", "MustHaveCampaigns");

                if (campaign.Campaigns.Any(x => x.Balance > 0))
                    await AddErroMessage("Challenge", "CampaignsBalance");

                if (campaign.Campaigns.Any(x => Actions.IsChallenge(x.ActionId)))
                    await AddErroMessage("Challenge", "MustNotHaveChallenge");

                if (campaign.Campaigns.Any(x => !x.AssetId.HasValue))
                    await AddErroMessage("Challenge", "CampaignAsset");
            }
            else
            { 
                await ValidateAsync(campaign, x => x.AssetId); 
            }

            return _resultValidation;
        }

        private async Task ValidateCollectionAsync<TSource, TResult>(
            TSource source,
            Expression<Func<TSource, TResult>> expression) where TResult : System.Collections.IEnumerable
        {
            _resultValidation.AddValidation();
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            (var tag, var prop, var value) = GetValues(source, expression);

            var collection = value as System.Collections.ICollection;

            if (collection.Count == 0)
                await AddErroMessage(tag, prop).ConfigureAwait(false);
        }
        
        private async Task ValidateAsync<TSource, TResult>(
            TSource source,
            Expression<Func<TSource, TResult>> expression,
            Compare operation = Compare.GreaterThan)
        {
            _resultValidation.AddValidation();
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            (var type, var prop, var value) = GetValues(source, expression);

            var isValid = false;
            if (value != null)
            {
#pragma warning disable CA1305 // Specify IFormatProvider
                switch (System.Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.String:
                        isValid = !string.IsNullOrEmpty(value as string);
                        break;
                    case TypeCode.Double:
                        isValid = IsTrue(Convert.ToDouble(value), operation, 0d);
                        break;
                    case TypeCode.DateTime:
                        isValid = IsTrue(Convert.ToDateTime(value), operation, DateTime.UtcNow);
                        break;
                    case TypeCode.Object:
                        isValid = ValidateObject(value);
                        break;
                }
#pragma warning restore CA1305 // Specify IFormatProvider
            }

            if (!isValid)
                await AddErroMessage(type, prop).ConfigureAwait(false);
        }

        private async Task AddErroMessage(string type, string prop) =>
            _resultValidation.AddErrorMessage(await _errorMessages.GetByTagAsync($"{type}:{prop}").ConfigureAwait(false));

        private static bool ValidateObject(object value)
        {
            switch (value.GetType().ToString())
            {
                case "System.Guid":
                    return !string.IsNullOrEmpty(value.ToString());
            }

            return false;

        }

        private static (string type, string memberName, TResult value) GetValues<TSource, TResult>(TSource source, Expression<Func<TSource, TResult>> expression)
        {
            var type = typeof(TSource);
            var memberExpr = (MemberExpression)expression.Body;
            var compiledDelegate = expression.Compile();
            var value = compiledDelegate(source);

            return (type.FullName, memberExpr.Member.Name, value);
        }

        public static bool IsTrue<TValue1, TValue2>(TValue1 value1, Compare comparisonOperator, TValue2 value2)
                where TValue1 : TValue2
                where TValue2 : IComparable
        {
            switch (comparisonOperator)
            {
                case Compare.GreaterThan:
                    return value1.CompareTo(value2) > 0;
                case Compare.GreaterThanOrEqual:
                    return value1.CompareTo(value2) >= 0;
                case Compare.LessThan:
                    return value1.CompareTo(value2) < 0;
                case Compare.LessThanOrEqual:
                    return value1.CompareTo(value2) <= 0;
                case Compare.Equal:
                    return value1.CompareTo(value2) == 0;
                default:
                    return false;
            }
        }

        public enum Compare
        {
            GreaterThan = 1,
            GreaterThanOrEqual = 2,
            LessThan = 3,
            LessThanOrEqual = 4,
            Equal = 5
        }
    }
}
