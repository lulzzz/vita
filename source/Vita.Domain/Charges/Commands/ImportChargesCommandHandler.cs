using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using EventFlow.Commands;
using ExtensionMinder;
using Vita.Contracts;

namespace Vita.Domain.Charges.Commands
{
    /// <summary>
    ///     see C:\dev\vita\scripts\prediction-model\data-builder.linq
    /// </summary>
    public class ImportChargesCommandHandler : CommandHandler<ChargeAggregate, ChargeId, ImportChargesCommand>
    {
        public override async Task ExecuteAsync(ChargeAggregate aggregate, ImportChargesCommand command,
            CancellationToken cancellationToken)
        {
            var assembly = Assembly.GetAssembly(typeof(ChargeAggregate));
            var t = typeof(ImportChargesCommand).Namespace;
            var resourcename = $"{t}.charges-import.csv";

            using (var stream = assembly.GetManifestResourceStream(resourcename))
            {
                if (stream == null) throw new ApplicationException("charges-import.csv");

                using (var sr = new StreamReader(stream))
                using (var csv = new CsvReader(sr))
                {
                    csv.Configuration.Delimiter = ",";
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.HasHeaderRecord = true;
                    csv.Configuration.IgnoreBlankLines = true;
                    // csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.TrimOptions = TrimOptions.Trim;

                    csv.Configuration.RegisterClassMap<ImportedChargeMap>();

                    var data = csv.GetRecords<ImportedCharge>().ToList();
                    Trace.WriteLine($"import charges {data.Count}");
                    await aggregate.ImportCharges(data);
                }
            }
        }

        public class ImportedChargeMap : ClassMap<ImportedCharge>
        {
            public ImportedChargeMap()
            {
                Map(m => m.Category).Index(0).ConvertUsing(ConvertExpression);
                Map(m => m.SubCategory).Index(1);
                Map(m => m.Description).Index(2);
                Map(m => m.Amount).Index(3);
                Map(m => m.TransactionUtcDateTime).Index(4);
            }

            private CategoryType ConvertExpression(IReaderRow arg)
            {
                return arg.GetField(0).ToEnum<CategoryType>();
            }
        }

        public class ImportedCharge
        {
            public CategoryType Category { get; set; }
            public string SubCategory { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public DateTime TransactionUtcDateTime { get; set; }
        }
    }
}