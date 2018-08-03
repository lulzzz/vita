using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Vita.Domain.BankStatements.Download;
using Vita.Domain.Infrastructure;
using Vita.Domain.Tests.BankStatements.Models.Fixtures;
using Vita.Predictor;
using Xunit;
using ExtensionMinder;

namespace Vita.Domain.Tests.BankStatements.Tsv
{
    public class FileUtilShould

    {
        public static string Data = PredictionModelWrapper.GetFilePath("data-sample.csv", false);
        public static string Train = PredictionModelWrapper.GetFilePath("train.csv", false,false);
        public static string Test = PredictionModelWrapper.GetFilePath("test.csv",false,false);

        [Fact]
        public void Create_list_of_bank_statement_list_item_from_fetch_all_response()
        {
            var json = BankStatementsFixture.Statement2;
            var response = new BankStatementDownload(json).FetchAllResponse;

            var text = FileUtil.Create(response);

            text.Length.Should().BeGreaterThan(0);

            var file = Path.Combine(Path.GetTempPath(), $"bankstatements-{Guid.NewGuid()}.tsv");

            File.WriteAllText(file, text);

            Console.WriteLine(file);

            var result = FileUtil.Read(File.ReadAllText(file));
            var data = response.Accounts.SelectMany(x => x.StatementData.Details);
            result.Count().Should().Be(data.Count());
            File.Delete(file);
        }

        [Fact]
        public void Makes_test_and_training_data()
        {
            var data = Vita.Domain.Infrastructure.Importers.PocketBookImporter.Import(Data);

            int percent = (int)(data.Count() * .8);

            data = data.Shuffle();

            var train = data.Take(percent);
            var test = data.Except(train);

            var train1 = FileUtil.Create(train);
            File.WriteAllText(Train, train1);

            var test1 = FileUtil.Create(test);
            File.WriteAllText(Test, test1);

            var traintsv = FileUtil.Read(train1);
    
        }
    }
}