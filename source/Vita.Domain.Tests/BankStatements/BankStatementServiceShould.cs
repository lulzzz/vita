using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using FluentAssertions;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Vita.Domain.BankStatements;
using Vita.Domain.BankStatements.Login;
using Vita.Domain.BankStatements.Models;
using Vita.Domain.Infrastructure;
using Xunit;

namespace Vita.Domain.Tests.BankStatements
{
  public class BankStatementServiceShould
  {
    private static readonly IBankStatementService Service =
      new BankStatementService(new BankStatementsConfiguration(Mode.Test.ApiUrl, Mode.Test.ApiKey, Mode.Test.Prefix));

    public static BankLogin BsLogin =>
      new BankLogin("bank_of_statements", "username", "12345678", "password", "TestMyMoney");



    public static BankLogin AnzLogin => new BankLogin("anz", "username", SecretMan.Get("bankstatements-anz-test-username") , "password",SecretMan.Get("bankstatements-anz-test-password"));

    [Fact]
    public async Task Institution_will_download_banks()
    {
      var institutions = await Service.InstitutionsAsync();
      institutions.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_valid_will_return_login_success_response()
    {
      var loginResult = await Service.LoginAsync("test", BsLogin);
      loginResult.ResponseType.Should().Be(BankStatementResponseType.LoginSuccess);
      loginResult.LoginResponse.ReferralCode.Should().Be($"{Mode.Test.Prefix}-test");
      loginResult.Error.Should().BeNull();
    }

    [Fact]
    public async Task Login_Anz_will_return_correct_accounts()
    {
      var loginResult = await Service.LoginAsync("test-anz", AnzLogin);
      loginResult.ResponseType.Should().Be(BankStatementResponseType.LoginSuccess);
      loginResult.LoginResponse.ReferralCode.Should().Be($"{Mode.Test.Prefix}-test-anz");
      loginResult.Error.Should().BeNull();

      var summary = loginResult;
      summary.LoginResponse.Accounts.Count().Should().Be(2);
      summary.LoginResponse.Accounts.ShouldContainItem<Account>(x => x.Bsb == "", "1");
      summary.LoginResponse.Accounts.ShouldContainItem<Account>(x=>x.Name =="Credit Card", "2");
      summary.LoginResponse.Accounts.ShouldContainItem<Account>(x => x.Bsb == "016002", "3");
      summary.LoginResponse.Accounts.ShouldContainItem<Account>(x => x.AccountNumber == "401415303", "4");
      summary.LoginResponse.Accounts.ShouldContainItem<Account>(x => x.Name == "ANZ Access Advantage", "5");
    }

    [Fact]
    public async Task Login_invalid_will_return_invalid_login_response()
    {
      var bsLogin = new BankLogin("bank_of_statements", "username", "wrong", "password", "wrong");
      var loginResult = await Service.LoginAsync("test", bsLogin);
      loginResult.ResponseType.Should().Be(BankStatementResponseType.InvalidLogin);
      loginResult.Error.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_with_bad_bank_will_return_failed_response()
    {
      var bsLogin = new BankLogin("bad", "username", "wrong", "password", "wrong");
      var loginResult = await Service.LoginAsync("test", bsLogin);
      loginResult.ResponseType.Should().Be(BankStatementResponseType.Failed);
      loginResult.Error.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_fetch_all_return_full_statements()
    {
      var loginResult = await Service.LoginFetchAllAsync("test", BsLogin);
      loginResult.ResponseType.Should().Be(BankStatementResponseType.Downloaded);

      var json = loginResult.ResponseJson;
      json.Length.Should().NotBe(0);

    }

    [Fact]
    public async Task Download_file_async_return_files()
    {
      var useridentifier = "test-anz";
      var loginResult = await Service.LoginFetchAllAsync(useridentifier, AnzLogin);
      var files = await Service.DownloadFilesAsync(useridentifier, loginResult.FetchAllResponse.UserToken);
      files.Should().NotBeNull();
      files.Length.Should().BeGreaterThan(0);
    }
  }
}