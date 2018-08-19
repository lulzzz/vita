using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using EventFlow;
using MediatR;
using Vita.Contracts;
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
           
            //var companies = CompanySpreadsheet.Import();

            var chance = new ChanceNET.Chance();
            var companies = new List<Company> {chance.Object<Company>()};

            var bus = IocContainer.Container.Resolve<ICommandBus>();

            foreach (var company in companies)
            {
                var command = new CreateCompanyCommand(CompanyId.New) {Company = company};
                try
                {
                    await bus.PublishAsync(command, CancellationToken.None);
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
