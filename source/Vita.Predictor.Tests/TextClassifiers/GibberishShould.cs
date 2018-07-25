using FluentAssertions;
using Vita.Predictor.TextClassifiers;
using Xunit;

namespace Vita.Predictor.Tests.TextClassifiers
{
    public class GibberishShould
    {
      [Fact]
      public void Be_true_if_more_numbers_than_letters()
      {
        string text = "abc1234";
        Gibberish.IsGibberish(text).Should().Be(true);
      }

      [Fact]
      public void Be_false_if_more_numbers_than_letters()
      {
        string text = "abcd123";
        Gibberish.IsGibberish(text).Should().Be(false);
      }

      [Fact]
      public void Be_false_if_more_number_count_same_as_letter_count()
      {
        string text = "abc123";
        Gibberish.IsGibberish(text).Should().Be(false);
      }

      [Fact]
      public void Be_false_if_text_is_postcode()
      {
        string text = "6024";
        Gibberish.IsGibberish(text).Should().Be(false);
      }
  }
}
