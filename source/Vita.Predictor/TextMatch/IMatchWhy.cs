using Vita.Contracts;

namespace Vita.Predictor.TextMatch
{
  public interface IMatchWhy
  {
    Classifier Why(string sentence);
  }
}