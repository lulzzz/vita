namespace Vita.Contracts
{
    public class BankStatementAuthToken : ValueObject
    {
        public string Token { get; set; }

        public BankStatementAuthToken(string token)
        {
            Token = token;
        }
    }
}