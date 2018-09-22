using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vita.Domain.Charges;
using Vita.Domain.Charges.Commands;

namespace Vita.Domain.Tests.Charges
{
    public class SpyChargeAggregate : ChargeAggregate
    {

        public IEnumerable<ImportChargesCommandHandler.ImportedCharge> ImportedData { get; set; }

        public SpyChargeAggregate(ChargeId id) : base(id)
        {

        }

        public override async Task ImportCharges(IEnumerable<ImportChargesCommandHandler.ImportedCharge> data)
        {
            var importedCharges = data as ImportChargesCommandHandler.ImportedCharge[] ?? data.ToArray();
            ImportedData = importedCharges;
            await base.ImportCharges(importedCharges);
        }
    }
}
