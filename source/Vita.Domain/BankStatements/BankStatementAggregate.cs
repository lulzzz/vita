using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using Vita.Contracts;
using Vita.Contracts.SubCategories;
using Vita.Domain.BankStatements.Commands;
using Vita.Domain.BankStatements.Events;

namespace Vita.Domain.BankStatements
{
    [AggregateName("BankStatement")]
    public class BankStatementAggregate : AggregateRoot<BankStatementAggregate, BankStatementId>
    {
        public BankStatementState State { get; protected set; }

        public BankStatementAggregate(BankStatementId id) : base(id)
        {
            State = new BankStatementState();
            Register(State);
        }

        public async Task ExtractBankStatementAsync(ExtractBankStatement1Command command,
            IEnumerable<PredictionRequest> requests)
        {
            var predictionRequests = requests as PredictionRequest[] ?? requests.ToArray();
            foreach (var request in predictionRequests)
                if (request.Id == Guid.Empty)
                    request.Id = Guid.NewGuid();

            Emit(new BankStatementExtracted1Event
            {
                PredictionRequests = predictionRequests
            });
            await Task.CompletedTask;
        }

        public async Task PredictAsync(PredictBankStatement2Command command, IPredict predict)
        {
            var results = await predict.PredictManyAsync(State.PredictionRequests);

            Emit(new BankStatementPredicted2Event
            {
                PredictionResults = results
            });
            await Task.CompletedTask;
        }

        public async Task TextMatchAsync(TextMatchBankStatement3Command command, ITextClassifier matcher)
        {
            var unmatched = State.PredictionResults.Where(x => x.PredictedValue == Categories.Uncategorised).ToArray();

            Trace.WriteLine($"{Id} unmatched {unmatched.Count()}");

            var matched = new ConcurrentDictionary<PredictionResult, TextClassificationResult>();

            unmatched.AsParallel().ForAll(async predictionResult =>
            {
                if (!matched.ContainsKey(predictionResult))
                {
                    var result = await matcher.Match(predictionResult.Request.Description);
                    if (result.Classifier != null)
                    {
                        predictionResult.Method = PredictionMethod.KeywordMatch;
                        matched.TryAdd(predictionResult, result);
                    }
                }
            });

            Trace.WriteLine($"{Id} matched {matched.Count()}");
            Emit(new BankStatementTextMatched3Event
            {
                Unmatched = unmatched,
                Matched = matched
            });
            await Task.CompletedTask;
        }
    }
}