namespace Vita.Domain.BankStatements
{
    public static class Mode
    {
        public static class Test 
        {
           public const string  ApiUrl = "https://test.bankstatements.com.au/api/v1";
           public const string  ApiKey = "                    # replace with the empty string";
           public const string  Prefix = "FGF";
        }

        public static class Prod
        {
           public const string  ApiUrl = "https://bankstatements.com.au/api/v1";
           public const string  ApiKey = "                    # replace with the empty string";
           public const string  Prefix = "FGF";
        }
    }
}
