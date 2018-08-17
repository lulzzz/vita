using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vita.Contracts
{
  public interface ITextClassifier
  {
    Task<TextClassificationResult> Match(string sentence);
    Task<IEnumerable<TextClassificationResult>> MatchMany(string sentence);
    IDictionary<int, IEnumerable<string>> CreateNgrams(string arg = null);
    void FlushCache();
    bool UseCache { get; set; }
  }
}