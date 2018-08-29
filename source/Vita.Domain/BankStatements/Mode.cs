using System;
using Vita.Domain.Infrastructure;

namespace Vita.Domain.BankStatements
{
    public static class Mode
    {
        public static class Test
        {
            public static string ApiUrl =>Environment.GetEnvironmentVariable("bankstatements-service-apiurl");
            public static string ApiKey => Environment.GetEnvironmentVariable("bankstatements-service-apikey");
            public static string Prefix => Environment.GetEnvironmentVariable("bankstatements-service-prefix");
        }
    }
}
