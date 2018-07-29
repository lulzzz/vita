using System.Threading.Tasks;
using Microsoft.ML.Models;

namespace Vita.Contracts
{
  public interface IPredict
  {
    Task<string> PredictAsync(PredictionRequest item);
    Task<string> TrainAsync(string trainpath, string testpath = null);
    Task<ClassificationMetrics> EvaluateAsync(string testPath);
  }
}