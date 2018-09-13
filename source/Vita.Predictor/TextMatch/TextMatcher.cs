using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ExtensionMinder;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using Serilog;
using Vita.Contracts;
using Vita.Contracts.SubCategories;
using Vita.Domain.Infrastructure;
using Vita.Predictor.TextClassifiers;

namespace Vita.Predictor.TextMatch
{
    public class TextMatcher : ITextClassifier, IMatchWho, IMatchWhat, IMatchWhere, IMatchWhen,
        IMatchWhy, IMatchHow
    {
        public static readonly IEnumerable<AustralianState> AustralianStates =
            AustralianState.NSW.GetAllItems<AustralianState>();

        private readonly IRepository<Classifier> _classifiers;
        private readonly IRepository<Company> _companies;
        private readonly IRepository<Locality> _localities;

        public IEnumerable<Classifier> ClassifierCache;
        public IEnumerable<Company> CompanyCache;
        public IEnumerable<Locality> LocalityCache;

        private IList<TextClassificationResult> _results;
        private string _sentence;

        private IList<string> _words;

        private bool _exact;
        private List<string> _wordsCleaned;

        public TextMatcher(IRepository<Company> companies, IRepository<Locality> localities,
            IRepository<Classifier> classifiers)
        {
            _companies = companies;
            _localities = localities;
            _classifiers = classifiers;
        }

        public IDictionary<int, IEnumerable<string>> Ngrams { get; set; }

        public async Task<TextClassificationResult> Match(string sentence, bool classifyOnly = true, bool exact = true)
        {
            if (string.IsNullOrWhiteSpace(sentence)) return null;
            _sentence = sentence;
            _exact = exact;
            await Init();

            var result = GetResult(classifyOnly);

            // fixes
            if (!result.HasResult()) Log.Warning("textclassifier not found {text}", sentence);

            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<TextClassificationResult>> MatchMany(string sentence, bool whyOnly = true)
        {
            if (string.IsNullOrWhiteSpace(sentence)) return null;
            _sentence = sentence.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(_sentence)) return null;
            await Init();

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

        public void FlushCache()
        {
            if (Cacher.Exists("localities")) Cacher.Remove("localities");
            if (Cacher.Exists("classifiers")) Cacher.Remove("classifiers");
        }

        public bool UseCache { get; set; } = true;

        public IDictionary<int, IEnumerable<string>> CreateNgrams(string arg = null)
        {
            if (!string.IsNullOrEmpty(arg)) _sentence = arg;

            Log.Verbose("text classifier {ngram} {text}", _sentence);

            _words = _sentence.SplitSentenceIntoWords()
                .ToList()
                .ConvertAll(x => x.ToLowerInvariantWithOutSpaces())
                .Where(x => !x.IsGibberish())
                .ToList();

            _wordsCleaned = _words.Select(x => x.ToAlphaNumericOnly()).Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            var text = string.Join(" ", _wordsCleaned);

            Ngrams = new ConcurrentDictionary<int, IEnumerable<string>>();
            if (_wordsCleaned.Count > 3)
                Ngrams.Add(new KeyValuePair<int, IEnumerable<string>>(4, NGramProcessor.MakeNgrams(text, 4)));

            if (_wordsCleaned.Count > 2)
                Ngrams.Add(new KeyValuePair<int, IEnumerable<string>>(3, NGramProcessor.MakeNgrams(text, 3)));

            if (_wordsCleaned.Count > 1)
                Ngrams.Add(new KeyValuePair<int, IEnumerable<string>>(2, NGramProcessor.MakeNgrams(text, 2)));

            return Ngrams;
        }

        private async Task Init()
        {
            Ngrams = CreateNgrams();
            //  _companyCache = await Cacher.GetOrSetAsync<IEnumerable<Company>>("companies", () => Task.FromResult(_companies.GetAll()));
            if (!UseCache)
            {
                LocalityCache = _localities.GetAll();
                ClassifierCache = _classifiers.GetAll();
            }
            else
            {
                LocalityCache =
                    await Cacher.GetAsync("localities", () => Task.FromResult(_localities.GetAll()));
                ClassifierCache =
                    await Cacher.GetAsync("classifiers",
                        () => Task.FromResult(_classifiers.GetAll()));
            }
        }

        private TextClassificationResult GetResult(bool classifyOnly = true)
        {
            if (_results == null || !_results.Any()) _results = new List<TextClassificationResult>();

            var result = new TextClassificationResult {Ngrams = Ngrams};

            result.SearchPhrase = _sentence;
            // why      
            result.Classifier = Why(_sentence);

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
                result.TransactionDate = When(_sentence);

                // who
                result.Company = Who(_sentence);

                //how
                result.PaymentMethodType = How(_sentence); //needs to be before what

                // what
                result.TransactionType = What(result, _sentence);

                // where
                result.Locality = Where(_sentence); // may only FirstOrDefault a single postcode or suburb      
            }

            return result;
        }


        public DateTime? When(string text)
        {
            // Get DateTime for the specified culture
            var results =
                DateTimeRecognizer.RecognizeDateTime(_sentence, Culture.English, DateTimeOptions.ExtendedTypes);

            // Check there are valid results
            if (results.Count > 0 && results.First().TypeName.StartsWith("datetimeV2"))
            {
                // The DateTime model can return several resolution types (https://github.com/Microsoft/Recognizers-Text/blob/master/.NET/Microsoft.Recognizers.Text.DateTime/Constants.cs#L7-L15)
                // We only care for those with a date, date and time, or date time period:
                // date, daterange, datetime, datetimerange

                var first = results.First();
                var resolutionValues = (IList<Dictionary<string, string>>) first.Resolution["values"];

                var subType = first.TypeName.Split('.').Last();
                if (subType.Contains("date") && !subType.Contains("range"))
                {
                    // a date (or date & time) or multiple
                    var moment = resolutionValues.Select(v => v["value"].ToAustralianDate()).FirstOrDefault();
                    return moment;
                }

                if (subType.Contains("date") && subType.Contains("range") &&
                    resolutionValues.Any(x => x.ContainsKey("start")))
                {
                    // range
                    var from = resolutionValues.First()["start"].ToAustralianDate();
                    //var to = DateTime.Parse(resolutionValues.First()["end"]);
                    return from;
                }

                if (subType.Contains("time"))
                {
                    Console.WriteLine(subType);
                    var moment = resolutionValues.Select(v => v["value"].ToAustralianDate()).FirstOrDefault();
                    return moment;
                }
            }

            return null;
        }

        private static Func<Classifier, bool> MatchSome(string text)
        {
            return classifier => classifier.Keywords.Any(x => x.Contains(text));
        }

        private static Func<Classifier, bool> MatchExact(string text)
        {
            return classifier =>
                classifier.Keywords.Any(x => string.Equals(x, text, StringComparison.CurrentCultureIgnoreCase));
        }

        public Classifier Why(string text)
        {
            Classifier found;

            if (_wordsCleaned.Count >= 4)
            {
                found = SearchNgrams(4);
                if (found != null) return found;
            }

            if (_wordsCleaned.Count > 3)
            {
                found = SearchNgrams(3);
                if (found != null) return found;
            }

            if (_wordsCleaned.Count > 2)
            {
                found = SearchNgrams(2);
                if (found != null) return found;
            }

            return (from word in _words.Where(x => x.Length > 1).Select(x => x.RemoveJunkWordsFromNumber())
                from cla in ClassifierCache
                from key in cla.Keywords.Select(x => x.ToLowerInvariant())
                where key == word
                select cla).FirstOrDefault();
        }


        private Classifier SearchNgrams(int ngramindex)
        {
            foreach (var ngram in Ngrams[ngramindex])
            {
                Log.Verbose("text classifier {ngram} {text}", ngramindex, ngram);
                var found = ClassifierCache.FirstOrDefault(_exact ? MatchExact(ngram) : MatchSome(ngram));

                if (found != null) return found;

                found = ClassifierCache.FirstOrDefault(MatchSome(ngram));

                if (found != null) return found;
            }

            return null;
        }

        public Company Who(string sentence = null)
        {
            //Debug.Assert(_companies.GetAll().Count()>1);
            var found = _companies.Find(SearchCriteriaCompany(sentence)).ToArray();
            if (found.Any()) return found.FirstOrDefault();
            if (_wordsCleaned.Count > 3)
                foreach (var ngram in Ngrams[4])
                {
                    Log.Verbose("text classifier {ngram} {text}", 4, ngram);
                    found = _companies.Find(SearchCriteriaCompany(ngram)).ToArray();
                    if (found.Any()) return found.FirstOrDefault();
                }

            if (_wordsCleaned.Count > 2)
                foreach (var ngram in Ngrams[3])
                {
                    Log.Verbose("text classifier {ngram} {text}", 3, ngram);
                    found = _companies.Find(SearchCriteriaCompany(ngram)).ToArray();
                    if (found.Any()) return found.FirstOrDefault();
                }

            if (_wordsCleaned.Count > 1)
                foreach (var ngram in Ngrams[2])
                {
                    Log.Verbose("text classifier {ngram} {text}", 2, ngram);
                    found = _companies.Find(SearchCriteriaCompany(ngram)).ToArray();
                    if (found.Any()) return found.FirstOrDefault();
                }

            foreach (var word in _wordsCleaned)
            {
                found = _companies.Find(SearchCriteriaCompany(word)).ToArray();
                if (found.Any()) return found.FirstOrDefault();
            }

            return null;
        }

        private static Expression<Func<Company, bool>> SearchCriteriaCompany(string text)
        {
            return company => company.CompanyName.ToLowerInvariant().Contains(text.ToLowerInvariant()) ||
                              !string.IsNullOrWhiteSpace(company.CurrentName) &&
                              company.CurrentName.Contains(text.ToLowerInvariant());
        }

        public TransactionType? What(TextClassificationResult result, string sentence)
        {
            AsyncUtil.RunSync(() => Init());
            if (result.Classifier == null) return TransactionType.Unknown;

            switch (result.Classifier.SubCategory)
            {
                case Categories.TransferringMoney.CreditCard:
                case Categories.BankingFinance.CreditCardPayments:
                    return TransactionType.Credit;
                case Categories.TransferringMoney.OtherTransferringMoney:
                case Categories.BankingFinance.OtherBankingFinance:
                    return TransactionType.Transfer;
                case Categories.BankingFinance.Fees:
                    return TransactionType.Fees;
                case Categories.BankingFinance.Interest:
                    return TransactionType.Interest;
                case Categories.BankingFinance.Reversal:
                    return TransactionType.Reversal;
                case Categories.BankingFinance.LoanRepayments:
                    return TransactionType.Repayments;
                case Categories.BankingFinance.Overdrawn:
                    return TransactionType.Overdrawn;
            }

            switch (result.PaymentMethodType)
            {
                case PaymentMethodType.DirectDebit:
                case PaymentMethodType.Eftpos:
                    return TransactionType.Debit;
                case PaymentMethodType.CreditCard:
                    return TransactionType.Credit;
            }

            foreach (var word in _wordsCleaned)
                switch (word.Trim())
                {
                    case "eftpos":
                    case "eftpost":
                        return TransactionType.Debit;
                    case "fee":
                        return TransactionType.Fees;
                    case "transfer":
                    case "exchange":
                    case "trnsfr":
                        return TransactionType.Transfer;
                }

            return TransactionType.Unknown;
        }


        public Locality Where(string text)
        {
            AsyncUtil.RunSync(Init);
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

        public PaymentMethodType How(string sentence)
        {
            AsyncUtil.RunSync(Init);

            foreach (var word in _wordsCleaned)
                switch (word.Trim())
                {
                    case "eftpos":
                    case "eftpost":
                        return PaymentMethodType.Eftpos;
                    case "withdraw":
                    case "advance":
                    case "cashwithdraw":
                    case "cashdraw":
                    case "cash":
                        return PaymentMethodType.CashWithdrawl;
                    case "directdebit":
                    case "debit":
                        return PaymentMethodType.DirectDebit;
                    case "creditcard":
                        return PaymentMethodType.CreditCard;
                }

            return PaymentMethodType.Unknown;
        }

        private string FindSuburb()
        {
            return (from word in _wordsCleaned
                from sub in LocalityCache.Where(x => !string.IsNullOrWhiteSpace(x.Suburb))
                where sub.Suburb.Trim().ToLowerInvariant() == word
                select sub.Suburb).FirstOrDefault();
        }

        private string FindPostCode()
        {
            return (from word in _wordsCleaned
                from sub in LocalityCache.Where(x => !string.IsNullOrWhiteSpace(x.Postcode))
                where sub.Postcode.Trim().ToLowerInvariant() == word
                select sub.Postcode).FirstOrDefault();
        }

        private AustralianState? FindAustralianState()
        {
            foreach (var word in _wordsCleaned)
            foreach (var state in AustralianStates)
            {
                var st = state.ToString().ToUpperInvariant();
                if (word.Trim() == st) return st.ToEnum<AustralianState>();
            }

            return null;
        }
    }
}