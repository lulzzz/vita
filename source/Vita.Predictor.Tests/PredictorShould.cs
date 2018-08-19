using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Vita.Contracts;
using Vita.Contracts.SubCategories;
using Vita.Domain.BankStatements.Download;
using Vita.Domain.BankStatements.Models;
using Vita.Domain.Infrastructure;
using Vita.Domain.Tests;
using Vita.Domain.Tests.BankStatements.Models.Fixtures;
using Vita.Predictor.Tests.TextClassifiers;
using Vita.Predictor.TextClassifiers;
using Xunit;

namespace Vita.Predictor.Tests
{
    [Collection("DataCollection")]
    public class PredictorShould
    {
        public static string Data = PredictionModelWrapper.GetFilePath("data-sample.csv");
        public static string Train = PredictionModelWrapper.GetFilePath("train.csv");
        public static string Test = PredictionModelWrapper.GetFilePath("test.csv");

        private readonly IPredict _predict;
        private readonly ITextClassifier _textClassifier;
        private readonly DataFixture _dataFixture;
        private readonly Account _account;

        public PredictorShould()
        {
            _dataFixture = new DataFixture();
            _dataFixture.Init();
            _predict = new Predict();
            _textClassifier =
                new TextClassifier(_dataFixture.Companies, _dataFixture.Localities, _dataFixture.Classifiers)
                {
                    UseCache = false
                };
            _account = _dataFixture.GetTestBankAccount();
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
                var request = new PredictionRequest
                {
                    Description = item.Description,
                    Amount = item.Amount,
                    //Tags = item.Tags,
                    //Notes = item.Notes,
                    Bank = item.Bank
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
        [InlineData("EFTPOS ALDI 27 CARRUM DOWNS AU", "ANZ", Categories.Groceries.Supermarkets)]
        [InlineData("Kmart Cannington Aus", "ANZ", Categories.Shopping.OtherShopping)]
        [InlineData("Bunnings Innaloo", "ANZ", Categories.Home.HomeImprovement)]
        [InlineData("City Of Perth Park1 Perth", "ParkingTolls", Categories.Transport.ParkingTolls)]
        [InlineData("Virgin Australia Airli Spring Hill Aus", "WESTPAC_INTERNET_BANKING",
            Categories.HolidayTravel.Flights)]
        [InlineData("My Healthy Place Floreat", "WESTPAC_INTERNET_BANKING", Categories.HealthBeauty.Chemists)]
        public async Task Predict_test_sample(string description, string bank, string subcategory)
        {
            var result = await _predict.PredictAsync(new PredictionRequest
            {
                Description = description,
                Bank = bank
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
            Console.WriteLine(
                "the number of correctly predicted instances in the class, divided by the total number of instances in the class");
            Console.WriteLine();
            Console.WriteLine($"AccuracyMicro: {metrics.AccuracyMicro}");
            Console.WriteLine("aggregate the contributions of all classes to compute the average metric");
            Console.WriteLine();
            Console.WriteLine($"LogLoss: {metrics.LogLoss}");
            Console.WriteLine(
                "Log Loss quantifies the accuracy of a classifier by penalising false classifications. Minimising the Log Loss is basically equivalent to maximising the accuracy of the classifier but has a twist see https://www.r-bloggers.com/making-sense-of-logarithmic-loss/");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"confusion matrix: {metrics.ConfusionMatrix}");
            Console.WriteLine(
                "table that is often used to describe the performance of a classification model (or 'classifier') on a set of test data for which the true values are known.");
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
            var predictionModel = await GetPredictionResults();
            predictionModel.Count(x => x.Method == Contracts.PredictionMethod.MultiClassClassifier).Should()
                .Be(predictionModel.Count());

            var groupedList = from r in predictionModel.Select(x => x.PredictedValue)
                group r by r
                into newGroup
                select new
                {
                    Category = newGroup.Key,
                    Total = newGroup.Count()
                };

            groupedList.Count().Should().BeGreaterThan(0);
        }


        [Theory]
        //[InlineData(Categories.Transport.Fuel)]
        //[InlineData(Categories.Transport.PublicTransport)]
        //[InlineData(Categories.Transport.TaxiRideshare)]
        //[InlineData(Categories.HouseholdUtilities.ElectricityGas)]
        //[InlineData(Categories.HouseholdUtilities.PhoneInternet)]
        //[InlineData(Categories.HouseholdUtilities.Water)]
        //[InlineData(Categories.Income.SalaryWages)]
        //[InlineData(Categories.Income.Deposits)]
        //[InlineData(Categories.Income.OtherIncome)]
        //[InlineData(Categories.Entertainment.BettingLotteries)]
        //[InlineData(Categories.Entertainment.Events)]
        //[InlineData(Categories.Entertainment.MediaSubscriptions)]
        //[InlineData(Categories.Entertainment.Movies)]
        //[InlineData(Categories.HealthBeauty.Chemists)]
        //[InlineData(Categories.HealthBeauty.DoctorsDentist)]
        //[InlineData(Categories.HealthBeauty.Eyewear)]
        //[InlineData(Categories.HealthBeauty.GymsFitness)]
        //[InlineData(Categories.HealthBeauty.HairBeauty)]
        //[InlineData(Categories.HealthBeauty.OtherHealthBeauty)]
        //[InlineData(Categories.HolidayTravel.Flights)]
        //[InlineData(Categories.HolidayTravel.HotelsAccomodation)]
        [InlineData(Categories.HolidayTravel.OtherTravel)]
        //[InlineData(Categories.Insurance.CarInsurance)]
        //[InlineData(Categories.Insurance.HealthLifeInsurance)]
        //[InlineData(Categories.Insurance.HomeInsurance)]
        public async Task Predict_vs_text_classifier(string expected)
        {
            var predictionModel = await GetPredictionResults();
            var textClassifiers = await GetTextClassifiers();

            var predictionResults = predictionModel as PredictionResult[] ?? predictionModel.ToArray();
            var textClassificationResults = textClassifiers as TextClassificationResult[] ?? textClassifiers.ToArray();
            
            predictionResults.Count().Should().Be(textClassificationResults.Count(), "Not comparing the same inputs");

            var totals1 = from r in predictionResults.Select(x => x.PredictedValue)
                group r by r
                into newGroup
                select new
                {
                    Category = newGroup.Key,
                    Total = newGroup.Count()
                };

            var totals2 = from r in textClassificationResults.Select(x => x.Classifier?.SubCategory)
                group r by r
                into newGroup
                select new
                {
                    Category = newGroup.Key,
                    Total = newGroup.Count()
                };

            var predicted = totals1.SingleOrDefault(x => x.Category ==expected);

            var textmatched = totals2.SingleOrDefault(x => x.Category ==expected);

            predictionResults
                .Where(x => x.PredictedValue == expected)
                .Select(x => x.Request.Description)
                .Dump($"Predicted {expected}");

            textClassificationResults
                .Where(x => x.Classifier?.SubCategory == expected)
                .Select(x => x.SearchPhrase)
                .Dump($"Test classified {expected}");

            
            int totalpredicted = predicted?.Total ?? 0;
            int totaltextmatched = textmatched?.Total ?? 0;

            totaltextmatched
                .Should()
                .Be(totalpredicted, $"{expected} - predicted {totalpredicted} textmatched {totaltextmatched}");
        }

        private async Task<IEnumerable<PredictionResult>> GetPredictionResults()
        {

            var data = _account.StatementData.Details
                .Select(x =>new PredictionRequest {Bank = _account.Institution, Description = x.Text, Amount = x.Amount});

            var predictionModel = await _predict.PredictManyAsync(data.AsEnumerable());
            return predictionModel;
        }

        private async Task<IEnumerable<TextClassificationResult>> GetTextClassifiers()
        {
            
            var list = new List<TextClassificationResult>();
            foreach (var det in _account.StatementData.Details)
            {
                var match = await _textClassifier.Match(det.Text);
                Console.WriteLine(match.Classifier != null
                    ? $"{det.Text}  {match.Classifier.SubCategory}"
                    : $"{det.Text}  NONE");

                list.Add(match);
            }

            return list;
        }
    }
}