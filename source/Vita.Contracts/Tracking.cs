using System;
using Microsoft.Azure.Search;

namespace Vita.Contracts
{
    public abstract class Tracking
    {
      [IsSearchable, IsFilterable, IsSortable]
      public DateTime CreatedUtc { get; set; }
      [IsSearchable, IsFilterable, IsSortable]
      public DateTime? ModifiedUtc { get; set; }
    }
}
