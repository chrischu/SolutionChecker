using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using SolutionInspector.Api.ObjectModel;
using SolutionInspector.Configuration.Attributes;
using SolutionInspector.Commons.Extensions;

namespace SolutionInspector.Api.Configuration
{
  /// <summary>
  ///   Filter for build configurations (e.g. Debug/AnyCPU).
  /// </summary>
  [ConfigurationConverter (typeof(BuildConfigurationFilterConverter))]
  public class BuildConfigurationFilter
  {
    private readonly BuildConfiguration[] _filters;

    /// <inheritdoc />
    public BuildConfigurationFilter (IEnumerable<BuildConfiguration> filters)
      : this(filters.ToArray())
    {
    }

    /// <summary>
    ///   Creates a <see cref="BuildConfigurationFilter" /> from the given <see cref="BuildConfiguration" />s.
    /// </summary>
    public BuildConfigurationFilter (params BuildConfiguration[] filters)
    {
      _filters = filters.ToArray();
    }

    /// <summary>
    ///   Returns <c>true</c> if the given <paramref name="buildConfiguration" /> matches the filter, <c>false</c> otherwise.
    /// </summary>
    public bool IsMatch (BuildConfiguration buildConfiguration)
    {
      var comparer = new BuildConfigurationFilterEqualityComparer();
      return _filters.Any(f => comparer.Equals(f, buildConfiguration));
    }

    /// <inheritdoc />
    [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
    [ExcludeFromCodeCoverage]
    public override string ToString ()
    {
      return _filters.ConvertAndJoin(separator: ",");
    }

    private class BuildConfigurationFilterEqualityComparer : IEqualityComparer<BuildConfiguration>
    {
      public bool Equals ([CanBeNull] BuildConfiguration x, [CanBeNull] BuildConfiguration y)
      {
        return (x?.ConfigurationName == y?.ConfigurationName || x?.ConfigurationName == "*" || y?.ConfigurationName == "*") &&
               (x?.PlatformName == y?.PlatformName || x?.PlatformName == "*" || y?.PlatformName == "*");
      }

      [ExcludeFromCodeCoverage]
      public int GetHashCode ([CanBeNull] BuildConfiguration obj)
      {
        return obj?.GetHashCode() ?? 0;
      }
    }
  }
}