using System.Linq;
using ExtensionMinder;
using Vita.Contracts;

namespace Vita.Predictor.TextMatch
{
    public class MatchWhere : MatchBase, IMatchWhere
    {
        public MatchWhere(IRepository<Company> companies, IRepository<Locality> localities,
            IRepository<Classifier> classifiers) : base(companies, localities, classifiers)
        {
        }

        public Locality Where(string sentence)
        {
            CreateNgrams(sentence);
            var locality = new Locality {Suburb = FindSuburb()};

            if (!string.IsNullOrWhiteSpace(locality.Suburb))
            {
                var exists = LocalityCache.FirstOrDefault(x => x.Suburb == locality.Suburb);
                if (exists != null) return exists;
            }

            locality.Postcode = FindPostCode();

            if (!string.IsNullOrWhiteSpace(locality.Postcode))
            {
                var exists = LocalityCache.FirstOrDefault(x => x.Postcode == locality.Postcode);
                if (exists != null) return exists;
            }

            if (!string.IsNullOrWhiteSpace(locality.Postcode))
                locality.AustralianState = FindAustralianState().GetValueOrDefault();

            return locality;
        }

        private string FindSuburb()
        {
            return (from word in WordsCleaned
                from sub in LocalityCache.Where(x => !string.IsNullOrWhiteSpace(x.Suburb))
                where sub.Suburb.Trim().ToLowerInvariant() == word
                select sub.Suburb).FirstOrDefault();
        }

        private string FindPostCode()
        {
            return (from word in WordsCleaned
                from sub in LocalityCache.Where(x => !string.IsNullOrWhiteSpace(x.Postcode))
                where sub.Postcode.Trim().ToLowerInvariant() == word
                select sub.Postcode).FirstOrDefault();
        }

        private AustralianState? FindAustralianState()
        {
            foreach (var word in WordsCleaned)
            foreach (var state in AustralianStates)
            {
                var st = state.ToString().ToUpperInvariant();
                if (word.Trim() == st) return st.ToEnum<AustralianState>();
            }

            return null;
        }
    }
}