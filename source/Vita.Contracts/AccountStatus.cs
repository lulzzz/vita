using System.ComponentModel;

namespace Vita.Contracts
{
    public enum AccountStatus
    {
        [Description("None")]
        None,
        [Description("Quote")]
        Quote,
        [Description("Open")]
        Open,
        [Description("Closed")]
        Closed,
        [Description("Unwanted")]
        Unwanted,
        [Description("ClosedPending")]
        ClosedPending,
        [Description("Declined")]
        Declined
    }
}