using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Vita.Domain.Infrastructure;
using Xunit;

namespace Vita.Domain.Tests.Infrastructure
{
  public class CacherShould
  {
    [Fact]
    public void Handle_concurrent_access()
    {
      // Arrange
      const string testKey = "key";
      const string result = "Ok";
      var timesRetrieved = 0;

      Task<string> RetrievalFuncAsync()
      {
        timesRetrieved++;
        Thread.Sleep(2);
        return Task.FromResult(result);
      }

      // Act
      var task1 = Task.Run(() => Cacher.GetAsync(testKey, (Func<Task<string>>) RetrievalFuncAsync));
      var task2 = Task.Run(() => Cacher.GetAsync(testKey, (Func<Task<string>>) RetrievalFuncAsync));
      Task.WaitAll(task1, task2);

      //Assert
      task1.Result.Should().Be(result);
      task2.Result.Should().Be(result);
      timesRetrieved.Should()
        .Be(1, "Retrival function should be called only the first time. Second time should be retrieved from cache");
    }
  }
}