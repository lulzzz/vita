using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using Vita.Contracts;

namespace Vita.Domain.BankStatements.Commands
{
    public class ExtractBankStatement1CommandHandler  : CommandHandler<BankStatementAggregate, BankStatementId, ExtractBankStatement1Command>
    {
        private readonly IBankStatementService _bankStatementService;

        public ExtractBankStatement1CommandHandler(IBankStatementService bankStatementService)
        {
            _bankStatementService = bankStatementService;
        }

        public override async Task ExecuteAsync(BankStatementAggregate aggregate, ExtractBankStatement1Command command,
            CancellationToken cancellationToken)
        {
            //TODO extract bank statements
            string bankName = "anz";
            var anz = new BankLogin("anz", "username", Environment.GetEnvironmentVariable("bankstatements-anz-cdm-username"),
                "password", Environment.GetEnvironmentVariable("bankstatements-anz-cdm-password"));
            var test = new BankLogin("bank_of_statements", "username", "12345678", "password", "TestMyMoney");
            BankLogin bank;

            switch (bankName)
            {
                case "anz":
                    bank = anz;
                    break;
                default:
                   // bank = test;
                    bank = anz;
                    break;
            }

            var result = await _bankStatementService.LoginFetchAllAsync(bankName, bank);
            var request = result.ToPredictionRequests();

            await aggregate.ExtractBankStatementAsync(command,request);
        }
    }
}
