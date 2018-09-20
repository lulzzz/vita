using FluentAssertions;
using Vita.Contracts;
using Vita.Predictor.TextMatch;
using Xunit;

namespace Vita.Predictor.Tests.TextMatch
{
    public class MatchWhereShould : MatchShouldBase
    {
        public MatchWhereShould(DataFixture dataFixture) : base(dataFixture)
        {
        }

        [Theory]
        [InlineData("Duncraig red rooster", AustralianState.WA, "DUNCRAIG", "6023")]
        [InlineData("Gymea Crust Pizza", AustralianState.NSW, "GYMEA", "2227")]
        [InlineData("ASDFASDFSS", null, null, null)]
        [InlineData("EFTPOS TOBACCO STATION KARI KARINGAL VIC", AustralianState.VIC, "KARINGAL", "3199")]
        [InlineData("EFTPOS DEBIT 18/11 11:30 COLES 4419", AustralianState.SA, "COLES",
            "5272")] //https://en.wikipedia.org/wiki/Coles,_South_Australia
        public void Match_where(string sentence, AustralianState? australianState, string suburb,
            string postCode)
        {
            //var results = await Matcher.Match(sentence,false, exact:false);

            var where = new MatchWhere(DataFixture.Companies, DataFixture.Localities, DataFixture.Classifiers);
            var result = where.Where(sentence);

            //where did it take place
            result.AustralianState.Should().Be(australianState);
            result.Suburb.Should().Be(suburb);
            result.Postcode.Should().Be(postCode);
            //results.Any(x => x.AustralianState == australianState).Should().BeTrue();
            //results.Any(x => x.Suburb == suburb).Should().BeTrue();
            //results.Any(x => x.Postcode == postCode).Should().BeTrue();
        }
    }
}