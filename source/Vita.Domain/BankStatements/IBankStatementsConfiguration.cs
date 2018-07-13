namespace Vita.Domain.BankStatements
{
    public interface IBankStatementsConfiguration
    {
        string ApiUrl { get; }
        string ApiKey { get; }
        string Prefix { get; }
    }
}
