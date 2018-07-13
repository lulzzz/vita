using System;
using Newtonsoft.Json;

namespace Vita.Contracts
{
    public class Error : ValueObject
    {
        public string Description { get; set; }
        public int Count { get; set; }
        public string Source { get; set; }
        public ErrorStack ErrorStack { get; set; }
        public Exception Exception { get; set; }
        public string ExceptionMessage { get; set; }
        public bool HoldError { get; set; }
        public bool IsFatal { get; set; }
        public string StackTrace { get; set; }

        public Error(string description,
            int count,
            string source,
            ErrorStack errorStack,
            Exception exception,
            string exceptionMessage,
            bool holdError,
            bool isFatal,
            string stackTrace)
        {
            Description = description;
            Count = count;
            Source = source;
            ErrorStack = errorStack;
            Exception = exception;
            ExceptionMessage = exceptionMessage;
            HoldError = holdError;
            IsFatal = isFatal;
            StackTrace = stackTrace;
        }
    }

    public class ErrorStack : ValueObject
    {
        public string ErrorType { get; set; }
        public ErrorStackItem[] Items { get; set; }

        public ErrorStack(string errorType, ErrorStackItem[] items)
        {
            ErrorType = errorType;
            Items = items;
        }
    }

    public class ErrorStackItem
    {
        public string Message { get; set; }

        public ErrorStackItem(string message)
        {
            Message = message;
        }
    }
}
