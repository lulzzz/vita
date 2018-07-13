using Newtonsoft.Json;

namespace Vita.Domain.BankStatements.Models
{
    public class Credential
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fieldId")]
        public string FieldId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("values")]
        public object Values { get; set; }

        [JsonProperty("keyboardType")]
        public string KeyboardType { get; set; }

        [JsonProperty("optional")]
        public bool? Optional { get; set; }
    }
}
