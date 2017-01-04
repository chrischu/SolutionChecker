using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using JetBrains.Annotations;
using SolutionInspector.Api.ObjectModel;

namespace SolutionInspector.ObjectModel
{
  internal sealed class NuGetPackage : INuGetPackage
  {
    public string Id { get; }

    public Version Version { get; }

    public bool IsPreRelease { get; }

    public string PreReleaseTag { get; }

    public string FullVersionString => $"{Version}{PreReleaseTag}";

    public string PackageDirectoryName => $"{Id}.{FullVersionString}";

    public string TargetFramework { get; }

    public bool IsDevelopmentDependency { get; }

    public NuGetPackage (
        string id,
        Version version,
        bool isPreRelease,
        [CanBeNull] string preReleaseTag,
        string targetFramework,
        bool isDevelopmentDependency)
    {
      Id = id;
      Version = version;
      IsPreRelease = isPreRelease;
      PreReleaseTag = preReleaseTag;
      TargetFramework = targetFramework;
      IsDevelopmentDependency = isDevelopmentDependency;
    }

    public static NuGetPackage FromXmlElement (XmlElement packageElement)
    {
      var id = packageElement.GetAttribute("id");
      bool isPreRelease = false;
      string preReleaseTag = null;

      var versionString = packageElement.GetAttribute("version");
      if (versionString.Contains("-"))
      {
        var split = versionString.Split('-');
        isPreRelease = true;
        preReleaseTag = "-" + split[1];
        versionString = split[0];
      }

      var version = Version.Parse(versionString);

      var targetFramework = packageElement.GetAttribute("targetFramework");

      var isDevelopmentDependency = packageElement.HasAttribute("developmentDependency")
                                    && packageElement.GetAttribute("developmentDependency") == "true";

      return new NuGetPackage(id, version, isPreRelease, preReleaseTag, targetFramework, isDevelopmentDependency);
    }

    public bool Equals ([CanBeNull] INuGetPackage other)
    {
      if (ReferenceEquals(null, other))
        return false;
      if (ReferenceEquals(this, other))
        return true;
      return string.Equals(PackageDirectoryName, other.PackageDirectoryName);
    }

    public override bool Equals ([CanBeNull] object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;
      if (ReferenceEquals(this, obj))
        return true;
      return obj is INuGetPackage && Equals((INuGetPackage) obj);
    }

    [ExcludeFromCodeCoverage]
    public override int GetHashCode ()
    {
      return PackageDirectoryName.GetHashCode();
    }

    public static bool operator == ([CanBeNull] NuGetPackage left, [CanBeNull] NuGetPackage right)
    {
      return Equals(left, right);
    }

    public static bool operator != ([CanBeNull] NuGetPackage left, [CanBeNull] NuGetPackage right)
    {
      return !Equals(left, right);
    }
  }
}