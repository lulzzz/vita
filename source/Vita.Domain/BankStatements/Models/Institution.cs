using System.Collections.Generic;
using Newtonsoft.Json;

namespace Vita.Domain.BankStatements.Models
{
    public class Institution
    {

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("credentials")]
        public IList<Credential> Credentials { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("searchable")]
        public string Searchable { get; set; }

        [JsonProperty("display")]
        public string Display { get; set; }

        [JsonProperty("searchVal")]
        public string SearchVal { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("export_with_password")]
        public string ExportWithPassword { get; set; }

        [JsonProperty("estatements_supported")]
        public string EstatementsSupported { get; set; }

        [JsonProperty("transaction_listings_supported")]
        public string TransactionListingsSupported { get; set; }

        [JsonProperty("requires_preload")]
        public string RequiresPreload { get; set; }

        [JsonProperty("requires_mfa")]
        public string RequiresMfa { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("max_days")]
        public string MaxDays { get; set; }
    }
}
