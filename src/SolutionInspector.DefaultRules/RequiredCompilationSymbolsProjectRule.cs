using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using JetBrains.Annotations;
using SolutionInspector.Configuration.Infrastructure;
using SolutionInspector.Extensions;
using SolutionInspector.ObjectModel;
using SolutionInspector.Rules;
using SolutionInspector.Utilities;

namespace SolutionInspector.DefaultRules
{
  /// <summary>
  /// Verifies that all the compilation symbols (configured in <see cref="RequiredCompilationSymbolsProjectRuleConfiguration"/>) are configured in the project.
  /// </summary>
  public class RequiredCompilationSymbolsProjectRule : ConfigurableProjectRule<RequiredCompilationSymbolsProjectRuleConfiguration>
  {
    /// <inheritdoc />
    public RequiredCompilationSymbolsProjectRule([NotNull] RequiredCompilationSymbolsProjectRuleConfiguration configuration)
        : base(configuration)
    {
    }

    /// <inheritdoc />
    public override IEnumerable<IRuleViolation> Evaluate(IProject target)
    {
      foreach (var config in Configuration)
      {
        var matchingBuildConfigs = target.BuildConfigurations.Where(c => config.BuildConfigurationFilter.IsMatch(c));

        foreach (var matchingBuildConfig in matchingBuildConfigs)
        {
          var properties = target.Advanced.ConfigurationDependentProperties[matchingBuildConfig];

          var actualSymbols = new HashSet<string>(properties.GetValueOrDefault("DefineConstants").Split(';'));

          foreach (var requiredSymbol in config.RequiredCompilationSymbols)
            if (!actualSymbols.Contains(requiredSymbol))
              yield return
                  new RuleViolation(
                      this,
                      target,
                      $"In the build configuration '{matchingBuildConfig}' the required compilation symbol '{requiredSymbol}' was not found.");
        }
      }
    }
  }

  /// <summary>
  /// Configuration for the <see cref="RequiredCompilationSymbolsProjectRule"/>.
  /// </summary>
  [ConfigurationCollection(typeof(RequiredCompilationSymbolsConfiguration))]
  public class RequiredCompilationSymbolsProjectRuleConfiguration
      : KeyedConfigurationElementCollectionBase<RequiredCompilationSymbolsConfiguration, BuildConfigurationFilter>
  {
  }

  /// <summary>
  /// Configuration for which compilation symbols (<see cref="RequiredCompilationSymbols"/>) are required in the build configurations matching the <see cref="BuildConfigurationFilter"/>.
  /// </summary>
  public class RequiredCompilationSymbolsConfiguration : KeyedConfigurationElement<BuildConfigurationFilter>
  {
    /// <inheritdoc />
    public override string KeyName => "buildConfigurationFilter";

    /// <summary>
    /// Filter that controlls which build configuration this <see cref="RequiredCompilationSymbolsConfiguration"/> applies to.
    /// </summary>
    [TypeConverter(typeof(BuildConfigurationFilterConverter))]
    [ConfigurationProperty("buildConfigurationFilter", DefaultValue = "*|*", IsRequired = true)]
    public BuildConfigurationFilter BuildConfigurationFilter
    {
      get { return (BuildConfigurationFilter)this["buildConfigurationFilter"]; }
      set { this["buildConfigurationFilter"] = value; }
    }

    /// <summary>
    /// All the compilation symbols that are required and are therefore checked.
    /// </summary>
    [TypeConverter(typeof(CommaDelimitedStringCollectionConverter))]
    [ConfigurationProperty("requiredCompilationSymbols", DefaultValue = "", IsRequired = true)]
    public CommaDelimitedStringCollection RequiredCompilationSymbols
    {
      get { return (CommaDelimitedStringCollection)this["requiredCompilationSymbols"]; }
      set { this["requiredCompilationSymbols"] = value; }
    }
  }
}