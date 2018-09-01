using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ML.Models;

namespace Vita.Contracts
{
    public interface IPredict
    {
        Task<string> PredictAsync(PredictionRequest request);
        Task<IEnumerable<PredictionResult>> PredictManyAsync(IEnumerable<PredictionRequest> requests);
        Task<string> TrainAsync(string trainpath, bool writeToDisk = true, bool exportOnnx = false);
        Task<ClassificationMetrics> EvaluateAsync(string testPath);
    }
}