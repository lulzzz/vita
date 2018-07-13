﻿using FluentAssertions;
using Vita.Domain.Services.TextClassifiers;
using Xunit;

namespace Vita.Domain.Tests.Services.TextClassifiers
{
  public class NGramProcessorShould
  {
    public const string Sentence = "hello this is an example sentence. I hope it handles words well. ain't that the truth!";

    [Fact]
    public void NGramProcessor_handles_trigrams()
    {
      var results = NGramProcessor.MakeNgrams(Sentence, 3);
      results.Should().Contain("hello this is");
      results.Should().Contain("an example sentence");
      results.Should().Contain("I hope it");
      results.Should().Contain("ain't that the");
    }

    [Fact]
    public void NGramProcessor_handles_bigrams()
    {
      var results = NGramProcessor.MakeNgrams(Sentence, 2);
      results.Should().Contain("hello this");
      results.Should().Contain("an example");
      results.Should().Contain("I hope");
      results.Should().Contain("ain't that");
    }

  }
}