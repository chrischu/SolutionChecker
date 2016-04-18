﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Primitives;
using JetBrains.Annotations;

namespace SolutionInspector.TestInfrastructure.AssertionExtensions
{
  [PublicAPI]
  public static class ObjectStructuralEqualityExtensions
  {
    public static void ShouldAllBeEquivalentTo<T>(this IEnumerable<T> subject, params object[] expectation)
    {
      FluentAssertions.AssertionExtensions.ShouldAllBeEquivalentTo(subject, expectation);
    }

    public static void ShouldAllBeEquivalentTo<T>(this IEnumerable<T> subject, Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config, params object[] expectation)
    {
      FluentAssertions.AssertionExtensions.ShouldAllBeEquivalentTo(subject, expectation, config);
    }

    public static void ShouldAllBeLike<T>(this IEnumerable<T> subject, params object[] expectation)
    {
      subject.ShouldAllBeEquivalentTo(options => options.ExcludingMissingMembers(), expectation);
    }

    public static void BeLike(this ObjectAssertions objectAssertions, object expectation)
    {
      objectAssertions.Subject.ShouldBeEquivalentTo(expectation, options => options.ExcludingMissingMembers());
    }
  }
}