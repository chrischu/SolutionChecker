using System;

namespace SolutionInspector.Api.Rules
{
  /// <summary>
  ///   The target of a <see cref="IRule" />.
  /// </summary>
  public interface IRuleTarget
  {
    /// <summary>
    ///   Identifies the <see cref="IRuleTarget" />.
    /// </summary>
    string Identifier { get; }

    /// <summary>
    ///   The full path of the <see cref="IRuleTarget" />.
    /// </summary>
    string FullPath { get; }
  }
}