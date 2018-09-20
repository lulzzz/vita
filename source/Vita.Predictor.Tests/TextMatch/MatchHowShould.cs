using FluentAssertions;
using Vita.Contracts;
using Vita.Predictor.TextMatch;
using Xunit;

namespace Vita.Predictor.Tests.TextMatch
{
    public class MatchHowShould : MatchShouldBase
    {
        public MatchHowShould(DataFixture dataFixture) : base(dataFixture)
        {
        }

        [Theory]
        [InlineData("xxxx", PaymentMethodType.Unknown)]
        [InlineData("eftpos", PaymentMethodType.Eftpos)]
        [InlineData("withdraw", PaymentMethodType.CashWithdrawl)]
        [InlineData("advance", PaymentMethodType.CashWithdrawl)]
        public void Match_how(string sentence, PaymentMethodType pmt)
        {
            //var result = await Matcher.Match(sentence, false, true);
            var how = new MatchHow(DataFixture.Companies, DataFixture.Localities, DataFixture.Classifiers);
            var result = how.How(sentence);
            result.Should().Be(pmt);
        }
    }
}