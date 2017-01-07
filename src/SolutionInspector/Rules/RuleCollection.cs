using System.Collections.Generic;
using System.Linq;
using SolutionInspector.Api.Rules;

namespace SolutionInspector.Rules
{
  internal interface IRuleCollection
  {
    IReadOnlyCollection<ISolutionRule> SolutionRules { get; }
    IReadOnlyCollection<IProjectRule> ProjectRules { get; }
    IReadOnlyCollection<IProjectItemRule> ProjectItemRules { get; }
  }

  internal class RuleCollection : IRuleCollection
  {
    public RuleCollection (
      IEnumerable<ISolutionRule> solutionRules,
      IEnumerable<IProjectRule> projectRules,
      IEnumerable<IProjectItemRule> projectItemRules)
    {
      SolutionRules = solutionRules.ToArray();
      ProjectRules = projectRules.ToArray();
      ProjectItemRules = projectItemRules.ToArray();
    }

    public IReadOnlyCollection<ISolutionRule> SolutionRules { get; }
    public IReadOnlyCollection<IProjectRule> ProjectRules { get; }
    public IReadOnlyCollection<IProjectItemRule> ProjectItemRules { get; }
  }
}