using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using Vita.Contracts;
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
      if (predictionRequests.Any(x => x == null)) throw new ApplicationException();
      var ev = new BankStatementExtracted1Event
      {
        PredictionRequests = predictionRequests
      };
      Emit(ev);
      await Task.CompletedTask;
    }

    public async Task PredictAsync(PredictBankStatement2Command command, IPredict predict)
    {
      var results = await predict.PredictManyAsync(State.PredictionRequests);
     
      var predictionResults = results as PredictionResult[] ?? results.ToArray();
      if (predictionResults.Any(x => x == null)) throw new ApplicationException();
      var ev = new BankStatementPredicted2Event
      {
        PredictionResults = predictionResults
      };
      Emit(ev);
      
      await Task.CompletedTask;
    }

    public async Task TextMatchAsync(TextMatchBankStatement3Command command, ITextClassifier matcher)
    {
      var unmatched = State.PredictionResults.Where(x => x.PredictedValue == SubCategories.Uncategorised).ToArray();

      Trace.WriteLine($"{Id} unmatched {unmatched.Count()}");

      var matched = new List<KeyValuePair<PredictionResult, TextClassificationResult>>();

      foreach (var item in unmatched)
      {
        item.Method = PredictionMethod.KeywordMatch;
        var result = await matcher.Match(item.Request.Description);
        if (result.Classifier != null && result.Classifier.SubCategory != SubCategories.Uncategorised)
        {
          matched.Add(new KeyValuePair<PredictionResult, TextClassificationResult>(item, result));
        }
      }

      Trace.WriteLine($"{Id} matched {matched.Count()}");
      var ev = new BankStatementTextMatched3Event
      {
        Unmatched = unmatched,
        Matched = matched
      };
      Emit(ev);
     
      await Task.CompletedTask;
    }
  }
}