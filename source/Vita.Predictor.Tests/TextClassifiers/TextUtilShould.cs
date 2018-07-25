using FluentAssertions;
using Vita.Predictor.TextClassifiers;
using Xunit;

namespace Vita.Predictor.Tests.TextClassifiers
{
    public class TextUtilShould
    {
        [Fact]
        public void Clean_only_world()
        {
          const string interest = @"interest";
          TextUtil.CleanWord(interest).Should().Be(interest);
        }
    }
}
