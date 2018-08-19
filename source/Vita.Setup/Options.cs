using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace Vita.Setup
{
  public class Options
  {
    [Option('b', "build", Required = false, HelpText = "build database")]
    public bool BuildDatabase { get; set; }

    [Option('f', "flash", Required = false, HelpText = "flash database")]
    public bool FlashDatabase { get; set; }


    [Option('p', "purge", Required = false, HelpText = "purge database")]
    public bool PurgeDatabase { get; set; }
  }
}
