using System;
using JetBrains.Annotations;

namespace SolutionInspector.Commons.Extensions
{
  /// <summary>
  ///   Extension methods for <see cref="string" />.
  /// </summary>
  public static class StringExtensions
  {
    /// <summary>
    ///   Removes the given <paramref name="suffix" /> from the string.
    /// </summary>
    [CanBeNull]
    public static string RemoveSuffix ([CanBeNull] this string s, string suffix)
    {
      if (s == null)
        return null;

      return s.EndsWith(suffix) ? s.Substring(0, s.Length - suffix.Length) : s;
    }

    /// <summary>
    ///   Get the prefix with the given <paramref name="length" /> from <paramref name="s" />.
    /// </summary>
    [CanBeNull]
    public static string Prefix ([CanBeNull] this string s, int length)
    {
      return s?.Substring(0, Math.Min(s.Length, length));
    }

    /// <summary>
    ///   Get the suffix with the given <paramref name="length" /> from <paramref name="s" />.
    /// </summary>
    [CanBeNull]
    public static string Suffix ([CanBeNull] this string s, int length)
    {
      return s?.Substring(Math.Max(0, s.Length - length));
    }

    public static string ToFirstCharUpper ([CanBeNull] this string s)
    {
      if (s == null)
        return null;

      if (s.Length == 0)
        return s;

      if (s.Length == 1)
        return s.ToUpper();

      return char.ToUpper(s[0]) + s.Substring(1);
    }
  }
}