using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using Xunit;

namespace Vita.Domain.Tests
{
    public static class TestExtensions
    {

      public static void ShouldContainItem<T>(this IEnumerable<T> collection, Func<T, bool> predicate, string message=null)
      {
      foreach (var item in collection)
        {
          if (predicate(item))
          {
            Assert.True(true, $"found {message}");
            return;
          }
        }

        Assert.True(false, $"NOT found {message}");
    }

      public static void ShouldBeJsonSerializable<T>(this T subject, string because = "", params object[] becauseArgs)
      {
        // modify from Should().BeXmlSerializable()
        try
        {
          var json = JsonConvert.SerializeObject(subject);
          var clone = JsonConvert.DeserializeObject<T>(json);
          clone.Should().BeEquivalentTo(subject, (Func<EquivalencyAssertionOptions<object>, EquivalencyAssertionOptions<object>>)(options => options.RespectingRuntimeTypes().IncludingFields().IncludingProperties()));
        }
        catch (Exception ex)
        {
          Execute.Assertion.BecauseOf(because, becauseArgs).FailWith("Expected {0} to be serializable{reason}, but serialization failed with:\r\n\r\n{1}", subject.GetType().Name, ex.Message);
        }
      }
  }
}
