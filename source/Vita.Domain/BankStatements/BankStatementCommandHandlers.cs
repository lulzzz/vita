using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using Vita.Contracts;
using Vita.Domain.BankStatements.Commands;

namespace Vita.Domain.BankStatements
{
  public class BankStatementCommandHandlers : ICommandHandler<BankStatementAggregate, BankStatementId, ExtractBankStatement1Command>
  {
    public async Task ExecuteAsync(BankStatementAggregate aggregate, ExtractBankStatement1Command command, CancellationToken cancellationToken)
    {
      //aggregate.
      await Task.CompletedTask;
    }
  }
}
