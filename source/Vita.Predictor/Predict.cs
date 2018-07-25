using System;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Models;
using Microsoft.ML.Runtime;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using Vita.Contracts;
using IPredict = Vita.Contracts.IPredict;

namespace Vita.Predictor
{
  public class Predict : IPredict
  {

    private static PredictionModel<PredictionRequest, PredictedLabel> _model;

    public async Task<string> TrainAsync(string trainpath, string testpath = null)
    {
      var pipeline = new LearningPipeline();

      pipeline.Add(new TextLoader(trainpath).CreateFrom<PredictionRequest>(separator:'\t',useHeader: true));

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

      var model = pipeline.Train<PredictionRequest, PredictedLabel>();

      await model.WriteAsync(PredictorSettings.ModelPath);

      Console.WriteLine("=============== End training ===============");
      Console.WriteLine("The model is saved to {0}", PredictorSettings.ModelPath);
      return PredictorSettings.ModelPath;
    }

    public async Task<string> PredictAsync(PredictionRequest item)
    {
      if (_model == null) _model = await PredictionModel.ReadAsync<PredictionRequest, PredictedLabel>(PredictorSettings.ModelPath);

      var prediction = _model.Predict(item);

      return prediction.SubCategory;
    }

    public async Task<ClassificationMetrics> EvaluateAsync(string testPath)
    {
      var testData = new TextLoader(testPath).CreateFrom<PredictionRequest>();
      var evaluator = new ClassificationEvaluator();

      if (_model == null) _model = await PredictionModel.ReadAsync<PredictionRequest, PredictedLabel>(PredictorSettings.ModelPath);

      var metrics = evaluator.Evaluate(_model, testData);

      Console.WriteLine();
      Console.WriteLine("PredictionModel quality metrics evaluation");
      Console.WriteLine("------------------------------------------");
      Console.WriteLine($"confusion matrix: {metrics.ConfusionMatrix}");
      return metrics;
    }
  }
}