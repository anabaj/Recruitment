using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
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
