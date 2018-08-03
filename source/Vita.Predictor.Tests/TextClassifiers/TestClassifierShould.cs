using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Azure.Search.Common;
using Vita.Contracts;
using Vita.Contracts.SubCategories;
using Vita.Domain.Infrastructure;
using Vita.Predictor.TextClassifiers;
using Xunit;

namespace Vita.Predictor.Tests.TextClassifiers
{
  [Collection("DataCollection")]
  public class TestClassifierShould
  {
    private readonly DataFixture _dataFixture;
    private readonly TextClassifier _analyser;


    public TestClassifierShould(DataFixture dataFixture)
    {
      _dataFixture = dataFixture;
      _dataFixture.Init();

      _analyser =
        new TextClassifier(_dataFixture.Companies, _dataFixture.Localities, _dataFixture.Classifiers)
        {
          UseCache = false
        };
      // _analyser.FlushCache();
    }


    [Theory]
    [InlineData("Duncraig red rooster", true, DataFixture.CompanyNames.RedRooster)]
    [InlineData("Gymea Crust Pizza", true, DataFixture.CompanyNames.CrustPizza)]
    [InlineData("XXXXXXXXXXXXXXXXXXXXXX", false, null)]
    [InlineData("EFTPOS TOBACCO STATION KARI KARINGAL VIC", true, "CompanyName1")]
    [InlineData("EFTPOS DEBIT EFTPOS 18/11 11:30 COLES 4419", true, DataFixture.CompanyNames.Coles)]
    [InlineData("coles 6018", true, DataFixture.CompanyNames.Coles)]
    public async Task Classify_who(string sentence, bool companyExists, string companyName = null)
    {
      var result = await _analyser.Match(sentence);

      //who - what company,bank etc
      if (companyExists)
      {
        result.Company.Should().NotBe(null);
        result.Company.CompanyName.Should().Be(companyName);
      }
      else
      {
        result.Company.Should().BeNull();
      }
    }


    [Theory]
    [InlineData("Duncraig red rooster", TransactionType.Unknown)]
    [InlineData("Gymea Crust Pizza", TransactionType.Unknown)]
    [InlineData("ASDFASDFSS", TransactionType.Unknown)]
    [InlineData("EFTPOS TOBACCO STATION KARI KARINGAL VIC", TransactionType.Debit)]
    [InlineData("TRANSFER 18/11 11:30 COLES 4419", TransactionType.Transfer)]
    [InlineData("card reversal", TransactionType.Reversal)]
    public async Task Classify_what(string sentence, TransactionType? tt)
    {
      var result = await _analyser.Match(sentence);
      result.TransactionType.Should().Be(tt);
    }

    [Theory]
    [InlineData("26 April 2017", "2017/04/26")]
    //[InlineData("EFTPOS DEBIT EFTPOS 08/12 14:20 MASTERS 7563", "2017-12-08 14:20:00")]
    [InlineData("PLEASE NOTE FROM 18 SEP 2015 YOUR DEBIT INT RATE IS 15.76%", "2015-09-18 00:00:00")]
    [InlineData("MISCELLANEOUS DEBIT V6870 24/10 VIDEO EZY EXPRESS SHEPPARTON 74564455299", "2017-10-24")]
    public async Task Classify_when(string sentence, string date)
    {
      var result = await _analyser.Match(sentence);

      DateTime? dt = DateTime.Parse(date, CultureInfo.GetCultureInfo("en-AU"));

      //when
      result.TransactionDate.Should().Be(dt);
    }

    [Theory]
    [InlineData("Duncraig red rooster", AustralianState.WA, "DUNCRAIG", "6023")]
    [InlineData("Gymea Crust Pizza", AustralianState.NSW, "GYMEA", "2227")]
    [InlineData("ASDFASDFSS", null, null, null)]
    [InlineData("EFTPOS TOBACCO STATION KARI KARINGAL VIC", AustralianState.VIC, "KARINGAL", "3199")]
    [InlineData("EFTPOS DEBIT 18/11 11:30 COLES 4419", AustralianState.SA, "COLES",
      "5272")] //https://en.wikipedia.org/wiki/Coles,_South_Australia
    public async Task Classify_where(string sentence, AustralianState? australianState, string suburb, string postCode)
    {
      var results = await _analyser.Match(sentence);
      //where did it take place
      results.Locality.AustralianState.Should().Be(australianState);
      results.Locality.Suburb.Should().Be(suburb);
      results.Locality.Postcode.Should().Be(postCode);
      //results.Any(x => x.AustralianState == australianState).Should().BeTrue();
      //results.Any(x => x.Suburb == suburb).Should().BeTrue();
      //results.Any(x => x.Postcode == postCode).Should().BeTrue();
    }


    [Theory]
    [InlineData("xxxx", PaymentMethodType.Unknown)]
    [InlineData("eftpos", PaymentMethodType.Eftpos)]
    [InlineData("withdraw", PaymentMethodType.CashWithdrawl)]
    [InlineData("advance", PaymentMethodType.CashWithdrawl)]
    public async Task Classify_how(string sentence, PaymentMethodType pmt)
    {
      var result = await _analyser.Match(sentence);

      result.PaymentMethodType.Should().Be(pmt);
    }


    [Theory]
    [InlineData("xxxx", PaymentMethodType.Unknown)]
    [InlineData("eftpos", PaymentMethodType.Eftpos)]
    [InlineData("withdraw", PaymentMethodType.CashWithdrawl)]
    [InlineData("advance", PaymentMethodType.CashWithdrawl)]
    public async Task Classify_by_keyword(string sentence, PaymentMethodType pmt)
    {
      var result = await _analyser.Match(sentence);

      result.PaymentMethodType.Should().Be(pmt);
    }

    [Theory]
    [InlineData("Periodical Payment To Mc To Masterca", CategoryType.BankingFinance,
      Categories.BankingFinance.CreditCardPayments)]
    [InlineData("Kidz", CategoryType.Kids, Categories.Kids.Childcare)]
    [InlineData("Liquorland North Perth Aus", CategoryType.Groceries, Categories.Groceries.LiquorStores)]
    [InlineData("St John Of God", CategoryType.HealthBeauty, Categories.HealthBeauty.DoctorsDentist)]
    public async Task Classify_why(string sentence, CategoryType ct, string sub)
    {
      var results = await _analyser.MatchMany(sentence);

      var cats = results.Select(x => x.Classifier.CategoryType);
      cats.Should().Contain(ct, cats.ToCommaSeparatedString());

      var subs = results.Select(x => x.Classifier.SubCategory);
      subs.Should().Contain(sub, subs.ToCommaSeparatedString());
    }

    [Fact]
    public async Task Classify_why_once()
    {
      var sentence = "St John of God";
      var ct = CategoryType.HealthBeauty;
      var sub = Categories.HealthBeauty.DoctorsDentist;

      var result = await _analyser.Match(sentence);

      _analyser.ClassifierCache.Count().Should().NotBe(0);
      _analyser.LocalityCache.Count().Should().NotBe(0);

      result.Classifier.Should().NotBe(null);
      result.Classifier.CategoryType.Should().Be(ct, result.Classifier.CategoryType.ToString());
      result.Classifier.SubCategory.Should().Be(sub);
    }

    [Theory]
    [InlineData(4, 1, "hello today is nice")]
    [InlineData(2, 3, "hello today is nice")]
    [InlineData(2, 1, "hello ^&*( today")]
    [InlineData(2, 1, "hello 10/11 today")]
    public void Create_ngrams(int ngram, int count, string sentence)
    {
      var result = _analyser.CreateNgrams(sentence);
      result[ngram].Count().Should().Be(count, string.Join(" ", result.Values.Select(x => x.ToList())));
    }

    [Fact]
    public void Classify_contains_test()
    {
      _dataFixture.Classifiers.GetAll().Select(a => a.Keywords.Contains("st john of god")).Count().Should()
        .BeGreaterThan(0);
    }

    [Fact]
    public async Task Predict()
    {
      string contents = File.ReadAllText(PredictionModelWrapper.GetFilePath(@"test.tsv"));
      var data = FileUtil.Read(contents);
      var plainClassifiers = new List<KeyValuePair<string, TextClassificationResult>>();

      foreach (var item in data)
      {
        var result = await _analyser.Match(item.Description);
        if (result.Classifier != null)
        {
          var kvp = new KeyValuePair<string, TextClassificationResult>(item.SubCategory, result);
          plainClassifiers.Add(kvp);
        }
      }

      double correct = (double)plainClassifiers.Count(x => x.Key == x.Value.Classifier.SubCategory);
      double over = (double)plainClassifiers.Count;
      double percentage=(correct/over);
      Console.WriteLine(percentage.ToString("P5"));

      percentage.Should().BeGreaterThan(.3,percentage.ToString("P5"));
    }
  }
}