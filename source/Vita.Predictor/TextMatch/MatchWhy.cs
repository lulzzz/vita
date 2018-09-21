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


            found = SearchNgrams(1);

            return found;

        }
    }
}