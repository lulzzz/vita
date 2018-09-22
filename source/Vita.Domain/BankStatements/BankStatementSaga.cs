using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.Exceptions;
using EventFlow.Sagas;
using EventFlow.Sagas.AggregateSagas;
using Vita.Domain.BankStatements.Commands;
using Vita.Domain.BankStatements.Events;

namespace Vita.Domain.BankStatements
{
    /// <summary>
    /// http://docs.geteventflow.net/Sagas.html
    /// </summary>
    public class BankStatementSaga : AggregateSaga<BankStatementSaga, BankStatementSagaId, BankStatementSagaLocator>,
        ISagaIsStartedBy<BankStatementAggregate, BankStatementId, BankStatementExtracted1Event>,
        ISagaHandles<BankStatementAggregate, BankStatementId, BankStatementPredicted2Event>,
        ISagaHandles<BankStatementAggregate, BankStatementId, BankStatementTextMatched3Event>,
        IEmit<BankStatementSagaExtractedEvent>,
        IEmit<BankStatementSagaPredictedEvent>,
        IEmit<BankStatementSagaTextMatchedEvent>
    {
    
        public BankStatementSaga(BankStatementSagaId id) : base(id)
        {
        }

        /// <summary>
        ///   extract bank statements
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <param name="sagaContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandleAsync(
            IDomainEvent<BankStatementAggregate, BankStatementId, BankStatementExtracted1Event> domainEvent,
            ISagaContext sagaContext, CancellationToken cancellationToken)
        {
            // This check is redundant! We do it to verify EventFlow works correctly
            if (State != SagaState.New) throw DomainError.With("Saga must be new!");
            Publish(new PredictBankStatement2Command
            {
                AggregateId = domainEvent.AggregateIdentity,
                SourceId = SourceId.New
            });
            Emit(new BankStatementSagaExtractedEvent());
            await Task.CompletedTask;
        }

        /// <summary>
        ///   predict bank statement lines
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <param name="sagaContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandleAsync(
            IDomainEvent<BankStatementAggregate, BankStatementId, BankStatementPredicted2Event> domainEvent,
            ISagaContext sagaContext, CancellationToken cancellationToken)
        {
            if (State != SagaState.Running) throw DomainError.With("Saga must be running!");
            Publish(new TextMatchBankStatement3Command
            {
                AggregateId = domainEvent.AggregateIdentity,
                SourceId = SourceId.New
                // SourceId = new SourceId(domainEvent.GetIdentity().Value)
            });
            Emit(new BankStatementSagaPredictedEvent());
            await Task.CompletedTask;
        }

        public async Task HandleAsync(
            IDomainEvent<BankStatementAggregate, BankStatementId, BankStatementTextMatched3Event> domainEvent,
            ISagaContext sagaContext, CancellationToken cancellationToken)
        {
            if (State != SagaState.Running) throw DomainError.With("Saga must be running!");
            Emit(new BankStatementSagaTextMatchedEvent());
            await Task.CompletedTask;
        }

        public void Apply(BankStatementSagaExtractedEvent aggregateEvent)
        {
            
        }

        public void Apply(BankStatementSagaPredictedEvent aggregateEvent)
        {
        }

        public void Apply(BankStatementSagaTextMatchedEvent aggregateEvent)
        {
            // As this is the last event, we complete the saga by calling Complete()
            Complete();
        }
    }

    public class BankStatementSagaId : ISagaId
    {
        public BankStatementSagaId(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }

    public class BankStatementSagaLocator : ISagaLocator
    {
        public Task<ISagaId> LocateSagaAsync(
            IDomainEvent domainEvent,
            CancellationToken cancellationToken)
        {
            var id = domainEvent.Metadata["aggregate_id"];
            var sagaId = new BankStatementSagaId(id.Replace("bankstatement-", string.Empty));

            return Task.FromResult<ISagaId>(sagaId);
        }
    }

    #region Events

    public class BankStatementSagaExtractedEvent : AggregateEvent<BankStatementSaga, BankStatementSagaId>
    {
    }

    public class BankStatementSagaPredictedEvent : AggregateEvent<BankStatementSaga, BankStatementSagaId>
    {
    }

    public class BankStatementSagaTextMatchedEvent : AggregateEvent<BankStatementSaga, BankStatementSagaId>
    {
    }

    #endregion
}