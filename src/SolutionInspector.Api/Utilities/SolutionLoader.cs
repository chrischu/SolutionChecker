using System;
using JetBrains.Annotations;
using SolutionInspector.Api.Configuration.MsBuildParsing;
using SolutionInspector.Api.ObjectModel;
using Wrapperator.Interfaces.IO;

namespace SolutionInspector.Api.Utilities
{
  internal interface ISolutionLoader
  {
    ISolution Load (string solutionPath, IMsBuildParsingConfiguration msBuildParsingConfiguration);
  }

  [UsedImplicitly /* by container */]
  internal class SolutionLoader : ISolutionLoader
  {
    private readonly IFile _file;

    public SolutionLoader (IFile file)
    {
      _file = file;
    }

    public ISolution Load (string solutionPath, IMsBuildParsingConfiguration msBuildParsingConfiguration)
    {
      if (!_file.Exists(solutionPath))
        throw new SolutionNotFoundException(solutionPath);

      return Solution.Load(solutionPath, msBuildParsingConfiguration);
    }
  }
}