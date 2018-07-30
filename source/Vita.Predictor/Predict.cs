﻿using System;
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

        public async Task<string> TrainAsync(string trainpath)
        {
            // pipeline
            var pipeline = new LearningPipeline
            {
                // load from CSV --> SubCategory manually classified), Description, Bank, Amount, AccountName, Notes, Tags
                new TextLoader(trainpath).CreateFrom<BankStatementLineItem>(separator: ',', useHeader: true),
                new Dictionarizer(("SubCategory", "Label")), //Converts input values (words, numbers, etc.) to index in a dictionary.
                // ngram analysis over the transaction description
                new TextFeaturizer("Description", "Description")
                {
                    TextCase = TextNormalizerTransformCaseNormalizationMode.Lower,
                    WordFeatureExtractor = new NGramNgramExtractor(){Weighting = NgramTransformWeightingCriteria.TfIdf}
                },
                new TextFeaturizer("Bank", "Bank")
                {
                    TextCase = TextNormalizerTransformCaseNormalizationMode.Lower
                },
                // feature column using bank and description
                new ColumnConcatenator("Features", "Bank", "Description"),
                //The SDCA method combines several of the best properties and capabilities of logistic regression and SVM algorithms
                new StochasticDualCoordinateAscentClassifier(){Shuffle = false, NumThreads = 1},
                //Transforms a predicted label column to its original values, unless it is of type bool
                new PredictedLabelColumnOriginalValueConverter() {PredictedLabelColumn = "PredictedLabel"}
            };

            // training 
            Console.WriteLine("=============== Start training ===============");
            var watch = System.Diagnostics.Stopwatch.StartNew();

            _model = pipeline.Train<BankStatementLineItem, PredictedLabel>();
            await _model.WriteAsync(PredictorSettings.Model1Path);

            watch.Stop();
            Console.WriteLine($"=============== End training ===============");
            Console.WriteLine($"training took {watch.ElapsedMilliseconds} milliseconds");
            Console.WriteLine("The model is saved to {0}", PredictorSettings.Model1Path);

            return PredictorSettings.Model1Path;
        }

        public async Task<string> PredictAsync(PredictionRequest request)
        {
            if (_model == null)
            {
                _model = await PredictionModel.ReadAsync<BankStatementLineItem, PredictedLabel>(PredictorSettings.Model1Path);
            }

            var item = new BankStatementLineItem
            {
                Description = request.Description,
                AccountName = request.AccountName,
                Amount = request.Amount,
                AccountNumber = request.AccountNumber,
                Bank = request.Bank,
                TransactionUtcDate = request.TransactionUtcDate,
                Notes = request.Notes,
                Tags = request.Tags
            };

            var prediction = _model.Predict(item);

            return prediction.SubCategory;
        }


        public async Task<ClassificationMetrics> EvaluateAsync(string testPath)
        {
            var testData = new TextLoader(testPath).CreateFrom<BankStatementLineItem>(separator: ',', useHeader: true);
            var evaluator = new ClassificationEvaluator();

            if (_model == null)
            {
                _model = await PredictionModel.ReadAsync<BankStatementLineItem, PredictedLabel>(PredictorSettings.Model1Path);
            }

            var metrics = evaluator.Evaluate(_model, testData);
            return metrics;
        }
    }
}