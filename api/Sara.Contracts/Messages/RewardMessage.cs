using Newtonsoft.Json;

namespace Sara.Contracts.Messages
{
    public class RewardMessage
    {
        protected RewardMessage() { }

        public RewardMessage(string customer, string logo, double reward)
        {
            Customer = customer;
            Logo = logo;
            Reward = $"BRE$ {reward:0.00}";
        }

        [JsonProperty("cs")]
        public string Customer { get; set; }

        [JsonProperty("lo")]
        public string Logo { get; set; }

        [JsonProperty("re")]
        public string Reward { get; set; }
    }
}
