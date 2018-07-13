using Vita.Domain.BankStatements.Login;

namespace Vita.Domain.BankStatements.Download
{
    public class DownloadResult
    {
        public DownloadResult(BankStatementResponseType bankStatementResponseType, BankStatementDownload bankStatementDownload)
        {
            BankStatementResponseType = bankStatementResponseType;
            BankStatementDownload = bankStatementDownload;
        }

        public DownloadResult(BankStatementResponseType bankStatementResponseType, string error)
        {
            BankStatementResponseType = bankStatementResponseType;
            Error = error;
        }

        public BankStatementResponseType BankStatementResponseType { get; set; }
        public BankStatementDownload BankStatementDownload { get; set; }
        public string Error { get; private set; }
    }
}
