using Vita.Domain.Infrastructure;

namespace Vita.Domain.BankStatements
{
    public static class Mode
    {
        public static class Test
        {
            public static string ApiUrl => SecretMan.Get("bankstatements-service-apiurl-test");
            public static string ApiKey => SecretMan.Get("bankstatements-service-apikey-test");
            public static string Prefix => SecretMan.Get("bankstatements-service-prefix-test");
        }
    }
}
