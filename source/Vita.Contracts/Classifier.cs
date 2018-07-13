using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Search;

namespace Vita.Contracts
{
  public class Classifier
  {
    [Key] public Guid Id { get; set; }

    [IsSearchable]
    [IsFilterable]
    [IsSortable]
    [IsFacetable]
    public CategoryType CategoryType { get; set; }

    [IsSearchable]
    [IsSortable]
    public string SubCategory { get; set; }

    [IsSearchable] public HashSet<string> Keywords { get; set; }

    public void AddKeyword(string word)
    {
      if (string.IsNullOrWhiteSpace(word)) return;
      if (Keywords == null) Keywords = new HashSet<string>();
     
      word = word.Trim().ToLowerInvariant();
      if (!Keywords.Contains(word))
      {
        Keywords.Add(word);
      }
    }
  }
}