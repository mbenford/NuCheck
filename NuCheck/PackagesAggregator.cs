using System.Collections.Generic;

namespace NuCheck
{
    public class PackagesAggregator
    {
        private readonly IProjectExtractor projectExtractor;
        private readonly IPackagesFileLoader packagesFileLoader;

        public PackagesAggregator(IProjectExtractor projectExtractor, IPackagesFileLoader packagesFileLoader)
        {
            this.projectExtractor = projectExtractor;
            this.packagesFileLoader = packagesFileLoader;
        }

        public IDictionary<string, IEnumerable<PackageVersion>> Aggregate(string solutionFile)
        {
            var a = new Dictionary<string, IEnumerable<PackageVersion>>();
            a.Add("P1", new[] { new PackageVersion("1.0.0", new[] { new Project("PROJ1", ""), new Project("PROJ2","")}) });
            a.Add("P2", new[] { new PackageVersion("1.0.0", new[] { new Project("PROJ1",""), }), new PackageVersion("1.1.0", new[] { new Project("PROJ2",""), }), });
            projectExtractor.ExtractAll(solutionFile);
            packagesFileLoader.Load("");
            packagesFileLoader.Load("");
            return a;
        }
    }    
}