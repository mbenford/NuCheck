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

            var packagesWithIssues = from package in packagesAggregation.Keys
                                     group package by package.Id
                                     into g
                                     where g.Count() > 1
                                     select new { Id = g.Key, Versions = g.ToList() };

            return packagesWithIssues.SelectMany(package => package.Versions)
                                     .Select(package => new Issue(package, packagesAggregation[package]))
                                     .ToList();
        }
    }
}