﻿using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using SolutionInspector.Commons.Extensions;

namespace SolutionInspector.Commons.Tests.Extensions
{
  public class EnumerableExtensionsTests
  {
    [Test]
    [TestCase(new int[0], false)]
    [TestCase(new[] { 7 }, false)]
    [TestCase(new[] { 7, 9 }, true)]
    public void ContainsMoreThanOne (IEnumerable<int> enumerable, bool expected)
    {
      // ACT
      var result = enumerable.ContainsMoreThanOne();

      // ASSERT
      result.Should().Be(expected);
    }

    [Test]
    public void ForEach ()
    {
      var source = new[] { 1, 2 };
      var action = A.Fake<Action<int>>();

      // ACT
      source.ForEach(action);

      // ASSERT
      A.CallTo(() => action(1)).MustHaveHappened(Repeated.Exactly.Once);
      A.CallTo(() => action(2)).MustHaveHappened(Repeated.Exactly.Once);
    }
  }
}