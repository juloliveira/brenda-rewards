using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brenda.Utils
{
    public class ReCaptcha
    {
        public static async Task<ReCaptcha> Validate(string key, string token)
        {
            using var client = new System.Net.WebClient();
            var response = await client.DownloadStringTaskAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={key}&response={token}");

            var captchaResponse = JsonConvert.DeserializeObject<ReCaptcha>(response);

            return captchaResponse;
        }

        [JsonProperty("success")]
        public bool SuccessResult { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTime? ChallengedAt { get; set; }

        [JsonProperty("apk_package_name")]
        public string PackageName { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonIgnore]
        public bool Success =>
            SuccessResult &&
            ChallengedAt < DateTime.UtcNow.AddMinutes(3);

    }
}
