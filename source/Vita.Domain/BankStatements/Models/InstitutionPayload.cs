using Newtonsoft.Json;

namespace Vita.Domain.BankStatements.Models
{
    public class InstitutionPayload
    {
        [JsonProperty("institutions")]
        public Institution[] Institutions { get; set; }

        [JsonProperty("user_token")]
        public string UserToken { get; set; }

    }
}
