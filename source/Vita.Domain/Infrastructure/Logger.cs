using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using EventFlow.Aggregates;
using EventFlow.Commands;
using ExtensionMinder;
using MongoDB.Bson;
using Serilog;
using Vita.Domain.Companies;

namespace Vita.Domain.Infrastructure
{
    public enum LogLevel
    {
        None = 0,
        Fatal,
        Error,
        Warning,
        Information,
        Debug,
        Verbose
    }

    public static class Logger
    {
        public static void LogMessage(
            LogLevel logLevel,
            string message,
            [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "",
            object id = null)
        {
            var method = ToString(id) + " : " + GetClassMethodName(sourceFilePath, memberName);
            switch (logLevel)
            {
                case LogLevel.None:
                    break;
                case LogLevel.Fatal:
                    Log.Fatal("{method}: {message}", method, message);
                    break;
                case LogLevel.Error:
                    Log.Error("{method}: {message}", method, message);
                    break;
                case LogLevel.Warning:
                    Log.Warning("{method}: {message}", method, message);
                    break;
                case LogLevel.Information:
                    Log.Information("{method}: {message}", method, message);
                    break;
                case LogLevel.Debug:
                    Log.Debug("{method}: {message}", method, message);
                    break;
                case LogLevel.Verbose:
                    Log.Verbose("{method}: {message}", method, message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }

        #region general implementations - debug, information, warning, error

        public static void Exception(ICommand command, Exception exception, string id = "", string message = "")
        {
            Log.Error(exception, "command exception: {name} {command} {id} {message}", command.GetType().Name, command,
                id, message);
        }

        public static void Ex(Exception exception, object id, string message,
            [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "")
        {
            Log.Error(exception, "exception: {message} {id} {sourceFilePath} {memberName}", message, ToString(id),
                sourceFilePath, memberName);
        }

        public static void Warning(string message, object id = null, Exception ex = null,
            [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "")
        {
            var method = GetClassMethodName(sourceFilePath, memberName);
            Log.Warning(ex, "warning: {message} {id} {method}", message, ToString(id), method);
        }

        public static void Warning(string template, string message, object id = null, Exception ex = null,
            [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "")
        {
            var method = GetClassMethodName(sourceFilePath, memberName);
            Log.Warning(ex, template, message, ToString(id), method);
        }

        public static void Error<T>(T ex, object id = null, object parameter = null,
            [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "") where T : Exception
        {
            var method = GetClassMethodName(sourceFilePath, memberName);
            var description = GetParameterDescription(id, parameter);
            Log.Error(ex, $"error {method} : {description}");
            Trace.TraceError($"error {method} : {description}");
        }

        public static void Error<T>(T ex, string message, object id = null, [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "") where T : Exception
        {
            Log.Error(ex, $"error {id} {message}", id, message);
            Trace.TraceError($"error {message} : {ex.Message}");
        }

        public static void Information(string message, object id = null, object parameter = null,
            [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "")
        {
            var method = GetClassMethodName(sourceFilePath, memberName);
            var description = GetParameterDescription(ToString(id), parameter);
            Log.Information("{0} {1} {2}", message, description, method);
        }

        public static void Debug(string message, object id = null, object parameter = null,
            [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "")
        {
            var method = GetClassMethodName(sourceFilePath, memberName);
            var description = GetParameterDescription(ToString(id), parameter);
            Log.Debug("debug {0} {1} {2}", message, description, method);
        }

        public static T Verbose<T>(string message, Func<T> func, object id = null, object parameter = null,
            [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "")
        {
            var method = GetClassMethodName(sourceFilePath, memberName);
            var description = GetParameterDescription(ToString(id), parameter);
            Log.Verbose("start {0} {1} {2}", message, description, method);
            var result = func();
            Log.Verbose("end {0} {1} {2}", message, description, method);
            return result;
        }

        public static void Verbose(Action action, object id = null, object parameter = null,
            [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "")
        {
            var method = GetClassMethodName(sourceFilePath, memberName);
            var description = GetParameterDescription(ToString(id), parameter);
            Log.Verbose("start {0} {1} {2}", description, method);
            action();
            Log.Verbose("end {0} {1} {2}", description, method);
        }

        public static void Verbose(string message, [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "")
        {
            Log.Verbose("{verbose}", $"{message} {sourceFilePath} {memberName}");
        }


        public static void Query<T>(IIdentity<T> query, string message)
        {
            Log.Debug("query: {query} {message}", $"{query}", message);
        }

        #endregion

        #region start stop steps

        public static void Start(object id = null, object parameter = null, [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "")
        {
            var method = GetClassMethodName(sourceFilePath, memberName);
            var description = GetParameterDescription(ToString(id), parameter);
            Log.Debug("start {method} : {description}", method, description);
        }

        public static void Step(string message, object id = null, [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "")
        {
            var method = GetClassMethodName(sourceFilePath, memberName);
            Log.Debug("step: {method} : {message} : {id}", method, message, ToString(id));
        }

        public static void Done(string message, object id = null, [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "")
        {
            var method = GetClassMethodName(sourceFilePath, memberName);
            Log.Debug($"done {method} : {ToString(id)}");
        }

        #endregion

        #region actions functions

        public static void Try(string message, Action act, bool errorAlert = false,
            [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "")
        {
            try
            {
                act();
                Log.Debug("try success: {message}", message);
            }
            catch (Exception ex)
            {
                if (errorAlert)
                    Log.Error(ex, "try failed: {message} {sourceFilePath} {memberName}", message, sourceFilePath,
                        memberName);
                else
                    Log.Warning(ex, "try failed: {message} {sourceFilePath} {memberName}", message, sourceFilePath,
                        memberName);

                throw;
            }
        }

        #endregion

        #region commands events

        public static void Command(string message, object command = null, [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "")
        {
            var method = GetClassMethodName(sourceFilePath, memberName);
            Log.Information("command: {method} : {message} : {command}", method, message, command);
        }

        public static void Event(IDomainEvent @event, string message = null,
            [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "")
        {
            Log.Information("event: {name} {message} {event} {sourceFilePath} {memberName}", @event.EventType.Name,
                message?.ToLowerInvariant(), @event.ToJson(), sourceFilePath, memberName);
        }


        #endregion

        private static string GetParameterDescription(object id, object parameter)
        {
            return $"{ToString(id)} : {ToString(parameter)}";
        }

        public static object ToString(object obj)
        {
            if (obj != null)
            {
                if (obj is string) return obj.ToString();
                obj = (obj as Enum)?.GetDescription() ?? obj.GetType().Name;
                return obj;
            }

            return string.Empty;
        }

        public static string GetClassMethodName(string sourceFilePath, string memberName)
        {
            var methodName = string.IsNullOrEmpty(sourceFilePath)
                ? memberName
                : $"{Path.GetFileNameWithoutExtension(sourceFilePath)}.{memberName}";
            return methodName;
        }
    }
}