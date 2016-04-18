using System.Xml.Linq;

namespace SolutionInspector.ObjectModel
{
  /// <summary>
  /// A project item representing the project's configuration file (App.config/Web.config).
  /// </summary>
  public interface IConfigurationProjectItem : IProjectItem
  {
    /// <summary>
    /// The XML contents of the configuration file.
    /// </summary>
    XDocument ConfigurationXml { get; }
  }

  internal class ConfigurationProjectItem : ProjectItem, IConfigurationProjectItem
  {
    public ConfigurationProjectItem(IProject project, IProjectItem projectItem)
        : base(project, projectItem.Include, projectItem.BuildAction, projectItem.File, projectItem.Metadata)
    {
      string xmlString;
      using (var str = File.OpenText())
        xmlString = str.ReadToEnd();
      ConfigurationXml = XDocument.Parse(xmlString);
    }

    public XDocument ConfigurationXml { get; }
  }
}