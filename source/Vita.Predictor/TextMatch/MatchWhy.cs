using System.Linq;
using ExtensionMinder;
using Vita.Contracts;

namespace Vita.Predictor.TextMatch
{
    public class MatchWhy : MatchBase, IMatchWhy
    {
        public MatchWhy(IRepository<Company> companies, IRepository<Locality> localities,
            IRepository<Classifier> classifiers) : base(companies, localities, classifiers)
        {
        }

        public Classifier Why(string sentence)
        {
            CreateNgrams(sentence);
            Classifier found;

            if (WordsCleaned.Count >= 4)
            {
                found = SearchNgrams(4);
                if (found != null) return found;
            }

            if (WordsCleaned.Count > 3)
            {
                found = SearchNgrams(3);
                if (found != null) return found;
            }

            if (WordsCleaned.Count > 2)
            {
                found = SearchNgrams(2);
                if (found != null) return found;
            }

            return (from word in Words.Where(x => x.Length > 1).Select(x => x.RemoveJunkWordsFromNumber())
                from cla in ClassifierCache
                from key in cla.Keywords.Select(x => x.ToLowerInvariant())
                where key == word
                select cla).FirstOrDefault();
        }
    }
}