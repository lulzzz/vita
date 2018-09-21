using FluentAssertions;
using Vita.Predictor.TextMatch;
using Xunit;

namespace Vita.Predictor.Tests.TextMatch
{
    public class MatchWhatShould : MatchShouldBase
    {

        [Theory]
        [InlineData("Duncraig red rooster", true, DataFixture.CompanyNames.RedRooster)]
        [InlineData("Gymea Crust Pizza", true, DataFixture.CompanyNames.CrustPizza)]
        [InlineData("XXXXXXXXXXXXXXXXXXXXXX", false, null)]
        [InlineData("EFTPOS TOBACCO STATION KARI KARINGAL VIC", true, "CompanyName1")]
        [InlineData("EFTPOS DEBIT EFTPOS 18/11 11:30 COLES 4419", true, DataFixture.CompanyNames.Coles)]
        [InlineData("coles 6018", true, DataFixture.CompanyNames.Coles)]
        public void Classify_who(string sentence, bool companyExists, string companyName = null)
        {
            //var result = await Matcher.Match(sentence, false, true);
            var who = new MatchWho(DataFixture.Companies,DataFixture.Localities, DataFixture.Classifiers);

            var result = who.Who(sentence);

            //who - what company,bank etc
            if (companyExists)
            {
                result.Should().NotBe(null);
                result.CompanyName.Should().Be(companyName);
            }
            else
            {
                result.Should().BeNull();
            }
        }

        public MatchWhatShould(DataFixture dataFixture) : base(dataFixture)
        {
        }
    }
}