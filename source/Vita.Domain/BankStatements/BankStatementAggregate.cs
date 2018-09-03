using System;
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
            IEnumerable<PredictionRequest> request)
        {
            Emit(new BankStatementExtracted1Event
            {
                PredictionRequests = request
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

        public async Task TextMatchAsync(TextMatchBankStatement3Command command, ITextClassifier textClassifier)
        {
            var unmatched = State.PredictionResults.Where(x => x.PredictedValue == Categories.Uncategorised);
            Trace.WriteLine($"{this.Id} unmatched {unmatched.Count()}");

            var matched = new List<Tuple<PredictionResult, TextClassificationResult>>();

            var predictionResults = unmatched as PredictionResult[] ?? unmatched.ToArray();
            foreach (var x in predictionResults.AsParallel())
            {
                //var many = await textClassifier.MatchMany(x.Request.Description);
                //foreach (var item in many)
                //{
                //    matched.Add(new Tuple<PredictionResult, TextClassificationResult>(x, item));
                //}
                var result = await textClassifier.Match(x.Request.Description);
                if (result.Classifier!=null) matched.Add(new Tuple<PredictionResult, TextClassificationResult>(x, result));
            }

            foreach (var t in matched)
            {
                t.Item1.Method = PredictionMethod.KeywordMatch;
            }
            Trace.WriteLine($"{this.Id} matched {matched.Count()}");
            Emit(new BankStatementTextMatched3Event()
            {
                Unmatched = predictionResults,
                Matched= matched
            });
            await Task.CompletedTask;
        }
    }
}