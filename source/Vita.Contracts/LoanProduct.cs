namespace Vita.Contracts
{
    public class LoanProduct : ValueObject
    {
        public string ProductTypeId { get; private set; }
        public ProductType ProductType { get; private set; }

        public string AccountTypeId { get; private set; }
        public AccountType AccountType { get; private set; }

        public ContractType ContractType { get; private set; }
        public string DocumentTemplateId { get; private set; }

        public LoanProduct(decimal loanAmount, ClientType clientType)
        {
            var secured = loanAmount >= 3000 && clientType == ClientType.New;
            var mate = clientType == ClientType.Mate;
            SetContractType(loanAmount);
            SetProductType(secured);
            SetAccountType(mate, secured);
        }

        private void SetProductType(bool secured)
        {
            ProductTypeId = secured ? "SEC" : "USL";
            ProductType = secured ? ProductType.SecuredLoans : ProductType.UnsecuredLoans;
        }

        private void SetAccountType(bool mate, bool secured)
        {
            
        }

        private void SetContractType(decimal loanAmount)
        {
            if (loanAmount <= 2000)
            {
                ContractType = ContractType.Sacc;
                DocumentTemplateId = Constant.Document.SaccTemplate;
            }
            else if (loanAmount > 2000 && loanAmount <= 5000)
            {
                ContractType = ContractType.Macc;
                DocumentTemplateId = Constant.Document.MaccTemplate;
            }
            else if (loanAmount > 5000 && loanAmount <= 10000) {
                ContractType = ContractType.Lacc;
                DocumentTemplateId = Constant.Document.LaccTemplate;
            }
        }
    }
}
