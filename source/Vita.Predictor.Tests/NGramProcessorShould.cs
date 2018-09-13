using System.Linq;
using FluentAssertions;
using Xunit;

namespace Vita.Predictor.Tests
{
    public class NGramProcessorShould
    {
        public const string Sentence =
            "hello this is an example sentence. I hope it handles words well. ain't that the truth!";

        [Fact]
        public void Handle_trigrams()
        {
            var results = NGramProcessor.MakeNgrams(Sentence, 3);
            var arr = results as string[] ?? results.ToArray();
            arr.Should().Contain("hello this is");
            arr.Should().Contain("an example sentence");
            arr.Should().Contain("I hope it");
            arr.Should().Contain("ain't that the");
        }

        [Fact]
        public void Handle_bigrams()
        {
            var results = NGramProcessor.MakeNgrams(Sentence, 2);
            var arr = results as string[] ?? results.ToArray();
            arr.Should().Contain("hello this");
            arr.Should().Contain("an example");
            arr.Should().Contain("I hope");
            arr.Should().Contain("ain't that");
        }
    }
}