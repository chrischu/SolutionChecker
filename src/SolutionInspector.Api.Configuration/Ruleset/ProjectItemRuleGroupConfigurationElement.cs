﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SolutionInspector.Api.Rules;
using SolutionInspector.Configuration;

namespace SolutionInspector.Api.Configuration.Ruleset
{
  /// <summary>
  ///   Configuration for a group of <see cref="ProjectItemRule" />s.
  /// </summary>
  public interface IProjectItemRuleGroupConfiguration
  {
    INameFilter AppliesTo { get; }
    INameFilter InProject { get; }
    IReadOnlyCollection<IRuleConfiguration> Rules { get; }
  }

  /// <inheritdoc cref="IProjectItemRuleGroupConfiguration" />
  /// >
  public class ProjectItemRuleGroupConfigurationElement : ConfigurationElement, IProjectItemRuleGroupConfiguration
  {
    [ConfigurationValue]
    public NameFilter AppliesTo
    {
      get { return GetConfigurationValue<NameFilter>(); }
      set { SetConfigurationValue(value); }
    }

    [ConfigurationValue]
    public NameFilter InProject
    {
      get { return GetConfigurationValue<NameFilter>(); }
      set { SetConfigurationValue(value); }
    }

    [ConfigurationCollection (IsDefaultCollection = true, ElementName = "rule", IsOptional = true)]
    public ConfigurationElementCollection<RuleConfigurationElement> Rules => GetConfigurationCollection<RuleConfigurationElement>();

    [ExcludeFromCodeCoverage]
    INameFilter IProjectItemRuleGroupConfiguration.AppliesTo => AppliesTo;

    [ExcludeFromCodeCoverage]
    INameFilter IProjectItemRuleGroupConfiguration.InProject => InProject;

    [ExcludeFromCodeCoverage]
    IReadOnlyCollection<IRuleConfiguration> IProjectItemRuleGroupConfiguration.Rules => Rules;
  }
}