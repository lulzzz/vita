namespace Vita.Domain.BankStatements.Login
{
    public enum BankStatementResponseType
    {
        None = 0,
        Failed,
        InvalidLogin,
        LoginSuccess,
        Downloaded
    }
}
