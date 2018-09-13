using Vita.Contracts;

namespace Vita.Predictor.TextMatch
{
    public interface IMatchWhat
    {
        TransactionType? What(TextClassificationResult result, string sentence);
    }
}