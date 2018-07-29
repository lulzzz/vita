using System;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Models;
using Microsoft.ML.Runtime;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using Vita.Contracts;
using Vita.Domain.BankStatements;

namespace Vita.Predictor
{
    public class Predict : IPredict
    {
        private static PredictionModel<BankStatementLineItem, PredictedLabel> _model;

        public async Task<string> TrainAsync(string trainpath, string testpath = null)
        {
            var pipeline = new LearningPipeline
            {
                new TextLoader(trainpath).CreateFrom<BankStatementLineItem>(separator: ',', useHeader: true),
                new Dictionarizer(("SubCategory", "Label")), //Converts input values (words, numbers, etc.) to index in a dictionary.
                new TextFeaturizer("Description", "Description")
                {
                    TextCase = TextNormalizerTransformCaseNormalizationMode.Lower,
                    WordFeatureExtractor = new NGramNgramExtractor()
                },

                new ColumnConcatenator("Features", "Description")
            };

            var classifier = new StochasticDualCoordinateAscentClassifier();
            pipeline.Add(classifier);

            pipeline.Add(new PredictedLabelColumnOriginalValueConverter {PredictedLabelColumn = "PredictedLabel"});

            Console.WriteLine("=============== Training model ===============");

            var model = pipeline.Train<BankStatementLineItem, PredictedLabel>();

            await model.WriteAsync(PredictorSettings.Model1Path);

            Console.WriteLine("=============== End training ===============");
            Console.WriteLine("The model is saved to {0}", PredictorSettings.Model1Path);
            return PredictorSettings.Model1Path;
        }

        public async Task<string> PredictAsync(PredictionRequest request)
        {
            if (_model == null)
                _model = await PredictionModel.ReadAsync<BankStatementLineItem, PredictedLabel>(PredictorSettings
                    .Model1Path);

            var item = new BankStatementLineItem();
            item.Description = request.Description;
            item.AccountName = request.AccountName;
            item.Amount = request.Amount;
            item.AccountNumber = request.AccountNumber;
            item.Bank = request.Bank;
            item.TransactionUtcDate = request.TransactionUtcDate;
            item.Notes = request.Notes;
            item.Tags = request.Tags;

            var prediction = _model.Predict(item);

            return prediction.SubCategory;
        }

        public async Task<ClassificationMetrics> EvaluateAsync(string testPath)
        {
            var testData = new TextLoader(testPath).CreateFrom<BankStatementLineItem>(separator: ',', useHeader: true);
            var evaluator = new ClassificationEvaluator();

            if (_model == null)
                _model = await PredictionModel.ReadAsync<BankStatementLineItem, PredictedLabel>(PredictorSettings
                    .Model1Path);

            var metrics = evaluator.Evaluate(_model, testData);
            return metrics;
        }
    }
}