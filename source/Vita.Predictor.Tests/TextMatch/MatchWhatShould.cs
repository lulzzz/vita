using System.Threading.Tasks;
using FluentAssertions;
using Vita.Contracts;
using Xunit;

namespace Vita.Predictor.Tests.TextMatch
{
    public class MatchWhoShould : MatchShouldBase
    {
        public MatchWhoShould(DataFixture dataFixture) : base(dataFixture)
        {
        }

        [Theory]
        [InlineData("Duncraig red rooster", TransactionType.Unknown)]
        [InlineData("Gymea Crust Pizza", TransactionType.Unknown)]
        [InlineData("ASDFASDFSS", TransactionType.Unknown)]
        [InlineData("eftpos TOBACCO STATION KARI KARINGAL VIC", TransactionType.Debit)]
        [InlineData("TRANSFER 18/11 11:30 COLES 4419", TransactionType.Transfer)]
        [InlineData("card reversal", TransactionType.Reversal)]
        public async Task Match_what(string sentence, TransactionType? tt)
        {
            var result = await Matcher.Match(sentence, false, true);
            result.TransactionType.Should().Be(tt);
        }

    }
}
