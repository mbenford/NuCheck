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
                Console.WriteLine("{0} issues found\n", issues.Count());

                foreach (Issue issue in issues)
                {
                    Console.WriteLine("{0} {1} is being used by {2}", issue.Package.Id, issue.Package.Version,
                        String.Join(", ", issue.Projects.Select(p => p.Name)));                    
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
