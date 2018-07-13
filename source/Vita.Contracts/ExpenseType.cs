using System.ComponentModel;

namespace Vita.Contracts
{
    public enum ExpenseType
    {
        [Description("None")]
        None = 0,

        [Description("Personal Loan")]
        PersonalLoan = 1,

        [Description("Car Loan")]
        CarLoan = 2,

        [Description("Payday Loan")]
        PaydayLoan = 3,

        [Description("Child Care")]
        ChildCare = 4,

        [Description("Child Support")]
        ChildSupport = 5,

        [Description("Debt Agreement")]
        DebtAgreement = 6,

        [Description("Foxtel")]
        Foxtel = 7,

        [Description("Gym Membership")]
        GymMembership = 8,

        [Description("Insurances")]
        Insurances = 9,

        [Description("Penalty/Fine Payments")]
        PenaltyOrFinePayments = 10,

        [Description("Other Expenses")]
        OtherExpenses = 11,

        [Description("Living")]
        Living
    }
}