using FluentAssertions;
using Vita.Domain.Services.TextClassifiers;
using Xunit;

namespace Vita.Domain.Tests.Services.TextClassifiers
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
