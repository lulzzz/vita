using System.Threading.Tasks;

namespace Vita.Contracts
{
  public interface IPredict
  {
    Task<string> PredictAsync(PredictionRequest item);
    Task<string> TrainAsync(string trainpath, string testpath = null);
  }
}