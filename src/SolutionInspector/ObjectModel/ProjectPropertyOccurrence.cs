using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.Build.Construction;
using SolutionInspector.Api.ObjectModel;

namespace SolutionInspector.ObjectModel
{
  [DebuggerDisplay ("{Value} at {Location} when ({Condition})")]
  internal class ProjectPropertyOccurrence : IProjectPropertyOccurrence
  {
    public string Value { get; }

    [CanBeNull]
    public IProjectPropertyCondition Condition { get; }

    public IProjectLocation Location { get; }

    public ProjectPropertyOccurrence (string value, [CanBeNull] IProjectPropertyCondition condition, IProjectLocation location)
    {
      Value = value;
      Condition = condition;
      Location = location;
    }

    public ProjectPropertyOccurrence (ProjectPropertyElement property)
        : this(property.Value, CreateCondition(property), new ProjectLocation(property))
    {
    }

    private static ProjectPropertyCondition CreateCondition (ProjectPropertyElement property)
    {
      var condition = new ProjectPropertyCondition(property);
      if (condition.Parent != null || condition.Self != null)
        return condition;

      return null;
    }
  }
}