using System.ComponentModel;

namespace Vita.Contracts
{
    public enum ProductType
    {
        [Description("SEC")]
        SecuredLoans,
        [Description("USL")]
        UnsecuredLoans
    }
}