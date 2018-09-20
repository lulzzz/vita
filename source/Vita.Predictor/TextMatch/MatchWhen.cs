using System;
using System.Collections.Generic;
using System.Linq;
using ExtensionMinder;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using Vita.Contracts;

namespace Vita.Predictor.TextMatch
{
    public class MatchWhen : MatchBase, IMatchWhen
    {
        public MatchWhen(IRepository<Company> companies, IRepository<Locality> localities,
            IRepository<Classifier> classifiers) : base(companies, localities, classifiers)
        {
        }

        public DateTime? When(string sentence)
        {
            // Get DateTime for the specified culture
            var results =
                DateTimeRecognizer.RecognizeDateTime(sentence, Culture.English, DateTimeOptions.ExtendedTypes);

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
    }
}