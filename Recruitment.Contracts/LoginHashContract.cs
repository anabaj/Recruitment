using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Recruitment.Contracts
{
    public class LoginHashContract
    {
        [JsonProperty(PropertyName = "hash_value")]
        [JsonPropertyName("hash_value")]
        public string HashValue { get; set; }
    }
}
