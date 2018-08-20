using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using EventFlow;
using MediatR;
using Vita.Domain.Companies;
using Vita.Domain.Companies.Commands;
using Vita.Domain.Infrastructure;
using Vita.Predictor.TextClassifiers.SpreadSheets;

namespace Vita.Setup.SeedDatabase
{
    public class SeedHandler : IRequestHandler<SeedCommand, bool>
    {
        public async Task<bool> Handle(SeedCommand request, CancellationToken cancellationToken)
        {
            Consoler.TitleStart("seed database start");

            var companies = CompanySpreadsheet.Import();

            //var chance = new ChanceNET.Chance();
            //var companies = new List<Company> {chance.Object<Company>()};

            var bus = IocContainer.Container.Resolve<ICommandBus>();
          /*
    
    DRGD - De-registered.
    EXAD - External administration (in receivership/liquidation).
    NOAC - Not active.
    NRGD - Not registered.
    PROV - Provisional (mentioned only under charges and refers
    to those which have not been fully registered).
    REGD – Registered.
    SOFF - Strike-off action in progress.
    DISS - Dissolved by Special Act of Parliament.
    DIV3 - Organisation Transferred Registration via DIV3.
    PEND - Pending - Schemes.
    
    */
            foreach (var company in companies)//.Where(x=>x.Status =="REGD"))
            {
                var command = new CreateCompanyCommand(CompanyId.New) {Company = company};
                try
                {
                    AsyncUtil.RunSync(() => bus.PublishAsync(command, cancellationToken));
                    await Task.CompletedTask;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            Consoler.TitleEnd("seed database finished");
            return true;
        }
    }
}