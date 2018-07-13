using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Vita.Contracts
{
    public class BankLogin : ValueObject
    {
        public string Slug { get; }
        public Dictionary<string, string> Credentials { get; }

        [JsonConstructor]
        public BankLogin(string slug, Dictionary<string, string> credentials)
        {   
            Slug = slug;
            Credentials = credentials;
        }

        public BankLogin(string slug, params string[] credentialkeyValues)
        {
            
            if (credentialkeyValues.Length % 2 != 0)
            {
                throw new ArgumentException($"BankLogin credentialkeyValues should be key value pairs, therefore expecting even counts but found {credentialkeyValues.Length} elements");
            }
            Slug = slug;
            Credentials = new Dictionary<string, string>();
            for (var i = 0; i < credentialkeyValues.Length; i += 2)
            {
                Credentials.Add(credentialkeyValues[i], credentialkeyValues[i + 1]);
            }
        }
    }
}
