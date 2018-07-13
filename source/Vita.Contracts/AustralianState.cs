using System.ComponentModel;

namespace Vita.Contracts
{
  public enum AustralianState
  {
    // ReSharper disable InconsistentNaming
    [Description("Australian Capital Territory")]
    ACT,

    [Description("New South Wales")]
    NSW,

    [Description("Northern Territory")]
    NT,

    [Description("Queensland")]
    QLD,

    [Description("South Australia")]
    SA,

    [Description("Tasmania")]
    TAS,

    [Description("Victoria")]
    VIC,

    [Description("Western Australia")]
    WA
    // ReSharper restore InconsistentNaming
  }
}
