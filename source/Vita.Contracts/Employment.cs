using System;

namespace Vita.Contracts
{
    public class Employment : ValueObject
    {
        public IncomeType IncomeType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime NextPayDate { get; set; }
        public string Occupation { get; set; }
        public Employment(IncomeType incomeType, DateTime startDate, DateTime nextPayDate, string occupation)
        {
            IncomeType = incomeType;
            StartDate = startDate;
            NextPayDate = nextPayDate;
            Occupation = occupation;
        }
    }
}