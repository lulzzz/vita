namespace Vita.Domain.BankStatements
{
    public class BankStatementsConfiguration : IBankStatementsConfiguration
    {
        public BankStatementsConfiguration(string apiUrl, string apiKey, string prefix)
        {
            ApiUrl = apiUrl;
            ApiKey = apiKey;
            Prefix = prefix;
        }

        public string ApiUrl { get; }
        public string ApiKey { get; }
        public string Prefix { get; }
    }
}
