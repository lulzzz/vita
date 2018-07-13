using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExtensionMinder;
using FluentAssertions;
using Vita.Domain.BankStatements;
using Vita.Domain.BankStatements.Tsv;
using Vita.Domain.Services.Predictions;
using Xunit;

namespace Vita.Domain.Tests.Services.Predictions
{
    public class PredictorShould
    {
        public static string Data = PredictorSettings.GetFilePath("data-sample.csv");
        public static string Train = PredictorSettings.GetFilePath("train.tsv");
        public static string Test = PredictorSettings.GetFilePath("train.tsv");

        
        //[Fact(Skip = "run on demand")]
        [Fact]
        public async Task Train_model()
        {
            var model = await new Predictor().TrainAsync(Train);
            model.Length.Should().NotBe(0);
            File.Exists(model).Should().Be(true);
        }

        [Fact]
        public async Task Predict()
        {
            var contents = File.ReadAllText(Test);
            var list = new List<KeyValuePair<string, string>>();
            var data = TsvHelper.Read(contents);

            foreach (var item in data)
            {
                var result = await new Predictor().PredictAsync(item);
                list.Add(new KeyValuePair<string, string>(item.SubCategory, result));
            }

            var correct = list.Count(x => x.Key == x.Value);
            var over = (double) list.Count;
            var percentage = correct / over;
            Console.WriteLine(percentage.ToString("P5"));

            percentage.Should().BeGreaterThan(.3);
        }

        [Fact]
        public async Task Predict_single()
        {
            var result = await new Predictor().PredictAsync(new BankStatementLineItem
            {
                Description = "EFTPOS ALDI 27 CARRUM DOWNS AU",
                Bank = "ANZ"
            });

            Console.WriteLine(result);
        }

        [Fact]
        public async Task Evaluate_gives_good_prediction_results()
        {
            var model = await new Predictor().EvaluateAsync(Test);
            model.AccuracyMacro.Should().BeGreaterOrEqualTo(0.5);
        }
    }
}