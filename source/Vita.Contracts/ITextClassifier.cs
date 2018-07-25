using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vita.Contracts
{
  public interface ITextClassifier
  {
    Task<TextClassificationResult> Match(string sentence);
    Task<IEnumerable<TextClassificationResult>> MatchMany(string sentence);
    IDictionary<int, IEnumerable<string>> CreateNgrams(string sentence);
    void FlushCache();
    bool UseCache { get; set; }
  }
}