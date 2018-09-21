using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Vita.Contracts;

namespace Vita.Predictor.TextMatch
{
    public class TextMatcher : MatchBase, ITextClassifier, IMatchWho, IMatchWhat, IMatchWhere, IMatchWhen,
        IMatchWhy, IMatchHow
    {
        private readonly MatchWhy _matchWhy;
        private readonly MatchWho _matchWho;
        private readonly MatchWhen _matchWhen;
        private readonly MatchWhat _matchWhat;
        private readonly MatchWhere _matchWhere;
        private readonly MatchHow _matchHow;

        public TextMatcher(IRepository<Company> companies, IRepository<Locality> localities,
            IRepository<Classifier> classifiers) : base(companies, localities, classifiers)
        {
            _matchWhy = new MatchWhy(Companies, Localities, Classifiers);
            _matchWho = new MatchWho(Companies, Localities, Classifiers);
            _matchWhen = new MatchWhen(Companies, Localities, Classifiers);
            _matchWhat = new MatchWhat(Companies, Localities, Classifiers);
            _matchWhere = new MatchWhere(Companies, Localities, Classifiers);
            _matchHow = new MatchHow(Companies, Localities, Classifiers);
        }

        public async Task<TextClassificationResult> Match(string sentence, bool classifyOnly = true, bool exact = true)
        {
            if (Guard(sentence)) return null;
            Exact = exact;
            var result = GetResult(classifyOnly);

            // fixes
            if (!result.HasResult()) Log.Warning("textclassifier not found {text}", sentence);

            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<TextClassificationResult>> MatchMany(string sentence, bool whyOnly = true)
        {
            if (Guard(sentence)) return null;
            _results = new List<TextClassificationResult>();
            var iteractions = 1;
            var hasResult = false;
            do
            {
                var result = GetResult(_results.Any() && whyOnly);
                hasResult = result.HasResult();
                whyOnly = true;
                if (hasResult) _results.Add(result);
                iteractions++;
            } while (hasResult && iteractions < 3);

            return await Task.FromResult(_results);
        }

        private bool Guard(string sentence)
        {
            if (string.IsNullOrWhiteSpace(sentence)) return true;
            Sentence = sentence.ToLowerInvariant();
            CreateNgrams(sentence);
            return false;
        }

        private TextClassificationResult GetResult(bool classifyOnly = true)
        {
            if (_results == null || !_results.Any()) _results = new List<TextClassificationResult>();

            var result = new TextClassificationResult {Ngrams = Ngrams};

            result.SearchPhrase = Sentence;
            // why      
            result.Classifier = Why(Sentence);

            if (classifyOnly)
            {
                if (_results.Any())
                {
                    var first = _results.First();
                    result.TransactionDate = first.TransactionDate;
                    result.Company = first.Company;
                    result.TransactionType = first.TransactionType;
                    result.Locality = first.Locality;
                    result.PaymentMethodType = first.PaymentMethodType;
                }
            }
            else
            {
                // when 
                result.TransactionDate = When(Sentence);

                // who
                result.Company = Who(Sentence);

                //how
                result.PaymentMethodType = How(Sentence); //needs to be before what

                // what
                result.TransactionType = What(result, Sentence);

                // where
                result.Locality = Where(Sentence); // may only FirstOrDefault a single postcode or suburb      
            }

            return result;
        }

        public DateTime? When(string text)
        {
            return _matchWhen.When(text);
        }

        public Classifier Why(string text)
        {
            return _matchWhy.Why(text);
        }

        public Company Who(string sentence = null)
        {
            return _matchWho.Who(sentence);
        }

        public TransactionType? What(TextClassificationResult result, string sentence)
        {
            return _matchWhat.What(result, sentence);
        }


        public Locality Where(string text)
        {
            return _matchWhere.Where(text);
        }

        public PaymentMethodType How(string sentence)
        {
            return _matchHow.How(sentence);
        }
    }
}