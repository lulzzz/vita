using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Vita.Domain.Charges;
using Vita.Domain.Charges.Commands;
using Xunit;

namespace Vita.Domain.Tests.Charges.Commands
{
    public class ImportChargesCommandHandlerShould
    {
        [Fact]
        public async Task Import_csv_file()
        {
            ImportChargesCommandHandler handler = new ImportChargesCommandHandler();
            var spy = new SpyChargeAggregate(ChargeId.New);
            var cmd = new ImportChargesCommand();
            await handler.ExecuteAsync(spy, cmd, CancellationToken.None);

            spy.ImportedData.Count().Should().NotBe(0);
        }
    }
}
