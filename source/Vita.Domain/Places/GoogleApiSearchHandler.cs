using System;
using System.Linq;
using System.Threading.Tasks;
using GoogleApi.Entities.Common.Enums;
using MassTransit;
using Serilog.Context;
using Vita.Domain.Infrastructure;
using Vita.Domain.Services;

namespace Vita.Domain.Places
{
  public class GoogleApiSearchHandler : CollectionBase, IConsumer<GoogleApiSearchRequest>
  {
    private IBusControl _bus;
    private readonly IRepository<GoogleApiSearchResponse> _repository;

    public GoogleApiSearchHandler(IBusControl bus, IRepository<GoogleApiSearchResponse> repository)
    {
      _bus = bus;
      _repository = repository;
    }

    public async Task Consume(ConsumeContext<GoogleApiSearchRequest> context)
    {
      using (LogContext.PushProperty("RequestId", context.RequestId))
      using (LogContext.PushProperty("CorrelationId", context.CorrelationId))
      {
        LogVerbose($"received: {context.SentTime}");

        try
        {

          var found = _repository.GetAll()
            .Where(x => x.Request != null &&
                        String.Equals(x.Request.SearchTerm, context.Message.SearchTerm,
                          StringComparison.InvariantCultureIgnoreCase));
            
          if (found.Any())
          {
            LogInfo($"skip: {context.Message.SearchTerm}");
            await Task.CompletedTask;
          }
          else
          {
            LogDebug($"insert try: {context.Message.SearchTerm}");
            var search = new GooglePlacesSearcher();
            var result = await search.SearchAsync(context.Message.SearchTerm);

            if (result.Status != Status.Ok) LogWarn($"{result.Status.ToString()} {result.ErrorMessage}");

            var item = new GoogleApiSearchResponse
            {
              Id = Guid.NewGuid(),
              PlacesTextSearchResponse = result,
              Request = new GoogleApiSearchRequest {SearchTerm = context.Message.SearchTerm}
            };

            _repository.Insert(item);
            LogInfo($"insert ok: {context.Message.SearchTerm}");
          }
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          LogWarn(e, context.Message.SearchTerm);
        }
      }
    }
  }
}