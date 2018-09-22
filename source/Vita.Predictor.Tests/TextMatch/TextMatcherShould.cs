using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Azure.Search.Common;
using Vita.Contracts;

using Vita.Domain.Infrastructure;
using Xunit;

namespace Vita.Predictor.Tests.TextMatch
{
    [Collection("DataCollection")]
    public class TextMatcherShould : MatchShouldBase
    {
        public TextMatcherShould(DataFixture dataFixture) : base(dataFixture)
        {
        }       

        [Theory]
        [InlineData("xxxx", PaymentMethodType.Unknown)]
        [InlineData("eftpos", PaymentMethodType.Eftpos)]
        [InlineData("withdraw", PaymentMethodType.CashWithdrawl)]
        [InlineData("advance", PaymentMethodType.CashWithdrawl)]
        public async Task Match_by_keyword(string sentence, PaymentMethodType pmt)
        {
            var result = await Matcher.Match(sentence,false);

            result.PaymentMethodType.Should().Be(pmt);
        }

        [Theory]
        [InlineData("Periodical Payment To Mc To Masterca", CategoryType.TransferringMoney, SubCategories.TransferringMoney.OtherTransferringMoney)]
        [InlineData("Kidz", CategoryType.Kids, SubCategories.Kids.Childcare)]
        [InlineData("Liquorland", CategoryType.Groceries, SubCategories.Groceries.LiquorStores)]
        [InlineData("St John Of God", CategoryType.HealthBeauty, SubCategories.HealthBeauty.DoctorsDentist)]
        [InlineData("minimart", CategoryType.Groceries, SubCategories.Groceries.OtherGroceries)]
        public async Task MatchMany_why(string sentence, CategoryType ct, string sub)
        {
            var results = await Matcher.MatchMany(sentence);

            var textClassificationResults = results as TextClassificationResult[] ?? results.ToArray();

            var cats = textClassificationResults.Select(x => x.Classifier.CategoryType);
            cats.Should().Contain(ct);

            var subs = textClassificationResults.Select(x => x.Classifier.SubCategory);
            var enumerable = subs as string[] ?? subs.ToArray();
            Console.WriteLine(string.Join(',', enumerable.ToArray()));
            enumerable.Should().Contain(sub);
        }

        [Fact]
        public async Task Match_why_once()
        {
            var sentence = "St John of God";
            var ct = CategoryType.HealthBeauty;
            var sub = SubCategories.HealthBeauty.DoctorsDentist;

            var result = await Matcher.Match(sentence,true);

            Matcher.ClassifierCache.Count().Should().NotBe(0);
            Matcher.LocalityCache.Count().Should().NotBe(0);

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
             Matcher.CreateNgrams(sentence);
            Matcher.Ngrams[ngram].Count().Should().Be(count, string.Join(" ", Matcher.Ngrams.Values.Select(x => x.ToList())));
        }

        [Fact]
        public void Classify_contains_test()
        {
            DataFixture.Classifiers.GetAll().Select(a => a.Keywords.Contains("st john of god")).Count().Should()
                .BeGreaterThan(0);
        }

        [Fact]
        public async Task Predict()
        {
            var contents = File.ReadAllText(PredictionModelWrapper.GetFilePath(@"test.csv"));
            var data = FileUtil.Read(contents);
            var plainClassifiers = new List<KeyValuePair<string, TextClassificationResult>>();

            foreach (var item in data)
            {
                var result = await Matcher.Match(item.Description);
                if (result.Classifier != null)
                {
                    var kvp = new KeyValuePair<string, TextClassificationResult>(item.SubCategory, result);
                    plainClassifiers.Add(kvp);
                }
            }

            var correct = plainClassifiers.Count(x => x.Key == x.Value.Classifier.SubCategory);
            var over = (double) plainClassifiers.Count;
            var percentage = correct / over;
            Console.WriteLine(percentage.ToString("P5"));

            percentage.Should().BeGreaterThan(.3, percentage.ToString("P5"));
        }

        
    }
}