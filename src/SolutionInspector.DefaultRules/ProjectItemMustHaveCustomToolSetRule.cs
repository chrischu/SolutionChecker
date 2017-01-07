using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using SolutionInspector.Api.ObjectModel;
using SolutionInspector.Api.Rules;

namespace SolutionInspector.DefaultRules
{
  /// <summary>
  ///   Verifies project items have custom tool and custom tool namespace set that is configured via
  ///   <see cref="ProjectItemMustHaveCustomToolSetRuleConfiguration" />.
  /// </summary>
  [Description ("Verifies project items have custom tool and custom tool namespace set that is configured via 'expectedCustomTool' " +
                "and 'expectedCustomToolNamespace'.")]
  public class ProjectItemMustHaveCustomToolSetRule : ConfigurableProjectItemRule<ProjectItemMustHaveCustomToolSetRuleConfiguration>
  {
    /// <inheritdoc />
    public ProjectItemMustHaveCustomToolSetRule (ProjectItemMustHaveCustomToolSetRuleConfiguration configuration)
      : base(configuration)
    {
    }

    /// <inheritdoc />
    public override IEnumerable<IRuleViolation> Evaluate (IProjectItem target)
    {
      if ((target.CustomTool ?? "") != Configuration.ExpectedCustomTool)
        yield return
            new RuleViolation(
              this,
              target,
              $"Unexpected value for custom tool, was '{target.CustomTool}' but should be '{Configuration.ExpectedCustomTool}'.");

      if ((target.CustomToolNamespace ?? "") != Configuration.ExpectedCustomToolNamespace)
        yield return
            new RuleViolation(
              this,
              target,
              $"Unexpected value for custom tool namespace, was '{target.CustomToolNamespace}' " +
              $"but should be '{Configuration.ExpectedCustomToolNamespace}'.");
    }
  }

  /// <summary>
  ///   Configuration for the <see cref="ProjectItemMustHaveCustomToolSetRule" />.
  /// </summary>
  public class ProjectItemMustHaveCustomToolSetRuleConfiguration : ConfigurationElement
  {
    /// <summary>
    ///   The expected custom tool.
    /// </summary>
    [ConfigurationProperty ("expectedCustomTool", DefaultValue = "", IsRequired = true)]
    [Description ("The expected custom tool.")]
    public string ExpectedCustomTool
    {
      get { return (string) this["expectedCustomTool"]; }
      set { this["expectedCustomTool"] = value; }
    }

    /// <summary>
    ///   The expected custom tool namespace.
    /// </summary>
    [ConfigurationProperty ("expectedCustomToolNamespace")]
    [Description ("The expected custom tool namespace.")]
    public string ExpectedCustomToolNamespace
    {
      get { return (string) this["expectedCustomToolNamespace"]; }
      set { this["expectedCustomToolNamespace"] = value; }
    }
  }
}