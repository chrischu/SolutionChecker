using JetBrains.Annotations;

namespace SolutionInspector.Api.Rules
{
  /// <inheritdoc />
  [PublicAPI]
  public class RuleViolation : IRuleViolation
  {
    /// <summary>
    ///   Creates a new <see cref="RuleViolation" />.
    /// </summary>
    public RuleViolation (IRule rule, IRuleTarget target, string message)
    {
      Rule = rule;
      Target = target;
      Message = message;
    }

    /// <inheritdoc />
    public IRule Rule { get; }

    /// <inheritdoc />
    public IRuleTarget Target { get; }

    /// <inheritdoc />
    public string Message { get; }
  }
}