using System;
using System.Linq;
using System.Linq.Expressions;
using Serilog;
using Vita.Contracts;

namespace Vita.Predictor.TextMatch
{
    public class MatchWho : MatchBase, IMatchWho
    {
        public MatchWho(IRepository<Company> companies, IRepository<Locality> localities,
            IRepository<Classifier> classifiers) : base(companies, localities, classifiers)
        {
        }

        public Company Who(string sentence)
        {
            CreateNgrams(sentence);
            //Debug.Assert(_companies.GetAll().Count()>1);
            var found = Companies.Find(SearchCriteriaCompany(sentence)).ToArray();
            if (found.Any()) return found.FirstOrDefault();
            if (WordsCleaned.Count > 3)
                foreach (var ngram in Ngrams[4])
                {
                    Log.Verbose("text match {ngram} {text}", 4, ngram);
                    found = Companies.Find(SearchCriteriaCompany(ngram)).ToArray();
                    if (found.Any()) return found.FirstOrDefault();
                }

            if (WordsCleaned.Count > 2)
                foreach (var ngram in Ngrams[3])
                {
                    Log.Verbose("text match {ngram} {text}", 3, ngram);
                    found = Companies.Find(SearchCriteriaCompany(ngram)).ToArray();
                    if (found.Any()) return found.FirstOrDefault();
                }

            if (WordsCleaned.Count > 1)
                foreach (var ngram in Ngrams[2])
                {
                    Log.Verbose("text match {ngram} {text}", 2, ngram);
                    found = Companies.Find(SearchCriteriaCompany(ngram)).ToArray();
                    if (found.Any()) return found.FirstOrDefault();
                }

            foreach (var word in WordsCleaned)
            {
                found = Companies.Find(SearchCriteriaCompany(word)).ToArray();
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
    }
}