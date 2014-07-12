using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NuCheck
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DisplayHelpIfNeeded(args);

            string solutionFile = args[0];
            string pattern = args.ElementAtOrDefault(1);

            CheckIfSolutionFileExists(solutionFile);
            AnalyzeSolution(solutionFile, pattern);
        }

        private static void DisplayHelpIfNeeded(IEnumerable<string> args)
        {
            if (!args.Any() || args.Count() > 2)
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

                WriteLine("NuCheck Version: {0}", versionInfo.FileVersion);
                WriteLine("usage: NuCheck <solution-file> [pattern]\r\n\r\n" +
                          "solution-file     Solution file to be analyzed.\r\n" +
                          "pattern           Pattern to be used to select a subset of the packages in use. Wildcards are supported.");

                Exit(1);
            }
        }

        private static void CheckIfSolutionFileExists(string solutionFile)
        {
            if (!File.Exists(solutionFile))
            {
                WriteLine("File {0} not found", solutionFile);
                Exit(1);
            }
        }

        private static void AnalyzeSolution(string solutionFile, string pattern)
        {
            SolutionAnalyzer analyzer = CreateSolutionAnalyzer();
            IEnumerable<Issue> issues = analyzer.GetIssues(solutionFile, pattern);

            if (issues.Any())
            {
                WriteLine("{0} issues found", issues.Count());

                foreach (var issue in issues.OrderBy(issue => issue.PackageId))
                {
                    WriteLine("\n{0} ({1} versions)", issue.PackageId, issue.Versions.Keys.Count);

                    foreach (var version in issue.Versions.OrderBy(version => version.Key))
                    {
                        WriteLine("=> {0} ({1} {2})", version.Key, version.Value.Count(),
                            version.Value.Count() > 1 ? "projects" : "project");

                        foreach (var project in version.Value.OrderBy(project => project.Name))
                        {
                            WriteLine("   - {0}", project.Name);
                        }
                    }
                }

                Exit(1);
            }

            WriteLine("No issues found");
            Exit(0);
        }

        private static SolutionAnalyzer CreateSolutionAnalyzer()
        {
            var projectExtractor = new ProjectExtractor();
            var packagesFileLoader = new PackagesFileLoader();
            var packagesAggregator = new PackagesAggregator(projectExtractor, packagesFileLoader);
            return new SolutionAnalyzer(packagesAggregator);
        }

        private static void WriteLine(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        private static void Exit(int exitCode)
        {
            Environment.Exit(exitCode);
        }
    }
}