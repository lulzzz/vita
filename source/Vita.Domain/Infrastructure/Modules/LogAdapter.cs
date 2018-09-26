using System;
using System.Diagnostics;
using System.Text;
using EventFlow.Logs;
using Log = Serilog.Log;

namespace Vita.Domain.Infrastructure.Modules
{
    [DebuggerStepThrough]
    public class LogAdapter : ILog
    {
        public void Verbose(string format, params object[] args)
        {
            Log.Logger.Verbose(format, args);
        }

        public void Verbose(Exception exception, string format, params object[] args)
        {
            Log.Logger.Verbose(exception, format, args);
        }

        public void Verbose(Func<string> combersomeLogging)
        {
            Log.Logger.Verbose(combersomeLogging());
        }

        public void Verbose(Action<StringBuilder> combersomeLogging)
        {
            var sb = new StringBuilder();
            combersomeLogging(sb);
            Log.Logger.Verbose(sb.ToString());
        }

        public void Debug(string format, params object[] args)
        {
            Log.Logger.Debug(format, args);
        }

        public void Debug(Exception exception, string format, params object[] args)
        {
            Log.Logger.Debug(exception, format, args);
        }

        public void Debug(Func<string> combersomeLogging)
        {
            Log.Logger.Debug(combersomeLogging());
        }

        public void Debug(Action<StringBuilder> combersomeLogging)
        {
            var sb = new StringBuilder();
            combersomeLogging(sb);
            Log.Logger.Debug(sb.ToString());
        }

        public void Information(string format, params object[] args)
        {
            Log.Logger.Information(format, args);
        }

        public void Information(Exception exception, string format, params object[] args)
        {
            Log.Logger.Information(exception, format, args);
        }

        public void Information(Func<string> combersomeLogging)
        {
            Log.Logger.Information(combersomeLogging());
        }

        public void Information(Action<StringBuilder> combersomeLogging)
        {
            var sb = new StringBuilder();
            combersomeLogging.Invoke(sb);
            Log.Logger.Information(sb.ToString());
        }

        public void Warning(string format, params object[] args)
        {
            Log.Logger.Warning(format, args);
        }

        public void Warning(Exception exception, string format, params object[] args)
        {
            Log.Logger.Warning(exception, format, args);
        }

        public void Error(string format, params object[] args)
        {
            Log.Logger.Error(format, args);
        }

        public void Error(Exception exception, string format, params object[] args)
        {
            Log.Logger.Error(exception, format, args);
        }

        public void Fatal(string format, params object[] args)
        {
            Log.Logger.Fatal(format, args);
        }

        public void Fatal(Exception exception, string format, params object[] args)
        {
            Log.Logger.Fatal(exception, format, args);
        }

        public void Write(global::EventFlow.Logs.LogLevel logLevel, string format, params object[] args)
        {
            Write(logLevel, format, args);
        }

        public void Write(global::EventFlow.Logs.LogLevel logLevel, Exception exception, string format, params object[] args)
        {
            Write(logLevel, format, args);
        }

        public void Write(LogLevel logLevel, string format, params object[] args)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    Debug(string.Format(format, args));
                    break;
                case LogLevel.Information:
                    Information(string.Format(format, args));
                    break;
                case LogLevel.Warning:
                    Warning(string.Format(format, args));
                    break;
                case LogLevel.Error:
                    Error(string.Format(format, args));
                    break;
                case LogLevel.Verbose:
                    Verbose(string.Format(format, args));
                    break;
                case LogLevel.Fatal:
                    Fatal(string.Format(format, args));
                    break;
            }
        }

        public void Write(LogLevel logLevel, Exception exception, string format, params object[] args)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    Debug(string.Format(format, args));
                    break;
                case LogLevel.Information:
                    Information(string.Format(format, args));
                    break;
                case LogLevel.Warning:
                    Warning(exception, string.Format(format, args));
                    break;
                case LogLevel.Error:
                    Error(exception, string.Format(format, args));
                    break;
                case LogLevel.Verbose:
                    Verbose(string.Format(format, args));
                    break;
                case LogLevel.Fatal:
                    Fatal(exception, string.Format(format, args));
                    break;
            }
        }
    }
}