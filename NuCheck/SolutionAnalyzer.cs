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

        public IEnumerable<Issue> GetIssues(string solutionFile, string pattern = null)
        {
            IDictionary<Package, IEnumerable<Project>> packagesAggregation = packagesAggregator.Aggregate(solutionFile, pattern);

            return from package in packagesAggregation.Keys
                   group package by package.Id
                   into g
                   where g.Count() > 1
                   select new Issue(g.Key, g.ToDictionary(p => p.Version, p => packagesAggregation[p]));
        }
    }
}