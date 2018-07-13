using System;
using System.Configuration;
using System.Linq;
using Autofac;

namespace Vita.Domain.Infrastructure.Modules
{
  /// <summary>
  ///     A convention-based configuration module that will read and register a instance of <see cref="TConfig" /> to Autofac
  /// </summary>
  /// <remarks>Modified from http://paulstovell.com/blog/convention-configuration </remarks>
  public abstract class ConfiguredModuleBase<T, TConfig> : Module where T : ConfiguredModuleBase<T, TConfig>
      where TConfig : class, new()
  {
    public TConfig Configuration { get; protected set; }

    /// <summary>
    ///     Hydrate the Configuration object and register an instance with Autofac
    /// </summary>
    /// <param name="builder"></param>
    protected override void Load(ContainerBuilder builder)
    {
      ReadConfiguration();
      builder.RegisterInstance(Configuration).As<TConfig>();
    }

    private void ReadConfiguration()
    {
      Configuration = new TConfig();
      const char arg1 = ':';
      // Get all keys with exactly one ':' in format "NameModule.ConfigurationPropertyName"
      SetProperty(arg1);

      const char arg2 = '.';
      // Get all keys with exactly one '.' in format "NameModule.ConfigurationPropertyName"
      SetProperty(arg2);
    }

    private void SetProperty(char arg)
    {

      var settings = ConfigurationManager.AppSettings;
      var moduleName = typeof(T).Name.Replace("Module", "");
      var keys = settings.AllKeys
          .Where(k => k.Count(c => c == arg) == 1)
          .Where(k => k.IndexOf($"{moduleName}" + arg, StringComparison.Ordinal) == 0);

      // hydrate this.Configuration object
      foreach (var key in keys)
      {
        var propertyName = key.Substring(key.IndexOf(arg) + 1);
        var property = Configuration.GetType().GetProperty(propertyName);
        if (property == null)
        {
          throw new ConfigurationErrorsException($"Invalid configuration key: {key}");
        }
        var value = settings[key];
        property.SetValue(Configuration, Convert.ChangeType(value, property.PropertyType), null);
      }
    }
  }
}