using System.Collections.Generic;

namespace NuCheck
{
    public interface IPackagesAggregator
    {
        IDictionary<Package, IEnumerable<Project>> Aggregate(string solutionFile);
    }
}