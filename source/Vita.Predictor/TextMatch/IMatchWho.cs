using Vita.Contracts;

namespace Vita.Predictor.TextMatch
{
  public interface IMatchWho
  {
    Company Who(string sentence);
  }
}