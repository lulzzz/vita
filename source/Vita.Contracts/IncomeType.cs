using System.ComponentModel;

namespace Vita.Contracts
{
    public enum IncomeType
    {
        [Description("None")]
        None = 0,

        [Description("Full Time Work")]
        FullTimeWork = 1,

        [Description("Part Time Work")]
        PartTimeWork = 2,

        [Description("Casual Work")]
        CasualWork = 3,

        [Description("Self Employed")]
        SelfEmployed = 4,

        [Description("Centrelink")]
        Centrelink = 5,

        [Description("Other")]
        Other = 6
    }
}