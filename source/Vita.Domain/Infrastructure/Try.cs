using System;
using System.Diagnostics;
using System.Threading;

namespace Vita.Domain.Infrastructure
{
    public static class Try
    {
        public static T Get<T>(Func<T> getter)
        {
            try
            {
                return getter();
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static T Retry<T>(Func<T> getter, int times = 3, int interval = 1)
        {
            var attempts = 1;
            while (attempts < times)
            {
                attempts++;
                try
                {
                    return getter();
                }
                catch (Exception e)
                {
                    Trace.TraceWarning($"Retry {attempts} failed: {e.Message}");
                }

                Thread.Sleep(TimeSpan.FromSeconds(interval));
            }
            return getter();
        }
    }
}