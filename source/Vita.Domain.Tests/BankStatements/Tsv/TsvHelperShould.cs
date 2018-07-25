using System;
using System.IO;
using System.Linq;
using ExtensionMinder;
using FluentAssertions;
using Vita.Domain.BankStatements.Download;
using Vita.Domain.BankStatements.Tsv;
using Vita.Domain.Infrastructure.Importers;
using Vita.Domain.Tests.BankStatements.Models.Fixtures;
using Vita.Predictor;
using Xunit;

namespace Vita.Domain.Tests.BankStatements.Tsv
{
    public class TsvHelperShould

    {
        public static string Data = PredictorSettings.GetFilePath("data-sample.csv", false);
        public static string Train = PredictorSettings.GetFilePath("train.tsv", false,false);
        public static string Test = PredictorSettings.GetFilePath("test.tsv",false,false);


        [Fact(Skip = "run on demand")]
        //[Fact]
        public void Create_cross_validation_data()
        {
            var data = PocketBookImporter.Import(Data);

            int percent = (int)(data.Count() * .8);

            data = data.Shuffle();
                
            var train = data.Take(percent);
            var test = data.Except(train);

            var train1 = TsvHelper.Create(train);
            File.WriteAllText(Train, train1);

            var test1 = TsvHelper.Create(test);
            File.WriteAllText(Test, test1);

            var traintsv = TsvHelper.Read(train1);
            traintsv.Count().Should().Be(train.Count());

            var testtsv = TsvHelper.Read(test1);
            testtsv.Count().Should().Be(test.Count());
        }


        [Fact]
        public void Create_list_of_bank_statement_list_item_from_fetch_all_response()
        {
            var json = BankStatementsFixture.Statement2;
            var response = new BankStatementDownload(json).FetchAllResponse;

            var text = TsvHelper.Create(response);

            text.Length.Should().BeGreaterThan(0);

            var file = Path.Combine(Path.GetTempPath(), $"bankstatements-{Guid.NewGuid()}.tsv");

            File.WriteAllText(file, text);

            Console.WriteLine(file);

            var result = TsvHelper.Read(File.ReadAllText(file));
            var data = response.Accounts.SelectMany(x => x.StatementData.Details);
            result.Count().Should().Be(data.Count());
            File.Delete(file);
        }
    }
}