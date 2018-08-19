using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Vita.Domain.Infrastructure.EventFlow
{
    /// <summary>
    /// http://stackoverflow.com/questions/31138179/asynchronous-locking-based-on-a-key
    /// </summary>
    public static class AsyncLocker
    {
        private sealed class RefCounted<T>
        {
            public RefCounted(T value)
            {
                RefCount = 1;
                Value = value;
            }

            public int RefCount { get; set; }
            public T Value { get; }
        }

        private static readonly Dictionary<string, RefCounted<SemaphoreSlim>> SemaphoreSlims
            = new Dictionary<string, RefCounted<SemaphoreSlim>>();

        private static SemaphoreSlim GetOrCreate(string key)
        {
            RefCounted<SemaphoreSlim> item;
            lock (SemaphoreSlims)
            {
                if (SemaphoreSlims.TryGetValue(key, out item))
                {
                    ++item.RefCount;
                }
                else
                {
                    item = new RefCounted<SemaphoreSlim>(new SemaphoreSlim(1, 1));
                    SemaphoreSlims[key] = item;
                }
            }
            return item.Value;
        }

        public static IDisposable Lock(string key)
        {
            GetOrCreate(key).Wait();
            return new Releaser(key);
        }

        public static async Task<IDisposable> LockAsync(string key)
        {
            await GetOrCreate(key).WaitAsync().ConfigureAwait(false);
            return new Releaser(key);
        }

        private sealed class Releaser : IDisposable
        {
            private string Key { get; }

            public Releaser(string key)
            {
                Key = key;
            }

            public void Dispose()
            {
                RefCounted<SemaphoreSlim> item;
                lock (SemaphoreSlims)
                {
                    item = SemaphoreSlims[Key];
                    --item.RefCount;
                    if (item.RefCount == 0)
                        SemaphoreSlims.Remove(Key);
                }
                item.Value.Release();
            }
        }
    }
}