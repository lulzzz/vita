using System.ComponentModel;

namespace Vita.Contracts
{
    public enum LoanReasonType
    {
        [Description("None")]
        None = 0,
        [Description("Cash Shortfall")]
        CashShortfall,
        [Description("Bond Loan")]
        BondLoan,
        [Description("Household Bills")]
        HouseholdBills,
        [Description("Travel Holiday")]
        TravelHoliday,
        [Description("Vehicle Purchase")]
        VehiclePurchase,
        [Description("Consolidate Debt")]
        ConsolidateDebt,
        [Description("Home Renovations")]
        HomeRenovations,
        [Description("Furniture Appliances")]
        FurnitureAppliances,
        [Description("Funeral Expenses")]
        FuneralExpenses,
        [Description("Wedding Expenses")]
        WeddingExpenses
    }
}