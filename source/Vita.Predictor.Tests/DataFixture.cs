using System.Linq;
using FizzWare.NBuilder;
using Vita.Contracts;

using Vita.Domain.BankStatements.Download;
using Vita.Domain.BankStatements.Models;
using Vita.Domain.Infrastructure;
using Vita.Domain.Tests;
using Vita.Domain.Tests.BankStatements.Models.Fixtures;
using Vita.Predictor.SpreadSheets;
using Xunit;

namespace Vita.Predictor.Tests
{
    public class DataFixture
    {
        public IRepository<Locality> Localities { get; set; }
        public IRepository<Classifier> Classifiers { get; set; }
        public IRepository<Company> Companies { get; set; }

        public static class CompanyNames
        {
            public const string RedRooster = "RED ROOSTER PTY LTD";
            public const string CrustPizza = "CRUST PIZZA PTY LTD";
            public const string Coles = "COLES PTY LTD";
            public const string StJohnOfGod = "St John of God";
            public const string Kidz = "kidz";
        }

        public void Init(bool seed = true)
        {
            Localities = new Repository<Locality>();
            Classifiers = new FakeRepository<Classifier>();
            Companies = new FakeRepository<Company>();

            if (seed) BuildTestData();
        }

        public Account GetTestBankAccount()
        {
            return Cacher.GetOrSet("testaccount-bankstatement2", () =>
            {
                var json = BankStatementsFixture.Statement2;
                var response = new BankStatementDownload(json).FetchAllResponse;
                var account = response.Accounts.First();
                return account;
            });
        }

        private void BuildTestData()
        {
            BuildCompanies();

            BuildLocalities();

            LoadKeywordClassifiers();
        }


        private void LoadKeywordClassifiers()
        {
            var data = new KeywordsSpreadsheet().LoadData().ToList();
            data.ForEach(x => Classifiers.Save(x));

            data.Single(x => x.SubCategory == SubCategories.HealthBeauty.DoctorsDentist)
                .AddKeyword(CompanyNames.StJohnOfGod.ToLowerInvariant());
            data.Single(x => x.SubCategory == SubCategories.Kids.Childcare)
                .AddKeyword(CompanyNames.Kidz.ToLowerInvariant());
        }

        private void BuildCompanies()
        {
            var list = Builder<Company>.CreateListOfSize(5).Build();
            list.Add(Builder<Company>.CreateNew().With(x => x.CompanyName, CompanyNames.RedRooster).Build());
            list.Add(Builder<Company>.CreateNew().With(x => x.CompanyName, CompanyNames.CrustPizza).Build());
            list.Add(Builder<Company>.CreateNew().With(x => x.CompanyName, CompanyNames.Coles).Build());
            list.Add(Builder<Company>.CreateNew().With(x => x.CurrentName, "tobacco station").Build());
            list.Add(Builder<Company>.CreateNew().With(x => x.CurrentName, "syncoles").Build());
            list.Add(Builder<Company>.CreateNew().With(x => x.CurrentName, CompanyNames.Kidz).Build());
            list.Add(Builder<Company>.CreateNew().With(x => x.CompanyName, CompanyNames.Coles).Build());

            foreach (var c in list) Companies.Save(c);
        }

        private void BuildLocalities()
        {
            var list = Builder<Locality>.CreateListOfSize(5).Build();
            list.Add(Builder<Locality>.CreateNew().With(x => x.Suburb, "DUNCRAIG")
                .With(x => x.AustralianState, AustralianState.WA).With(x => x.Postcode, "6023").Build());
            list.Add(Builder<Locality>.CreateNew().With(x => x.Suburb, "GYMEA")
                .With(x => x.AustralianState, AustralianState.NSW).With(x => x.Postcode, "2227").Build());
            list.Add(Builder<Locality>.CreateNew().With(x => x.Suburb, "KARINGAL")
                .With(x => x.AustralianState, AustralianState.VIC).With(x => x.Postcode, "3199").Build());
            list.Add(Builder<Locality>.CreateNew().With(x => x.Suburb, "Toowoomba")
                .With(x => x.AustralianState, AustralianState.QLD).With(x => x.Postcode, "4419").Build());

            foreach (var locality in list) Localities.Save(locality);
        }
    }

    [CollectionDefinition("DataCollection")]
    public class DataCollection : ICollectionFixture<DataFixture>
    {
    }
}