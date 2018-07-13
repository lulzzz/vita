using Newtonsoft.Json;
using Vita.Domain.BankStatements.Models;

namespace Vita.Domain.BankStatements.Login
{
    public class LoginResult
    {
        [JsonConstructor]
        public LoginResult(BankStatementResponseType bankStatementResponseType, LoginResponse loginResponse)
        {
            ResponseType = bankStatementResponseType;
            LoginResponse = loginResponse;
        }

        public LoginResult(BankStatementResponseType bankStatementResponseType, string error)
        {
            ResponseType = bankStatementResponseType;
            Error = error;
        }

        public BankStatementResponseType ResponseType { get; }
        public LoginResponse LoginResponse { get; }
        public string Error { get; }
    }
}
