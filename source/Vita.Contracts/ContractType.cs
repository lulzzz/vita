using System.ComponentModel;

namespace Vita.Contracts
{
    public enum ContractType
    {
        [Description("Small Amount Credit Contract")]
        Sacc = 0,
        [Description("Medium Amount Credit Contract")]
        Macc = 1,
        [Description("Large Amount Credit Contract")]
        Lacc = 2
    }
}