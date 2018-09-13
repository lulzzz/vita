using Vita.Contracts;

namespace Vita.Predictor.TextMatch
{
    public interface IMatchWhere
    {
        Locality Where(string sentence);
    }
}