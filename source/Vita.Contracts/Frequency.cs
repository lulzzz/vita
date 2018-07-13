using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Vita.Contracts
{
    public enum Frequency
    {
        [Description("Weekly")] Weekly = 0,
        [Description("Fortnightly")] Fortnightly = 1,
        [Description("Monthly")] Monthly = 2,
        [Description("Annually")] Annually = 3
    }
}
