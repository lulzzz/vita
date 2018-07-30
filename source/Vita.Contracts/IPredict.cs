using System.Threading.Tasks;
using Microsoft.ML.Models;

namespace Vita.Contracts
{
  public interface IPredict
  {
    Task<string> PredictAsync(PredictionRequest request);
    Task<string> TrainAsync(string trainpath);
    Task<ClassificationMetrics> EvaluateAsync(string testPath);
  }
}