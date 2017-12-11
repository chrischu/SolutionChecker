using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using SolutionInspector.Api.ObjectModel;
using SolutionInspector.Api.Rules;

namespace SolutionInspector.DefaultRules
{
  /// <summary>
  ///   Verifies that all <see cref="IProjectReference" />s in the project are valid (i.e. point to existing csproj files included in the solution
  ///   and have the correct project GUID).
  /// </summary>
  [Description ("Verifies that all project references in the project are valid (i.e. point to existing csproj files included in the solution " +
                "and have the correct project GUID).")]
  public class ProjectReferencesMustBeValidRule : ProjectRule
  {
    /// <inheritdoc />
    public override IEnumerable<IRuleViolation> Evaluate ([NotNull] IProject target)
    {
      foreach (var projectReference in target.ProjectReferences)
        if (projectReference.ReferencedProjectGuid == null)
        {
          yield return
              new RuleViolation(
                this,
                target,
                $"The reference to project '{projectReference.ReferencedProjectName}' ('{projectReference.Include}') is invalid because it does " +
                "not specify the project GUID of the referenced project.");
        }
        else
        {
          if (projectReference.Project == null)
          {
            var projectReferencedByGuid = target.Solution.GetProjectByProjectGuid(projectReference.ReferencedProjectGuid.Value);
            if (projectReferencedByGuid != null)
              yield return
                  new RuleViolation(
                    this,
                    target,
                    $"The reference to project '{projectReference.ReferencedProjectName}' ('{projectReference.Include}') is invalid because the " +
                    "referenced project file could not be found. However, there is a project that matches the given project guid: " +
                    $"'{projectReferencedByGuid.Name}' ('{target.GetIncludePathFor(projectReferencedByGuid)}'). Did you mean to reference that one?")
                  ;
            else
              yield return
                  new RuleViolation(
                    this,
                    target,
                    $"The reference to project '{projectReference.ReferencedProjectName}' ('{projectReference.Include}') is invalid because the " +
                    "referenced project file could not be found and the solution does not contain a project that at least matches the specified" +
                    $"referenced project GUID ({projectReference.ReferencedProjectGuid.Value}).");
          }
          else
          {
            if (projectReference.Project.Guid != projectReference.ReferencedProjectGuid.Value)
              yield return
                  new RuleViolation(
                    this,
                    target,
                    $"The reference to project '{projectReference.ReferencedProjectName}' ('{projectReference.Include}') is invalid because the" +
                    $"specified project GUID for the referenced project ({projectReference.ReferencedProjectGuid.Value}) does not match the " +
                    $"project GUID of the actually referenced project ({projectReference.Project.Guid}).");
          }
        }
    }
  }
}