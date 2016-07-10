﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SystemInterface.Configuration;
using SystemInterface.IO;
using SystemInterface.Reflection;
using SystemWrapper.Configuration;
using SystemWrapper.IO;
using SystemWrapper.Reflection;
using Autofac;
using JetBrains.Annotations;
using ManyConsole;
using NLog;
using SolutionInspector.Api.Commands;
using SolutionInspector.Api.Configuration;
using SolutionInspector.Api.Reporting;
using SolutionInspector.Api.Rules;
using SolutionInspector.Api.Utilities;

namespace SolutionInspector.Api
{
  /// <summary>
  ///   Entry point for a SolutionInspector run.
  /// </summary>
  [PublicAPI]
  [ExcludeFromCodeCoverage]
  public static class SolutionInspector
  {
    private static Logger s_logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    ///   Runs the SolutionInspector with the given console <paramref name="args" />.
    /// </summary>
    public static int Run (string[] args)
    {
      s_logger.Debug($"SolutionInspector was run with the following arguments: [{string.Join(", ", args)}].");

      using (var container = SetupContainer())
      {
        var commands = container.Resolve<IEnumerable<ConsoleCommand>>();
        return ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
      }
    }

    private static IContainer SetupContainer ()
    {
      var builder = new ContainerBuilder();

      builder.RegisterType<FileWrap>().As<IFile>();
      builder.RegisterType<DirectoryWrap>().As<IDirectory>();
      builder.RegisterType<AssemblyWrap>().As<IAssembly>();
      builder.RegisterType<ConfigurationManagerWrap>().As<IConfigurationManager>();

      builder.RegisterType<SolutionLoader>().As<ISolutionLoader>();

      builder.RegisterType<RuleAssemblyLoader>().As<IRuleAssemblyLoader>();

      builder.RegisterType<RuleCollectionBuilder>().As<IRuleCollectionBuilder>();
      builder.RegisterType<RuleTypeResolver>().As<IRuleTypeResolver>();

      builder.RegisterType<RuleConfigurationInstantiator>().As<IRuleConfigurationInstantiator>();

      builder.RegisterType<RuleViolationViewModelConverter>().As<IRuleViolationViewModelConverter>();

      builder.Register(
          ctx =>
              new TableWriter(new TableWriterOptions { PreferredTableWidth = 200, Characters = TableWriterCharacters.AdvancedAscii })
          ).As<ITableWriter>();

      builder.RegisterViolationReporter(
          ViolationReportFormat.Table,
          ctx => tw => new TableViolationReporter(tw, ctx.Resolve<IRuleViolationViewModelConverter>(), ctx.Resolve<ITableWriter>()));

      builder.RegisterViolationReporter(
          ViolationReportFormat.Xml,
          ctx => tw => new XmlViolationReporter(tw, ctx.Resolve<IRuleViolationViewModelConverter>()));

      builder.RegisterViolationReporter(
          ViolationReportFormat.VisualStudio,
          ctx => tw => new VisualStudioViolationReporter(tw));

      builder.RegisterType<ViolationReporterFactory>().As<IViolationReporterFactory>();

      builder.RegisterType<InspectCommand>().As<ConsoleCommand>();

      builder.RegisterType<MsBuildInstallationChecker>().As<IMsBuildInstallationChecker>();

      builder.RegisterType<ConfigurationLoader>().As<IConfigurationLoader>();

      return builder.Build();
    }
  }
}