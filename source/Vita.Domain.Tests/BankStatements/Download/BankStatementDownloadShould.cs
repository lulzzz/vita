using System;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using Vita.Domain.BankStatements.Download;
using Vita.Domain.BankStatements.Models;
using Vita.Domain.Tests.BankStatements.Models.Fixtures;
using Xunit;

namespace Vita.Domain.Tests.BankStatements.Download
{
  public class BankStatementDownloadShould
  {
    private readonly string RawJson = @"{
    'accounts': [{
        'name': 'Credit Card',
        'accountNumber': '4509 49xx xxxx 0220',
        'id': 0,
        'bsb': '',
        'balance': '-924.57',
        'available': '20922.20',
        'accountType': 'credit card'
    }, {
        'name': 'ANZ Access Advantage',
        'accountNumber': '401415303',
        'id': 1,
        'bsb': '016002',
        'balance': '542.48',
        'available': '515.14',
        'accountType': 'transaction'
    }],
    'user_token': 'IRqO8Gi8Zbx2Glht371_1w',
    'referral_code': 'FGF-test-anz'
}";

    [Fact]
    public void With_Unsupported_Json_Schema_Should_Be_Able_Get_Token()
    {
      var response = new BankStatementDownload(RawJson);
      response.FetchAllResponse.UserToken.Should().Be("IRqO8Gi8Zbx2Glht371_1w");
    }

   
    [Fact]
    public void With_Unsupported_Json_Schema_Deserialize_Throw_Exception()
    {
      var response = new BankStatementDownload("{wrong}");
      Assert.Throws<ArgumentException>(() =>
      {
        var t = response.FetchAllResponse;
      });
    }

    [Fact]
    public void Parse_accounts()
    {
      var json = BankStatementsFixture.Statement2;
      var response = new BankStatementDownload(json).FetchAllResponse;
      response.Accounts.Count.Should().Be(2);
      response.Accounts[0].Name.Should().Be("Credit Card");
      response.Accounts[0].AccountNumber.Should().Be("4509 49xx xxxx 0220");
      response.Accounts[0].Balance.Should().Be(-924.57);
      response.Accounts[0].Available.Should().Be(20922.20);
      response.Accounts[0].AccountType.Should().Be("credit card");
      response.Accounts[0].AccountHolder.Should().Be("MR Fred Flintstone");
      response.Accounts[0].Institution.Should().Be("ANZ");
    }

    [Fact]
    public void Parse_finds_transaction()
    {
      //{
      //  'dateObj': {
      //    'date': '2018-04-23 00:00:00.000000',
      //    'timezone_type': 3,
      //    'timezone': 'Australia/Sydney'
      //  },
      //  'date': '23-04-2018',
      //  'text': 'CATABY ROADHOUSE CATABY',
      //  'notes': null,
      //  'transactionHash': null,
      //  'hashText': null,
      //  'amount': 112.48,
      //  'type': 'Debit',
      //  'balance': '726.56',
      //  'tags': []
      //},
      var json = BankStatementsFixture.Statement2;
      var response = new BankStatementDownload(json).FetchAllResponse;
      const string text = @"CATABY ROADHOUSE CATABY";

      var acc = response.Accounts.FindAll(x => x.StatementData.Details.Any(y => y.Text == text));

      var result = acc.First().StatementData.Details.FirstOrDefault(x => x.Text == text);

      result.Text.Should().Be(text);
      result.Amount.Should().Be(112.48);
      result.Type.Should().Be(TypeEnum.Debit);
      result.Balance.Should().Be(726.56);
      result.Date.Should().Be(("23-04-2018"));
    }

  }
}