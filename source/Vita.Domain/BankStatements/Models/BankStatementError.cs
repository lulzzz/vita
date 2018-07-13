namespace Vita.Domain.BankStatements.Models
{
    public class BankStatementError
    {
    
        public string Error { get; set; }
    
        public string UserToken { get; set; }
    
        public string ReferralCode { get; set; }

        public bool IsInvalidLogin()
        {
            return Error.Contains("Login Failed") || Error.Contains("login credentials");
        }
    }
}
