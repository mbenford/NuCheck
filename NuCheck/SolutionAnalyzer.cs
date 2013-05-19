using System.Collections.Generic;
using System.Linq;

namespace NuCheck
{
    public class SolutionAnalyzer
    {
        private readonly IPackagesAggregator packagesAggregator;

        public SolutionAnalyzer(IPackagesAggregator packagesAggregator)
        {
            this.packagesAggregator = packagesAggregator;
        }

        public IEnumerable<Issue> GetIssues(string solutionFile)
        {
            IDictionary<Package, IEnumerable<Project>> packagesAggregation = packagesAggregator.Aggregate(solutionFile);

            var packageGroup = from package in packagesAggregation.Keys
                               group package by package.Id
                               into g
                               select new { Id = g.Key, Versions = g.ToList() };

            var packageWithIssues = from package in packageGroup
                                    where package.Versions.Count() > 1
                                    select package.Versions;
            
            return packageWithIssues.SelectMany(package => package)
                                    .Select(package => new Issue(package, packagesAggregation[package]))
                                    .ToList();
        }
    }
}