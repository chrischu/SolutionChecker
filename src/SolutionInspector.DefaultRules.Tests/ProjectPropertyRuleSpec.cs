﻿using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using Machine.Specifications;
using SolutionInspector.ObjectModel;
using SolutionInspector.Rules;
using SolutionInspector.TestInfrastructure.AssertionExtensions;

#region R# preamble for Machine.Specifications files

// ReSharper disable ArrangeTypeModifiers
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable NotAccessedField.Local
// ReSharper disable StaticMemberInGenericType
// ReSharper disable UnassignedField.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnassignedGetOnlyAutoProperty

#endregion

namespace SolutionInspector.DefaultRules.Tests
{
  [Subject(typeof (ProjectPropertyRule))]
  class ProjectPropertyRuleSpec
  {
    static IProject Project;
    static IAdvancedProject AdvancedProject;

    static ProjectPropertyRule SUT;

    Establish ctx = () =>
    {
      Project = A.Fake<IProject>();

      AdvancedProject = A.Fake<IAdvancedProject>();
      A.CallTo(() => Project.Advanced).Returns(AdvancedProject);

      SUT = new ProjectPropertyRule(
          new ProjectPropertyRuleConfiguration
          {
              Property = "Property",
              ExpectedValue = "ExpectedValue"
          });
    };

    class when_evaluating_property_with_same_value
    {
      Establish ctx = () =>
      {
        A.CallTo(() => AdvancedProject.Properties).Returns(new Dictionary<string, string> { { "Property", "ExpectedValue" } });
      };

      Because of = () => Result = SUT.Evaluate(Project);

      It does_not_return_violations = () =>
          Result.Should().BeEmpty();

      static IEnumerable<IRuleViolation> Result;
    }

    class when_evaluating_property_with_different_value
    {
      Establish ctx = () =>
      {
        A.CallTo(() => AdvancedProject.Properties).Returns(new Dictionary<string, string> { { "Property", "ActualValue" } });
      }; 

      Because of = () => Result = SUT.Evaluate(Project);

      It returns_violation = () =>
          Result.ShouldAllBeEquivalentTo(
              new RuleViolation(SUT, Project, "Unexpected value for property 'Property', was 'ActualValue' but should be 'ExpectedValue'."));

      static IEnumerable<IRuleViolation> Result;
    }

    class when_evaluating_non_existing_property
    {
      Establish ctx = () =>
      {
        A.CallTo(() => AdvancedProject.Properties).Returns(new Dictionary<string, string>());
      };

      Because of = () => Result = SUT.Evaluate(Project);

      It returns_violation = () =>
          Result.ShouldAllBeEquivalentTo(
              new RuleViolation(SUT, Project, "Unexpected value for property 'Property', was '<null>' but should be 'ExpectedValue'."));

      static IEnumerable<IRuleViolation> Result;
    }
  }
}