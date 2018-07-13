using System.Threading.Tasks;
using Vita.Domain.BankStatements;

namespace Vita.Domain.Services.Predictions
{
  public interface IPredictor
  {
    Task<string> PredictAsync(BankStatementLineItem item);
    Task<string> TrainAsync(string trainpath, string testpath = null);
  }
}