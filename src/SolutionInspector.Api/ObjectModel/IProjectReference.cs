using System;
using JetBrains.Annotations;
using Microsoft.Build.Evaluation;
using SolutionInspector.Commons.Attributes;
using Wrapperator.Interfaces.IO;

namespace SolutionInspector.Api.ObjectModel
{
  /// <summary>
  ///   Represents a reference to another <see cref="IProject" /> in the same solution.
  /// </summary>
  [PublicApi]
  public interface IProjectReference
  {
    /// <summary>
    ///   The referenced project.
    /// </summary>
    [CanBeNull]
    IProject Project { get; }

    /// <summary>
    ///   The original MSBuild project item representing the reference.
    /// </summary>
    ProjectItem OriginalProjectItem { get; }

    /// <summary>
    ///   The include pointing to the referenced .csproj file.
    /// </summary>
    string Include { get; }

    /// <summary>
    ///   A <see cref="IFileInfo" /> that represents the referenced project file.
    /// </summary>
    IFileInfo File { get; }

    /// <summary>
    ///   The referenced project's GUID.
    /// </summary>
    Guid? ReferencedProjectGuid { get; }

    /// <summary>
    ///   The referenced project's name.
    /// </summary>
    string ReferencedProjectName { get; }
  }
}