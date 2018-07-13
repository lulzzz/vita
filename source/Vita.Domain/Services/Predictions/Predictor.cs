using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Models;
using Microsoft.ML.Runtime;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using Vita.Domain.BankStatements;

namespace Vita.Domain.Services.Predictions
{
  public class Predictor : IPredictor
  {

    private static PredictionModel<BankStatementLineItem, PredictedLabel> _model;

    public async Task<string> TrainAsync(string trainpath, string testpath = null)
    {
      var pipeline = new LearningPipeline();

      pipeline.Add(new TextLoader(trainpath).CreateFrom<BankStatementLineItem>(separator:'\t',useHeader: true));

      pipeline.Add(new Dictionarizer(("SubCategory", "Label")));
      // A transform that turns a collection of text documents into numerical feature vectors. The feature vectors are normalized counts of (word and/or character) ngrams in a given tokenized text.
      pipeline.Add(new TextFeaturizer("Bank", "Bank"));
      
      var ngramExtrator = new NGramNgramExtractor()
      {
        Weighting = NgramTransformWeightingCriteria.TfIdf
      };
      pipeline.Add(new TextFeaturizer("Description", "Description")
      {
        TextCase = TextNormalizerTransformCaseNormalizationMode.Lower,
        WordFeatureExtractor = ngramExtrator
      });

      pipeline.Add(new ColumnConcatenator("Features", "Bank", "Description"));
      
      //pipeline.Add(new TextFeaturizer("Amount", "Amount"));
     
      //pipeline.Add(new ColumnConcatenator("Features","Bank", "Description"));


      var classifier = new StochasticDualCoordinateAscentClassifier()
      {
        NumThreads = 8, 
       // Shuffle = true,
       // FeatureColumn = "Features",
        
      };

      // classifier.LabelColumn = "SubCategory";
      pipeline.Add(classifier);

      pipeline.Add(new PredictedLabelColumnOriginalValueConverter {PredictedLabelColumn = "PredictedLabel"});

      Console.WriteLine("=============== Training model ===============");

      var model = pipeline.Train<BankStatementLineItem, PredictedLabel>();

      await model.WriteAsync(PredictorSettings.ModelPath);

      Console.WriteLine("=============== End training ===============");
      Console.WriteLine("The model is saved to {0}", PredictorSettings.ModelPath);
      return PredictorSettings.ModelPath;
    }

    public async Task<string> PredictAsync(BankStatementLineItem item)
    {
      if (_model == null) _model = await PredictionModel.ReadAsync<BankStatementLineItem, PredictedLabel>(PredictorSettings.ModelPath);

      var prediction = _model.Predict(item);

      return prediction.SubCategory;
    }

    public async Task<ClassificationMetrics> EvaluateAsync(string testPath)
    {
      var testData = new TextLoader(testPath).CreateFrom<BankStatementLineItem>();
      var evaluator = new ClassificationEvaluator();

      if (_model == null) _model = await PredictionModel.ReadAsync<BankStatementLineItem, PredictedLabel>(PredictorSettings.ModelPath);

      var metrics = evaluator.Evaluate(_model, testData);

      Console.WriteLine();
      Console.WriteLine("PredictionModel quality metrics evaluation");
      Console.WriteLine("------------------------------------------");
      Console.WriteLine($"confusion matrix: {metrics.ConfusionMatrix}");
      return metrics;
    }
  }
}