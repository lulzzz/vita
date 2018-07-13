using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Vita.Domain.Infrastructure
{
  public static class VitaSerializerSettings
  {
    public static JsonSerializerSettings Settings
    {
      get
      {
        var settings = new JsonSerializerSettings
        {
          ContractResolver = new CamelCasePropertyNamesContractResolver(),
          Formatting = Newtonsoft.Json.Formatting.None,
          ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
          MaxDepth = 50
        };

        settings.Converters.Add(new StringEnumConverter());
        return settings;
      }
    }
  }
}