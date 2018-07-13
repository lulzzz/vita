using System;

namespace Vita.Contracts
{
    public class LoanContract : Document
    {
        public Uri Uri { get; }

        public LoanContract(Uri uri)
        {
            Uri = uri;
        }
    }
}