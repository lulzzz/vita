using System;
using System.Collections.Generic;
using GoogleApi.Entities.Common.Enums;
using Microsoft.Azure.Search;

namespace Vita.Contracts
{
    public class Place : Tracking
    {
      [System.ComponentModel.DataAnnotations.Key]
      public Guid Id { get; set; }

      [IsSearchable]
      public virtual string PlaceId { get; set; }

      [IsSearchable]
      public virtual string Name { get; set; }

      [IsSearchable]
      public virtual string FormattedAddress { get; set; }

    [IsSearchable]
      public virtual IEnumerable<PlaceLocationType?> Types { get; set; }
  }
}
