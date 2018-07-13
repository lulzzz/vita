using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Vita.Domain.Infrastructure
{
  public static class AsyncUtil
  {

    /// <summary>
    ///  Execute's an async Task<T> method which has a void return value synchronously
    /// </summary>
    /// <param name="task"></param>
    /// <param name="onError"></param>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static void RunSync(Func<Task> task, Action<Exception> onError = null)
    {
      var oldContext = SynchronizationContext.Current;
      var synch = new ExclusiveSynchronizationContext();
      SynchronizationContext.SetSynchronizationContext(synch);
      synch.Post(async _ =>
      {
        try
        {
          await task();
        }
        catch (Exception e)
        {
          synch.InnerException = e;
          onError?.Invoke(e);
          throw;
        }
        finally
        {
          synch.EndMessageLoop();
        }
      }, null);
      synch.BeginMessageLoop();

      SynchronizationContext.SetSynchronizationContext(oldContext);
    }

    /// <summary>
    ///  Execute's an async Task<T> method which has a T return type synchronously
    /// </summary>
    [DebuggerStepThrough]
    [DebuggerHidden]
    public static T RunSync<T>(Func<Task<T>> task, Action<Exception> onError = null)
    {
      var oldContext = SynchronizationContext.Current;
      var synch = new ExclusiveSynchronizationContext();
      SynchronizationContext.SetSynchronizationContext(synch);
      T ret = default(T);
      synch.Post(async _ =>
      {
        try
        {
          ret = await task().ConfigureAwait(false);
        }
        catch (Exception e)
        {
          synch.InnerException = e;
          onError?.Invoke(e);
        }
        finally
        {
          synch.EndMessageLoop();
        }
      }, null);
      synch.BeginMessageLoop();
      SynchronizationContext.SetSynchronizationContext(oldContext);
      return ret;
    }

    [DebuggerStepThrough]
    private class ExclusiveSynchronizationContext : SynchronizationContext
    {
      private readonly Queue<Tuple<SendOrPostCallback, object>> _items =
          new Queue<Tuple<SendOrPostCallback, object>>();

      private readonly AutoResetEvent _workItemsWaiting = new AutoResetEvent(false);

      private bool _done;
      public Exception InnerException { get; set; }

      [DebuggerStepThrough]
      public override void Send(SendOrPostCallback d, object state)
      {
        throw new NotSupportedException("We cannot send to our same thread");
      }

      [DebuggerStepThrough]
      public override void Post(SendOrPostCallback d, object state)
      {
        lock (_items)
        {
          _items.Enqueue(Tuple.Create(d, state));
        }
        _workItemsWaiting.Set();
      }

      [DebuggerStepThrough]
      public void EndMessageLoop()
      {
        Post(_ => _done = true, null);
      }

      [DebuggerStepThrough]
      public void BeginMessageLoop()
      {
        while (!_done)
        {
          Tuple<SendOrPostCallback, object> task = null;
          lock (_items)
          {
            if (_items.Count > 0)
            {
              task = _items.Dequeue();
            }
          }
          if (task != null)
          {
            task.Item1(task.Item2);
            if (InnerException != null) // the method threw an exeption
            {
              throw new AggregateException("AsyncHelpers.Run method threw an exception.", InnerException);
            }
          }
          else
          {
            _workItemsWaiting.WaitOne();
          }
        }
      }

      public override SynchronizationContext CreateCopy()
      {
        return this;
      }
    }
  }
}

