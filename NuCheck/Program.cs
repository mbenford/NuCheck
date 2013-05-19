using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuCheck
{
    class Program
    {
        static int Main(string[] args)
        {
            int exitCode = 0;

            if (args.Any())
            {
                string solutionFile = args[0];
                SolutionAnalyzer analyzer = CreateSolutionAnalyzer();
                var issues = analyzer.GetIssues(solutionFile);

                if (issues.Any())
                {
                    foreach (Issue issue in issues)
                    {
                        Console.WriteLine("Package {0} {1} is being referenced by the following projects:", issue.Package.Id, issue.Package.Version);
                        foreach (Project project in issue.Projects)
                        {
                            Console.WriteLine(project.Name);
                        }
                    }

                    exitCode = 1;
                }
                else
                {
                    Console.WriteLine("No issues found");
                }
            }

            return exitCode;
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
