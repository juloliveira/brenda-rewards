using Brenda.Utils;
using Newtonsoft.Json;
using System;

namespace Brenda.Contracts.V1.Campaign
{
    public class ChallengeOnGoing
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("ti")]
        public string Title { get; set; }

        [JsonProperty("ac")]
        public string Action { get; set; }

        [JsonProperty("ci")]
        public Guid CustomerId { get; set; }

        [JsonProperty("cn")]
        public string CustomerName { get; set; }

        [JsonProperty("br")]
        public string Brand { get; set; }

        [JsonProperty("re")]
        public double Reward { get; set; }

        [JsonProperty("ba")]
        public double Balance { get; set; }

        [JsonProperty("rs")]
        public string Resource { get; set; }

        [JsonProperty("vs")]
        public long ValidationStart { get; set; }

        [JsonProperty("ve")]
        public long ValidationEnd { get; set; }

        [JsonProperty("res")]
        public CampaignRestriction[] Restrictions { get; set; }


        [JsonProperty("ca")]
        public CampaignOnGoing[] Campaigns { get; set; }

    }


    public class CampaignOnGoing
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("ti")]
        public string Title { get; set; }

        [JsonProperty("ac")]
        public string Action { get; set; }

        [JsonProperty("ci")]
        public Guid CustomerId { get; set; }

        [JsonProperty("cn")]
        public string CustomerName { get; set; }

        [JsonProperty("br")]
        public string Brand { get; set; }

        [JsonProperty("re")]
        public double Reward { get; set; }

        [JsonProperty("ba")]
        public double Balance { get; set; }

        [JsonProperty("rs")]
        public string Resource { get; set; }

        [JsonProperty("vs")]
        public long ValidationStart { get; set; }

        [JsonProperty("ve")]
        public long ValidationEnd { get; set; }

        [JsonProperty("res")]
        public CampaignRestriction[] Restrictions { get; set; }

        [JsonProperty("quiz")]
        public CampaignQuiz[] AssetQuestions { get; set; }

        public bool IsGeoAllowed(double latitude, double longitude)
        {
            if (Restrictions == null || Restrictions.Length == 0)
                return true;

            foreach (var restriction in Restrictions)
            {
                var distance = restriction.DistanceTo(latitude, longitude);
                if (distance < restriction.Radius) return true;
            }

            return false;
        }

        public bool IsOutOfDate()
        {
            var validateStart = DateTimeExtentions.FromTimestamp(ValidationStart);
            var validateEnd = DateTimeExtentions.FromTimestamp(ValidationEnd);

            return validateStart > DateTime.UtcNow || validateEnd < DateTime.UtcNow;
        }

        public bool ShouldSerializeAssetQuestions() => AssetQuestions != null && AssetQuestions.Length > 0;

        public override bool Equals(object obj)
        {
            var item = obj as CampaignOnGoing;

            return item != null && this.Id == item.Id;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode();
        }
    }

    public class CampaignQuiz
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("ds")]
        public string Description { get; set; }

        [JsonProperty("or")]
        public int Order { get; set; }

        [JsonProperty("op")]
        public CampaignQuiz[] Options { get; set; }

        public bool ShouldSerializeOptions() => Options != null && Options.Length > 0;
    }
}
