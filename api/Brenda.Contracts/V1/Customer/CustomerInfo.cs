using Newtonsoft.Json;
using System;

namespace Brenda.Contracts.V1.Customer
{
    public class CustomerInfo
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("na")]
        public string Name { get; set; }

        [JsonProperty("lo")]
        public string LogoAvatar { get; set; }
    }
}
