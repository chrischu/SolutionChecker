using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;
using SolutionInspector.Api.ObjectModel;
using SolutionInspector.Api.Rules;
using SolutionInspector.Configuration;

namespace SolutionInspector.DefaultRules
{
  /// <summary>
  ///   Checks that all given resource files are localized in all given languages in the project.
  /// </summary>
  [Description ("Checks that all given resource files are localized in all given languages in the project.")]
  public class RequiredResourceLanguagesRule : ProjectRule
  {
    /// <summary>
    ///   Comma-separated list of required resources (e.g. "Resources,OtherResources").
    /// </summary>
    [CanBeNull]
    [ConfigurationValue]
    [Description ("List of required resources (e.g. 'Resources').")]
    public CommaSeparatedStringCollection RequiredResources => GetConfigurationValue<CommaSeparatedStringCollection>();

    /// <summary>
    ///   Comma-separated list of required languages (e.g. "de,pl,hr").
    /// </summary>
    [CanBeNull]
    [ConfigurationValue]
    [Description ("List of required languages (e.g. 'de', 'pl').")]
    public CommaSeparatedStringCollection RequiredLanguages => GetConfigurationValue<CommaSeparatedStringCollection>();

    /// <inheritdoc />
    public override IEnumerable<IRuleViolation> Evaluate ([NotNull] IProject target)
    {
      if (RequiredResources != null)
        foreach (var resourceName in RequiredResources)
        {
          var resourceDefaultLanguageFileName = $"{resourceName}.resx";
          if (target.ProjectItems.All(i => i.Name != resourceDefaultLanguageFileName))
          {
            yield return
                new RuleViolation(
                  this,
                  target,
                  $"For the required resource '{resourceName}' no default resource file ('{resourceDefaultLanguageFileName}') could be found.");
          }

          if (RequiredLanguages != null)
          {
            foreach (var languageName in RequiredLanguages)
            {
              var resourceLanguageFileName = $"{resourceName}.{languageName}.resx";
              if (target.ProjectItems.All(i => i.Name != resourceLanguageFileName))
                yield return
                    new RuleViolation(
                      this,
                      target,
                      $"For the required resource '{resourceName}' no resource file for language '{languageName}' " +
                      $"('{resourceLanguageFileName}') could be found.");
            }
          }
        }
    }
  }
}