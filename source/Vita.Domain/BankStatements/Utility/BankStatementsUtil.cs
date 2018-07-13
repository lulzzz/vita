using System;
using System.Collections.Generic;
using System.Linq;
using Vita.Domain.BankStatements.Login;
using Vita.Domain.BankStatements.Models;

namespace Vita.Domain.BankStatements.Utility
{
    public static class BankStatementsUtil
    {
        public const string BankStatementsInstitutionsList = "BanksStatements.Institutions";

        public static IDictionary<string, string> FormatCredentials(BankLogin bankLogin, IEnumerable<Institution> insitutions)
        {

            if (insitutions != null)
            {
                var bank = insitutions.Where(x => x.Slug.ToLowerInvariant() == bankLogin.Slug.ToLowerInvariant());
                var bankRequiredCredentials = bank.SelectMany(b => b.Credentials).ToList();
                var copy = bankLogin.Credentials.Where(x => !string.IsNullOrEmpty(x.Value)).ToList();
                foreach (var credential in copy)
                {
                    if (!string.IsNullOrEmpty(credential.Key) || (!string.IsNullOrEmpty(credential.Value)))
                    {
                        var fieldId =
                            bankRequiredCredentials.SingleOrDefault(
                                x => String.Equals(x.Name, credential.Key, StringComparison.InvariantCultureIgnoreCase));
                        if (fieldId != null)
                        {
                            // use the field id to send across to the bank
                            bankLogin.Credentials[fieldId.FieldId] = credential.Value;
                            // field ID needs to be passed over to bank statments
                        }
                        else
                        {
                            if (!bankLogin.Credentials.ContainsKey(credential.Key.ToLowerInvariant()))
                            {
                                bankLogin.Credentials.Add(credential.Key.ToLowerInvariant(), credential.Value);
                            }
                        }
                    }
                }
            }

            if (!bankLogin.Credentials.ContainsKey("institution"))
            {
                bankLogin.Credentials["institution"] = bankLogin.Slug;// needs this here for some reason
            }

            return bankLogin.Credentials;
        }
    }
}
