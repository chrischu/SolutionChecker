using System.IO;
using SystemInterface.IO;
using JetBrains.Annotations;
using SolutionInspector.Configuration.MsBuildParsing;
using SolutionInspector.ObjectModel;

namespace SolutionInspector.Utilities
{
  internal interface ISolutionLoader
  {
    ISolution Load(string solutionPath, IMsBuildParsingConfiguration msBuildParsingConfiguration);
  }

  [UsedImplicitly( /* by container */)]
  internal class SolutionLoader : ISolutionLoader
  {
    private readonly IFile _file;

    public SolutionLoader(IFile file)
    {
      _file = file;
    }

    public ISolution Load(string solutionPath, IMsBuildParsingConfiguration msBuildParsingConfiguration)
    {
      if (!_file.Exists(solutionPath))
        throw new FileNotFoundException($"Could not find solution file at '{solutionPath}'.");

      return Solution.Load(solutionPath, msBuildParsingConfiguration);
    }
  }
}