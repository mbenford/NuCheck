using System.Collections.Generic;
using System.IO;

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
            string basePath = Path.GetDirectoryName(solutionFile);

            var aggregation = new Dictionary<Package, IEnumerable<Project>>();

            foreach (Project project in projectExtractor.ExtractAll(solutionFile))
            {
                foreach (Package package in packagesFileLoader.Load(Path.Combine(basePath, project.FileName)))
                {
                    if (aggregation.ContainsKey(package))
                    {
                        (aggregation[package] as IList<Project>).Add(project);
                    }
                    else
                    {
                        aggregation.Add(package, new List<Project> { project });
                    }
                }
            }
                     
            return aggregation;
        }
    }
}