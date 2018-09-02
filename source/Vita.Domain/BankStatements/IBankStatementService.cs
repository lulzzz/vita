using System.Collections.Generic;
using System.Threading.Tasks;
using Vita.Contracts;
using Vita.Domain.BankStatements.Download;
using Vita.Domain.BankStatements.Login;
using Vita.Domain.BankStatements.Models;

namespace Vita.Domain.BankStatements
{
    public interface IBankStatementService
    {
        Task<IEnumerable<Institution>> InstitutionsAsync();

        Task<LoginResult> LoginAsync(string  userIdentifier, BankLogin bankLogin);

        Task<BankStatementDownload> LoginFetchAllAsync(string userIdentifier, BankLogin bankLogin);

        /// <summary>
        /// It is important to understand that this function does not cause any content to be retrieved from the bank, 
        /// but instead will only be able to return data that exists in the current temporary session as a result of a 
        /// successful request to the Retrieve Statement Data or Login Fetch All Statements API functions.
        /// </summary>
        /// <param name="userIdentifier"></param>
        /// <param name="userToken"></param>
        /// <returns></returns>
        Task<byte[]> DownloadFilesAsync(string userIdentifier, string userToken);
    }
}
