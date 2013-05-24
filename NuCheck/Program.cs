using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NuCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayHelpIfNeeded(args);

            CheckIfSolutionFileExists(args[0]);            

            AnalyzeSolution(args[0]);
        }

        private static void DisplayHelpIfNeeded(string[] args)
        {
            if (!args.Any())
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

                Console.WriteLine("NuCheck Version: {0}", versionInfo.FileVersion);
                Console.WriteLine("usage: NuCheck <solution-file>");
                Environment.Exit(1);
            }
        }

        private static void CheckIfSolutionFileExists(string solutionFile)
        {
            if (!File.Exists(solutionFile))
            {
                Console.Error.WriteLine("File {0} not found", solutionFile);
                Environment.Exit(1);
            }
        }

        private static void AnalyzeSolution(string solutionFile)
        {
            SolutionAnalyzer analyzer = CreateSolutionAnalyzer();
            IEnumerable<Issue> issues = analyzer.GetIssues(solutionFile);

            if (issues.Any())
            {
                Console.WriteLine("{0} issues found", issues.Count());

                foreach (var issue in issues.OrderBy(issue => issue.PackageId))
                {
                    Console.WriteLine("\n{0} ({1} versions)", issue.PackageId, issue.Versions.Keys.Count);
                    
                    foreach (var version in issue.Versions.OrderBy(version => version.Key))
                    {
                        Console.WriteLine("=> {0} ({1} {2})", version.Key, version.Value.Count(), 
                            version.Value.Count() > 1 ? "projects" : "project");

                        foreach (var project in version.Value.OrderBy(project => project.Name))
                        {
                            Console.WriteLine("   - {0}", project.Name);
                        }
                    }                    
                }

                Environment.Exit(1);
            }

            Console.WriteLine("No issues found");
            Environment.Exit(0);
        }

        private static SolutionAnalyzer CreateSolutionAnalyzer()
        {
            var projectExtractor = new ProjectExtractor();
            var packagesFileLoader = new PackagesFileLoader();
            var packagesAggregator = new PackagesAggregator(projectExtractor, packagesFileLoader);
            return new SolutionAnalyzer(packagesAggregator);
        }
    }
}
