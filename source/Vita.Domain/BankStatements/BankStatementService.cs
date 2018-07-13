using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ExtensionMinder;
using Flurl.Http;
using Newtonsoft.Json;
using Polly;
using Vita.Domain.BankStatements.Download;
using Vita.Domain.BankStatements.Login;
using Vita.Domain.BankStatements.Models;
using Vita.Domain.BankStatements.Utility;
using Vita.Domain.Infrastructure;

namespace Vita.Domain.BankStatements
{
  public class BankStatementService : IBankStatementService
  {
    private readonly IBankStatementsConfiguration _configuration;

    public BankStatementService(IBankStatementsConfiguration bankStatementsConfiguration)
    {
      _configuration = bankStatementsConfiguration;
    }

    public async Task<IEnumerable<Institution>> InstitutionsAsync()
    {
      var insitutions = await Cacher
        .GetAsync(BankStatementsUtil.BankStatementsInstitutionsList, GetInstitutionPayloadAsync)
        .ConfigureAwait(false);

      return insitutions;
    }

    private async Task<IEnumerable<Institution>> GetInstitutionPayloadAsync()
    {
      var payload = await (_configuration.ApiUrl + "/institutions")
        .WithHeader("X-API-KEY", _configuration.ApiKey)
        .WithHeader("Accept", "application/json")
        .GetJsonAsync<InstitutionPayload>().ConfigureAwait(false);
      return payload?.Institutions;
    }

    private string CreateReferralCode(string userIdentifier)
    {
      return $"{_configuration.Prefix}-{userIdentifier}";
    }

    public async Task<LoginResult> LoginAsync(string userIdentifier, BankLogin bankLogin)
    {
      try
      {
        var credentials =
          BankStatementsUtil.FormatCredentials(bankLogin, await InstitutionsAsync().ConfigureAwait(false));

        var policy = Policy
          .Handle<FlurlHttpTimeoutException>()
          .RetryAsync(3,
            (exception, retryCount) =>
            {
              Trace.TraceWarning(exception.ToString(), "BankStatement login timed out {RetryCount} times", retryCount);
            });

        string refcode = CreateReferralCode(userIdentifier);

        var response = await policy.ExecuteAsync(() =>
          (_configuration.ApiUrl + "/login")
          .WithHeader("X-API-KEY", _configuration.ApiKey)
          .WithHeader("REFERRAL-CODE", refcode)
          .WithHeader("Accept", "application/json")
          .PostJsonAsync(new
          {
            silent = 0,
            async = 0,
            credentials
          }).ReceiveString()).ConfigureAwait(false);

        var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response);
        loginResponse.ReferralCode = refcode;
        return new LoginResult(BankStatementResponseType.LoginSuccess, loginResponse);
      }
      catch (FlurlHttpException ex)
      {
        Trace.TraceWarning("Failed to login to bank statements for " + userIdentifier);

        if (ex.Call.Response == null)
          throw;

        var response = await ex.Call?.Response.Content.ReadAsStringAsync();

        if (response == null)
          return new LoginResult(BankStatementResponseType.Failed, ex.GetInnerMostException()?.Message);

        var error = JsonConvert.DeserializeObject<BankStatementError>(response);
        if (error.IsInvalidLogin())
          return new LoginResult(
            BankStatementResponseType.InvalidLogin,
            error.Error);

        return new LoginResult(BankStatementResponseType.Failed, response);
      }
    }

    public async Task<BankStatementDownload> LoginFetchAllAsync(string userIdentifier, BankLogin bankLogin)
    {
      try
      {
        var credentials =
          BankStatementsUtil.FormatCredentials(bankLogin, await InstitutionsAsync().ConfigureAwait(false));

        var policy = Policy
          .Handle<FlurlHttpTimeoutException>()
          .RetryAsync(3,
            (exception, retryCount) =>
            {
              Trace.TraceWarning(exception.ToString(), "BankStatement download timed out {RetryCount} times",
                retryCount);
            });

        var response = await policy.ExecuteAsync(() =>
          (_configuration.ApiUrl + "/login_fetch_all")
          .WithHeader("X-API-KEY", _configuration.ApiKey)
          .WithHeader("REFERRAL-CODE", CreateReferralCode(userIdentifier))
          .WithHeader("Accept", "application/json")
          .PostJsonAsync(new
          {
            silent = 0,
            async = 0,
            credentials
          }).ReceiveString());

        return new BankStatementDownload(response);
      }
      catch (FlurlHttpException ex)
      {
        Debug.Write(ex, "Failed to download bank statement");

        if (ex.Call.Response == null)
          throw;

        var response = ex.Call.Exception.Message;

        if (response == null)
          return BankStatementDownload.Errored(new BankStatementError
          {
            Error = "Empty response from bs.com",
            ReferralCode = userIdentifier
          });

        var deserializer = new XmlSerializer(typeof(BankStatementError));
        var reader = new StringReader(response);
        var error = (BankStatementError) deserializer.Deserialize(reader);
        return BankStatementDownload.Errored(error);
      }
    }

    /// <summary>
    ///   It is important to understand that this function does not cause any content to be retrieved from the bank,
    ///   but instead will only be able to return data that exists in the current temporary session as a result of a
    ///   successful request to the Retrieve Statement Data or Login Fetch All Statements API functions.
    /// </summary>
    /// <param name="userIdentifier"></param>
    /// <param name="userToken"></param>
    /// <returns></returns>
    public async Task<byte[]> DownloadFilesAsync(string userIdentifier, string userToken)
    {
      var policy = Policy
        .Handle<FlurlHttpTimeoutException>()
        .RetryAsync(3,
          (exception, retryCount) =>
          {
            Trace.TraceWarning(exception.ToString(), "BankStatement download timed out {RetryCount} times", retryCount);
          });

      var response = await policy.ExecuteAsync(() =>
      {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Add("X-API-KEY", _configuration.ApiKey);
        client.DefaultRequestHeaders.Add("REFERRAL-CODE", CreateReferralCode(userIdentifier));
        client.DefaultRequestHeaders.Add("X-USER-TOKEN", userToken);

        //REMARK: Flurl does not support Content-Type header
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-zip"));
        return client.GetAsync($"{_configuration.ApiUrl}/files/");
      });

      if (response.StatusCode == HttpStatusCode.OK)
        using (var ms = new MemoryStream())
        {
          using (var contentStream = await response.Content.ReadAsStreamAsync())
          {
            await contentStream.CopyToAsync(ms);
            return ms.ToArray();
          }
        }

      return null;
    }
  }
}