using System;

namespace Vita.Contracts
{
    public class Agreement : ValueObject
    {
        public DateTimeOffset? PrivacyConsentAgreedTime { get; }

        public DateTimeOffset? BankStatementUsageTermsAgreedTime { get;  }

        public DateTimeOffset? ReferralConsentAgreedTime { get; }

        public DateTimeOffset? DvsConsentAgreedTime { get; }

        public Agreement(DateTimeOffset? privacyConsentAgreedTime,
            DateTimeOffset? bankStatementUsageTermsAgreedTime,
            DateTimeOffset? referralConsentAgreedTime,
            DateTimeOffset? dvsConsentAgreedTime)
        {
            PrivacyConsentAgreedTime = privacyConsentAgreedTime;
            BankStatementUsageTermsAgreedTime = bankStatementUsageTermsAgreedTime;
            ReferralConsentAgreedTime = referralConsentAgreedTime;
            DvsConsentAgreedTime = dvsConsentAgreedTime;
        }
    }
}