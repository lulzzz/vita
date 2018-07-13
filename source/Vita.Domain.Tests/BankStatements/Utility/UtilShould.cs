using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Vita.Domain.BankStatements.Login;
using Vita.Domain.BankStatements.Models;
using Vita.Domain.BankStatements.Utility;
using Vita.Domain.Infrastructure;
using Xunit;

namespace Vita.Domain.Tests.BankStatements.Utility
{
    public class UtilShould
    {
        /// <summary>
        /// eg ANZ has 'customer_registration_number' --> this needs to be placed with username
        /// </summary>
        [Fact]
        public void Format_credentials_replaces_bank_username_field_with_banks_custom_field()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Vita.Domain.Tests.BankStatements.bankstatements-institutions.json";

            var names = assembly.GetManifestResourceNames();
            foreach(var name in names)
            {
                Console.WriteLine(name);
            }

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                var dic = new Dictionary<string, string> {{"customer registration number",  SecretMan.Get("bankstatements-cba-test-username") }, {"password",SecretMan.Get("bankstatements-cba-test-password")}};
                var login = new BankLogin("anz", dic);
                var data = JsonConvert.DeserializeObject<Institution[]>(result);
                var creds = BankStatementsUtil.FormatCredentials(login, data);
                Assert.True(creds.ContainsKey("username"));
            }
        }

        [Fact]
        public void Format_credentials_adds_institution()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Vita.Domain.Tests.BankStatements.bankstatements-institutions.json";

            var names = assembly.GetManifestResourceNames();
            foreach (var name in names)
            {
                Console.WriteLine(name);
            }

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                var dic = new Dictionary<string, string> { { "customer registration number", SecretMan.Get("bankstatements-cba-test-username") }, { "password", SecretMan.Get("bankstatements-cba-test-password") } };
                var login = new BankLogin("anz", dic);
                var data = JsonConvert.DeserializeObject<Institution[]>(result);
                var creds = BankStatementsUtil.FormatCredentials(login, data);
                Assert.True(creds.ContainsKey("institution"));
            }
        }
    }
}
