using Vita.Contracts;
using Vita.Contracts.SubCategories;

namespace Vita.Predictor.TextMatch
{
    public class MatchWhat : MatchBase, IMatchWhat
    {
        public MatchWhat(IRepository<Company> companies, IRepository<Locality> localities,
            IRepository<Classifier> classifiers) : base(companies, localities, classifiers)
        {
        }

        public TransactionType? What(TextClassificationResult result, string sentence)
        {
            CreateNgrams(sentence);
            if (result.Classifier == null) return TransactionType.Unknown;

            switch (result.Classifier.SubCategory)
            {
                case Categories.TransferringMoney.CreditCard:
                case Categories.BankingFinance.CreditCardPayments:
                    return TransactionType.Credit;
                case Categories.TransferringMoney.OtherTransferringMoney:
                case Categories.BankingFinance.OtherBankingFinance:
                    return TransactionType.Transfer;
                case Categories.BankingFinance.Fees:
                    return TransactionType.Fees;
                case Categories.BankingFinance.Interest:
                    return TransactionType.Interest;
                case Categories.BankingFinance.Reversal:
                    return TransactionType.Reversal;
                case Categories.BankingFinance.LoanRepayments:
                    return TransactionType.Repayments;
                case Categories.BankingFinance.Overdrawn:
                    return TransactionType.Overdrawn;
            }

            switch (result.PaymentMethodType)
            {
                case PaymentMethodType.DirectDebit:
                case PaymentMethodType.Eftpos:
                    return TransactionType.Debit;
                case PaymentMethodType.CreditCard:
                    return TransactionType.Credit;
            }

            foreach (var word in WordsCleaned)
                switch (word.Trim())
                {
                    case "eftpos":
                    case "eftpost":
                        return TransactionType.Debit;
                    case "fee":
                        return TransactionType.Fees;
                    case "transfer":
                    case "exchange":
                    case "trnsfr":
                        return TransactionType.Transfer;
                }

            return TransactionType.Unknown;
        }
    }
}