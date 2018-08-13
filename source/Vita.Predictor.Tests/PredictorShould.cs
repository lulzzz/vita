using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Vita.Contracts;
using Vita.Contracts.SubCategories;
using Vita.Domain.BankStatements.Download;
using Vita.Domain.Infrastructure;
using Vita.Domain.Tests.BankStatements.Models.Fixtures;
using Xunit;

namespace Vita.Predictor.Tests
{
    public class PredictorShould
    {
        public static string Data = PredictionModelWrapper.GetFilePath("data-sample.csv");
        public static string Train = PredictionModelWrapper.GetFilePath("train.csv");
        public static string Test = PredictionModelWrapper.GetFilePath("test.csv");

        private readonly IPredict _predict;

        public PredictorShould()
        {
            _predict = new Predict();
        }

       // [Fact(Skip = "run on demand")]
        [Fact]
        public async Task Train_model()
        {
            var modelpath = await _predict.TrainAsync(Train);
            modelpath.Length.Should().NotBe(0);
            File.Exists(modelpath).Should().Be(true);
            Console.WriteLine(modelpath);
        }

        [Fact]
        public async Task Predict_test()
        {
            // run through the TEST file and predict each one
            var contents = File.ReadAllText(Test);
            var list = new List<KeyValuePair<string, string>>();
            var data = FileUtil.Read(contents);

            foreach (var item in data)
            {
                var request = new PredictionRequest()
                {
                    Description = item.Description,
                    Amount = item.Amount,
                    //Tags = item.Tags,
                    //Notes = item.Notes,
                    Bank = item.Bank,
                    //AccountType = item.AccountType,
                    //TransactionUtcDate = item.TransactionUtcDate
                };
                var result = await new Predict().PredictAsync(request);
                list.Add(new KeyValuePair<string, string>(item.SubCategory, result));
            }

            // whats our total correct?
            var correct = list.Count(x => x.Key == x.Value);
            var over = (double) list.Count;
            var percentage = correct / over;
            Console.WriteLine(percentage.ToString("P5"));

            percentage.Should().BeGreaterThan(.9);
        }

        [Theory]
        [InlineData("EFTPOS ALDI 27 CARRUM DOWNS AU","ANZ",Categories.Groceries.Supermarkets)]
        [InlineData("Kmart Cannington Aus","ANZ",Categories.Shopping.OtherShopping)]
        [InlineData("Bunnings Innaloo","ANZ",Categories.Home.HomeImprovement)]
        [InlineData("City Of Perth Park1 Perth","ParkingTolls",Categories.Transport.ParkingTolls)]
        [InlineData("Virgin Australia Airli Spring Hill Aus","WESTPAC_INTERNET_BANKING",Categories.HolidayTravel.Flights)]
        [InlineData("My Healthy Place Floreat","WESTPAC_INTERNET_BANKING",Categories.HealthBeauty.Chemists)]
        public async Task Predict_test_sample(string description, string bank, string subcategory)
        {
            var result = await _predict.PredictAsync(new PredictionRequest()
            {
                Description = description,
                Bank =bank
            });

            Console.WriteLine(result);
            result.Should().Be(subcategory);
        }

        ///https://docs.microsoft.com/en-us/dotnet/api/microsoft.ml.models.classificationmetrics?view=ml-dotnet
        [Fact]
        public async Task Evaluate_accuracy_micro_greater_than_95_percent()
        {
            var metrics = await _predict.EvaluateAsync(Test);
            Console.WriteLine();
            Console.WriteLine("PredictionModel quality metrics evaluation");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"AccuracyMacro: {metrics.AccuracyMacro}");
            Console.WriteLine();
            Console.WriteLine("the number of correctly predicted instances in the class, divided by the total number of instances in the class");
            Console.WriteLine();
            Console.WriteLine($"AccuracyMicro: {metrics.AccuracyMicro}");
            Console.WriteLine("aggregate the contributions of all classes to compute the average metric");
            Console.WriteLine();
            Console.WriteLine($"LogLoss: {metrics.LogLoss}");
            Console.WriteLine("Log Loss quantifies the accuracy of a classifier by penalising false classifications. Minimising the Log Loss is basically equivalent to maximising the accuracy of the classifier but has a twist see https://www.r-bloggers.com/making-sense-of-logarithmic-loss/");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"confusion matrix: {metrics.ConfusionMatrix}");
            Console.WriteLine("table that is often used to describe the performance of a classification model (or 'classifier') on a set of test data for which the true values are known.");
            Console.WriteLine();
      
            metrics.AccuracyMicro.Should().BeGreaterOrEqualTo(0.95);

            /*
             *
             * Micro- and macro-averages (for whatever metric) will compute slightly different things,
             * and thus their interpretation differs.
             *
             * A macro-average will compute the metric independently for each class and then take the average (hence treating all classes equally),
             * whereas a micro-average will aggregate the contributions of all classes to compute the average metric.
             *
             * In a multi-class classification setup, micro-average is preferable if you suspect there might be class imbalance
             * (i.e you may have many more examples of one class than of other classes).
             */
        }


        [Fact]
        public async Task PredictMany_will_run_through_transactions()
        {
            var json = BankStatementsFixture.Statement2;
            var response = new BankStatementDownload(json).FetchAllResponse;
            var account = response.Accounts.First();
            var statementData = account.StatementData;
            var data = statementData.Details.Select(x => new PredictionRequest() {Bank = account.Institution, Description = x.Text,Amount = x.Amount});
          
            var results = await _predict.PredictManyAsync(data.AsEnumerable());
            results.Count(x => x.Method == (Contracts.PredictionMethod.MultiClassClassifier)).Should().Be(results.Count());
          
          var groupedList = from r in results.Select(x=>x.PredictedValue)
            group r by r
            into newGroup
            select new {
              Category = newGroup.Key,
              Total = newGroup.Count(),
            };

          groupedList.Count().Should().BeGreaterThan(0);
        }
    }
}