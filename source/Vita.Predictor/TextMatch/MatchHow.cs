using Vita.Contracts;

namespace Vita.Predictor.TextMatch
{
    public class MatchHow : MatchBase, IMatchHow
    {
        public MatchHow(IRepository<Company> companies, IRepository<Locality> localities,
            IRepository<Classifier> classifiers) : base(companies, localities, classifiers)
        {
        }

        public PaymentMethodType How(string sentence)
        {
            CreateNgrams(sentence);

            foreach (var word in WordsCleaned)
                switch (word.Trim())
                {
                    case "eftpos":
                    case "eftpost":
                        return PaymentMethodType.Eftpos;
                    case "withdraw":
                    case "advance":
                    case "cashwithdraw":
                    case "cashdraw":
                    case "cash":
                        return PaymentMethodType.CashWithdrawl;
                    case "directdebit":
                    case "debit":
                        return PaymentMethodType.DirectDebit;
                    case "creditcard":
                        return PaymentMethodType.CreditCard;
                }

            return PaymentMethodType.Unknown;
        }
    }
}