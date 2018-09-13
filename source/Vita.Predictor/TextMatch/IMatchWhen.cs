using System;

namespace Vita.Predictor.TextMatch
{
    public interface IMatchWhen
    {
        DateTime? When(string sentence);
    }
}