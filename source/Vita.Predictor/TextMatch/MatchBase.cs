using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionMinder;
using Serilog;
using Vita.Contracts;
using Vita.Domain.Infrastructure;

namespace Vita.Predictor.TextMatch
{
    public abstract class MatchBase
    {
        public IDictionary<int, IEnumerable<string>> Ngrams { get; set; }
        public bool UseCache { get; set; } = true;

        public IEnumerable<Classifier> ClassifierCache;
        public IEnumerable<Company> CompanyCache;
        public IEnumerable<Locality> LocalityCache;

        public static readonly IEnumerable<AustralianState> AustralianStates =
            AustralianState.NSW.GetAllItems<AustralianState>();

        protected IRepository<Classifier> Classifiers;
        protected IRepository<Company> Companies;
        protected IRepository<Locality> Localities;
        protected IList<TextClassificationResult> _results;
        protected string Sentence;
        protected IList<string> Words;
        protected bool Exact;
        protected List<string> WordsCleaned;

        protected MatchBase(IRepository<Company> companies, IRepository<Locality> localities,
            IRepository<Classifier> classifiers)
        {
            Classifiers = classifiers;
            Localities = localities;
            Companies = companies;
            AsyncUtil.RunSync(Init);
        }

        public void CreateNgrams(string arg = null)
        {
            if (!string.IsNullOrEmpty(arg)) Sentence = arg;

            Log.Verbose("text match {ngram} {text}", Sentence);

            Words = Sentence.SplitSentenceIntoWords()
                .ToList()
                .ConvertAll(x => x.ToLowerInvariantWithOutSpaces())
                .Where(x => !x.IsGibberish())
                .ToList();

            WordsCleaned = Words.Select(x => x.ToAlphaNumericOnly()).Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            var text = string.Join(" ", WordsCleaned);

            Ngrams = new ConcurrentDictionary<int, IEnumerable<string>>();
            if (WordsCleaned.Count > 3)
                Ngrams.Add(new KeyValuePair<int, IEnumerable<string>>(4, NGramProcessor.MakeNgrams(text, 4)));

            if (WordsCleaned.Count > 2)
                Ngrams.Add(new KeyValuePair<int, IEnumerable<string>>(3, NGramProcessor.MakeNgrams(text, 3)));

            if (WordsCleaned.Count > 1)
                Ngrams.Add(new KeyValuePair<int, IEnumerable<string>>(2, NGramProcessor.MakeNgrams(text, 2)));

            Ngrams.Add(new KeyValuePair<int, IEnumerable<string>>(1, WordsCleaned));

        }

        private async Task Init()
        {
            //  _companyCache = await Cacher.GetOrSetAsync<IEnumerable<Company>>("companies", () => Task.FromResult(_companies.GetAll()));
            if (!UseCache)
            {
                LocalityCache = Localities.GetAll();
                ClassifierCache = Classifiers.GetAll();
            }
            else
            {
                LocalityCache =
                    await Cacher.GetAsync("localities", () => Task.FromResult(Localities.GetAll()));
                ClassifierCache =
                    await Cacher.GetAsync("classifiers",
                        () => Task.FromResult(Classifiers.GetAll()));
            }
        }

        public void FlushCache()
        {
            if (Cacher.Exists("localities")) Cacher.Remove("localities");
            if (Cacher.Exists("classifiers")) Cacher.Remove("classifiers");
        }

        protected Classifier SearchNgrams(int ngramindex)
        {
            Classifier found = null;

            Parallel.ForEach(Ngrams[ngramindex], (ngram,state) =>
            {
                found = ClassifierCache.FirstOrDefault(Exact ? MatchExact(ngram) : MatchSome(ngram));
                if (found == null) return;
                Log.Verbose("text match MatchExact {ngram} {text}", ngramindex, ngram);
                state.Break();
            });

            if (found != null) return found;

            Parallel.ForEach(Ngrams[ngramindex], (ngram,state) =>
            {
                Log.Verbose("text match {ngram} {text}", ngramindex, ngram);
                found = ClassifierCache.FirstOrDefault(MatchSome(ngram));
                if (found == null) return;
                Log.Verbose("text match MatchSome {ngram} {text}", ngramindex, ngram);
                state.Break();
            });
          
            if (found == null)Log.Warning("text match NotFound {ngram} {text}", ngramindex);
            return found;
        }

        protected static Func<Classifier, bool> MatchSome(string text)
        {
            return classifier => classifier.Keywords.Any(x => x.Contains(text.Trim()));
        }

        protected static Func<Classifier, bool> MatchExact(string text)
        {
            return classifier =>
                classifier.Keywords.Any(x => string.Equals(x.Trim(), text.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }

    }
}
