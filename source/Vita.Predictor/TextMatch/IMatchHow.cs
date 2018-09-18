using Vita.Contracts;

namespace Vita.Predictor.TextMatch
{
    public interface IMatchHow
    {
        PaymentMethodType How(string sentence);
    }
}