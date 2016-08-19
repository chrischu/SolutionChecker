using System;
using System.Collections.Generic;
using System.Linq;

namespace SolutionInspector.ConfigurationUi.Controls
{
  internal interface IConfigurationControlRetriever
  {
    IConfigurationControl GetControlFor (object value);
  }

  internal class ConfigurationControlRetriever : IConfigurationControlRetriever
  {
    private Dictionary<Type, IConfigurationControl> _configurationControls;

    public ConfigurationControlRetriever ()
    {
      _configurationControls = LoadConfigurationControls();
    }

    private Dictionary<Type, IConfigurationControl> LoadConfigurationControls ()
    {
      var interfaceType = typeof(IConfigurationControl);

      var configurationControls = AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(a => a.GetExportedTypes())
          .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
          .Select(Activator.CreateInstance)
          .Cast<IConfigurationControl>()
          .ToDictionary(c => c.ValueType);

      foreach (var control in configurationControls.Values)
        App.Current.Resources.MergedDictionaries.Add(control.Template);

      return configurationControls;
    }

    public IConfigurationControl GetControlFor (object value)
    {
      IConfigurationControl control;
      if (_configurationControls.TryGetValue(value.GetType(), out control))
        return control;

      return null;
    }
  }
}