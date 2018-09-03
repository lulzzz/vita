using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vita.Contracts
{
  public interface ITextClassifier
  {
    Task<TextClassificationResult> Match(string sentence, bool classifyOnly=true);
    Task<IEnumerable<TextClassificationResult>> MatchMany(string sentence,bool classifyOnly=true);
    IDictionary<int, IEnumerable<string>> CreateNgrams(string arg = null);
    void FlushCache();
    bool UseCache { get; set; }
  }
}