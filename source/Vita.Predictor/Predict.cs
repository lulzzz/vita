using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Models;
using Microsoft.ML.Runtime;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using Serilog;
using Serilog.Core;
using Vita.Contracts;
using Vita.Domain.BankStatements;

namespace Vita.Predictor
{
    public class Predict : IPredict
    {
        private static PredictionModel<BankStatementLineItem, PredictedLabel> _model;

        public async Task<string> TrainAsync(string trainpath, bool writeToDisk = true)
        {
            // pipeline encapsulates the data loading, data processing/featurization, and learning algorithm
            var pipeline = new LearningPipeline
            {
                // load from CSV --> SubCategory, Description, Bank, Amount,
                new TextLoader(trainpath).CreateFrom<BankStatementLineItem>(separator: ',', useHeader: true),
                
                //Converts input values (words, numbers, etc.) to index in a dictionary.
                new Dictionarizer(("SubCategory", "Label")), 
                
                // convert the data columns to the feature. For that TextFeaturizer
                // ngram analysis over the transaction description
                new TextFeaturizer("Description", "Description")
                {
                    TextCase = TextNormalizerTransformCaseNormalizationMode.Lower,
                    WordFeatureExtractor = new NGramNgramExtractor()
                    {
                       // Term frequency -- the number of times that term t occurs in document d
                        Weighting = NgramTransformWeightingCriteria.Tf,
                    }
                },
                new TextFeaturizer("Bank", "Bank")
                {
                    TextCase = TextNormalizerTransformCaseNormalizationMode.Lower
                },
                // feature column using bank and description
                new ColumnConcatenator("Features", "Bank", "Description"),

                //********************************************************************
                // classifiers
                //********************************************************************
                //new NaiveBayesClassifier(),
                new StochasticDualCoordinateAscentClassifier(){Shuffle = false, NumThreads = 1}, 
                //new LightGbmClassifier(),                
                //********************************************************************

                //Transforms a predicted label column to its original values, unless it is of type bool
                new PredictedLabelColumnOriginalValueConverter() {PredictedLabelColumn = "PredictedLabel"}
            };

            //********************************************************************
            // training 
            //********************************************************************
            Console.WriteLine("=============== Start training ===============");
            var watch = System.Diagnostics.Stopwatch.StartNew();

            _model = pipeline.Train<BankStatementLineItem, PredictedLabel>();
            await _model.WriteAsync(PredictionModelWrapper.Model1Path);

            watch.Stop();
            Console.WriteLine($"=============== End training ===============");
            Console.WriteLine($"training took {watch.ElapsedMilliseconds} milliseconds");
            Console.WriteLine("The model is saved to {0}", PredictionModelWrapper.Model1Path);
            //********************************************************************

            return PredictionModelWrapper.Model1Path;
        }

        //Instantiate a Singleton of the Semaphore with a value of 1. This means that only 1 thread can be granted access at a time.
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1,1);

        public async Task<string> PredictAsync(PredictionRequest request)
        {
            if (_model == null)
            {
                _model = await PredictionModel.ReadAsync<BankStatementLineItem, PredictedLabel>(PredictionModelWrapper.GetModel());
            }

            var item = new BankStatementLineItem
            {
                Description = request.Description,
                Amount = request.Amount,
                //AccountNumber = request.AccountNumber,
                //AccountName = request.AccountName,
                Bank = request.Bank,
                //TransactionUtcDate = request.TransactionUtcDate,
                //Notes = request.Notes,
                //Tags = request.Tags
            };

            await SemaphoreSlim.WaitAsync();
            PredictedLabel predicted = null;
            try
            {
                predicted = _model.Predict(item);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "error {err}", request);
                throw;
            }
            finally
            {
                //When the task is ready, release the semaphore. It is vital to ALWAYS release the semaphore when we are ready, or else we will end up with a Semaphore that is forever locked.
                //This is why it is important to do the Release within a try...finally clause; program execution may crash or take a different path, this way you are guaranteed execution
                SemaphoreSlim.Release();
            }

            return predicted.SubCategory;
        }

        public async Task<IEnumerable<PredictionResult>> PredictManyAsync(IEnumerable<PredictionRequest> requests)
        {
            var results = new List<PredictionResult>();
            foreach (var item in requests)
            {
                var result = new PredictionResult()
                {
                    Request = item,
                    PredictedValue = await PredictAsync(item),
                    Method = Contracts.PredictionMethod.MultiClassClassifier
                };
                results.Add(result);
            }

            return results;
        }

        public async Task<ClassificationMetrics> EvaluateAsync(string testPath)
        {
            var testData = new TextLoader(testPath).CreateFrom<BankStatementLineItem>(separator: ',', useHeader: true);
            var evaluator = new ClassificationEvaluator();

            if (_model == null)
            {
                _model = await PredictionModel.ReadAsync<BankStatementLineItem, PredictedLabel>(PredictionModelWrapper.Model1Path);
            }

            var metrics = evaluator.Evaluate(_model, testData);
            return metrics;
        }
    }
}