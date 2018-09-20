using System;
using System.Globalization;
using FluentAssertions;
using Vita.Predictor.TextMatch;
using Xunit;

namespace Vita.Predictor.Tests.TextMatch
{
    public class MatchWhenShould : MatchShouldBase
    {
        public MatchWhenShould(DataFixture dataFixture) : base(dataFixture)
        {

        }

        [Theory]
        [InlineData("26 April 2017", "2017/04/26")]
        //[InlineData("EFTPOS DEBIT EFTPOS 08/12 14:20 MASTERS 7563", "2017-12-08 14:20:00")]
        [InlineData("PLEASE NOTE FROM 18 SEP 2015 YOUR DEBIT INT RATE IS 15.76%", "2015-09-18 00:00:00")]
        [InlineData("MISCELLANEOUS DEBIT V6870 24/10 VIDEO EZY EXPRESS SHEPPARTON 74564455299", "2017-10-24")]
        public void Match_when(string sentence, string date)
        {
            DateTime? dt = DateTime.Parse(date, CultureInfo.GetCultureInfo("en-AU"));
            var when = new MatchWhen(DataFixture.Companies,DataFixture.Localities, DataFixture.Classifiers);
            var result = when.When(sentence);
            result.Should().Be(dt);
        }
    }
}
