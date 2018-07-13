using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Serilog;
using Serilog.Context;
using Vita.Contracts;
using Vita.Domain.Infrastructure;

namespace Vita.Domain.Places
{
  public class PlaceHandler : CollectionBase, IConsumer<PlaceRequest>
  {
    private readonly IRepository<Place> _repository;

    public PlaceHandler(IRepository<Place> repository)
    {
      _repository = repository;
    }

    public async Task Consume(ConsumeContext<PlaceRequest> context)
    {
      using (LogContext.PushProperty("RequestId", context.RequestId))
      using (LogContext.PushProperty("CorrelationId", context.CorrelationId))
      {
        LogVerbose($"received: {context.SentTime}");

        try
        {
          var found = _repository.GetById(context.Message.Place.Id);

          if (found == null)
            found = _repository.Find(x => x.PlaceId == context.Message.Place.PlaceId).SingleOrDefault();

          if (found == null)
            found = _repository.Find(x => x.Name.ToLowerInvariant() == context.Message.Place.Name.ToLowerInvariant()).SingleOrDefault();

          if (found == null)
          {
            if (context.Message.Place.Id == Guid.Empty) context.Message.Place.Id = Guid.NewGuid();
            context.Message.Place.CreatedUtc = DateTime.UtcNow;
            var result = _repository.Insert(context.Message.Place);
            LogInfo($"insert new ok {context.Message.Place.Name} {result}");
          }
          else
          {
            found.ModifiedUtc = DateTime.UtcNow;
            found.Name = context.Message.Place.Name;
            found.PlaceId = context.Message.Place.PlaceId;
            found.Types = context.Message.Place.Types;
            _repository.Save(found);
            LogInfo($"save ok {context.Message.Place.Name} {found.Name}");
          }
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          Log.Error(e, "error {err}", context.Message.Place.Name);
          throw;
        }
      }

      await Task.Delay(TimeSpan.FromSeconds(1));
    }
  }
}