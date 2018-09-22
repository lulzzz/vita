using System;

namespace Vita.Contracts
{
    public class PredictionRequest
    {
        public Guid Id { get; private set; }
        public string Description { get; set; }
        public string Bank { get; set; }

        public double Amount { get; set; }

        //public string AccountName { get; set; }
        //public string Notes { get; set; }
        //public string Tags { get; set; }
        public AccountType? AccountType { get; set; }

        //public string AccountNumber { get; set; }
        public DateTime TransactionUtcDate { get; set; }

        public PredictionRequest()
        {
            this.Id = Guid.NewGuid();
        }
    }
}