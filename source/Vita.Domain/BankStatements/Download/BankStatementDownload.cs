using System;
using Newtonsoft.Json;
using Vita.Domain.BankStatements.Login;
using Vita.Domain.BankStatements.Models;

namespace Vita.Domain.BankStatements.Download
{
    public class BankStatementDownload
    {
        public string ResponseJson { get; }
        public FetchAllResponse FetchAllResponse => TryToDeserialize();
        public BankStatementResponseType ResponseType { get; }
        public BankStatementError Error { get; }

        public BankStatementDownload(string responseJson)
        {
            ResponseJson = responseJson;
            ResponseType = BankStatementResponseType.Downloaded;
        }

        private FetchAllResponse TryToDeserialize()
        {
            //BS response xml structure is not the same across banks, so it is not safe to deserialize to an .NET object
            //TODO: using query model/helper to get data in need to deal with the schema change
            try
            {
              return JsonConvert.DeserializeObject<Vita.Domain.BankStatements.Models.FetchAllResponse>(ResponseJson);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Cannot deserialize json data: " + ResponseJson, ex);
            }
        }

        private BankStatementDownload(BankStatementResponseType errorType, BankStatementError error)
        {
            if (errorType == BankStatementResponseType.None ||
                errorType == BankStatementResponseType.LoginSuccess ||
                errorType == BankStatementResponseType.Downloaded)
            {
                throw new InvalidOperationException($"Cannot create errored DownloadResponse with response type {errorType}");
            }
            ResponseType = errorType;
            Error = error ?? throw new ArgumentNullException(nameof(error));
        }

        public static BankStatementDownload Errored(BankStatementError error)
        {
            var errorType = error.IsInvalidLogin()
                ? BankStatementResponseType.InvalidLogin
                : BankStatementResponseType.Failed;
            return new BankStatementDownload(errorType, error);
        }
    }
}
