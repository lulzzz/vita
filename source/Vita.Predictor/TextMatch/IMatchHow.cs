using Vita.Contracts;

namespace Vita.Predictor.TextClassifiers
{
    public interface IMatchHow
    {
        PaymentMethodType How(string sentence);
    }
}