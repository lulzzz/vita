using System.Collections.Generic;
using Vita.Contracts;
using Vita.Domain.BankStatements.Download;

namespace Vita.Domain.BankStatements
{
    public static class BankStatementExtensions
    {
        public static IEnumerable<PredictionRequest> ToPredictionRequests(this BankStatementDownload download)
        {
            if (download == null) return null;
            if (download.Error != null) return null;

            var list = new List<PredictionRequest>();

            foreach (var account in download.FetchAllResponse.Accounts)
            {
                foreach (var item in account.StatementData.Details)
                {
                    var pr = new PredictionRequest
                    {
                        Description = item.Text,
                        AccountType = AccountTypeConverter.Convert(account.AccountType),
                        Amount = item.Amount,
                        Bank = account.Institution,
                        TransactionUtcDate = item.DateObj.Date.UtcDateTime
                    };
                    list.Add(pr);
                }                
            }

            return list;
        }
    }
}