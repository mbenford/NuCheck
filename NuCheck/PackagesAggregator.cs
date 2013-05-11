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

        public IDictionary<Package, IEnumerable<Project>> Aggregate(string solutionFile)
        {
            var a = new Dictionary<Package, IEnumerable<Project>>();
            a.Add(new Package("P1", "1.0.0"), new[] { new Project("PROJ1", "project1.csproj"), new Project("PROJ2", "project2.csproj") });
            a.Add(new Package("P2", "1.0.0"), new[] { new Project("PROJ1", "project1.csproj") });
            a.Add(new Package("P2", "1.1.0"), new[] { new Project("PROJ2", "project2.csproj") });

            projectExtractor.ExtractAll(solutionFile);
            packagesFileLoader.Load("project1.csproj");
            packagesFileLoader.Load("project2.csproj");            

            return a;            
        }
    }
}