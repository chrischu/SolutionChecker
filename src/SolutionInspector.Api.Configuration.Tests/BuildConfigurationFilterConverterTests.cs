﻿using System;
using FluentAssertions;
using SolutionInspector.Api.ObjectModel;
using Xunit;

namespace SolutionInspector.Api.Configuration.Tests
{
  public class BuildConfigurationFilterConverterTests
  {
    private readonly BuildConfigurationFilterConverter _sut;

    public BuildConfigurationFilterConverterTests ()
    {
      _sut = new BuildConfigurationFilterConverter();
    }

    [Fact]
    public void ConvertTo ()
    {
      var filter = new BuildConfigurationFilter(new BuildConfiguration("A", "B"), new BuildConfiguration("C", "D"));

      // ACT
      var result = _sut.ConvertTo(filter);

      // ASSERT
      result.Should().Be("A|B,C|D");
    }

    [Fact]
    public void ConvertTo_WithNull_ReturnsNull()
    {
      // ACT
      var result = _sut.ConvertTo(null);

      // ASSERT
      result.Should().BeNull();
    }

    [Fact]
    public void ConvertFrom()
    {
      var buildConfigurationFilter = new BuildConfigurationFilter(new BuildConfiguration("A", "B"), new BuildConfiguration("C", "D"));
      var buildConfigurationFilterString = _sut.ConvertTo(buildConfigurationFilter);

      // ACT
      var result = _sut.ConvertFrom(buildConfigurationFilterString);

      // ASSERT
      result.IsMatch(new BuildConfiguration("A", "B")).Should().BeTrue();
      result.IsMatch(new BuildConfiguration("C", "D")).Should().BeTrue();
      result.IsMatch(new BuildConfiguration("X", "Y")).Should().BeFalse();
    }

    [Fact]
    public void ConvertFrom_WithNull_ReturnsNull()
    {
      // ACT
      var result = _sut.ConvertFrom(null);

      // ASSERT
      result.Should().BeNull();
    }

    [Fact]
    public void ConvertFrom_WithInvalidFormat_Throws()
    {
      var filterString = "NOT A BUILD CONFIGURATION FILTER";

      // ACT
      Action act = () => _sut.ConvertFrom(filterString);

      // ASSERT
      act.ShouldThrow<FormatException>()
          .WithMessage($"The value '{filterString}' is not a valid string representation of a {nameof(BuildConfiguration)}.");
    }
  }
}