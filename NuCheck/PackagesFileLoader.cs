using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NuCheck
{
    public class PackagesFileLoader : IPackagesFileLoader
    {
        public IEnumerable<Package> Load(string projectFile)
        {
            string packageFile = Path.Combine(Path.GetDirectoryName(projectFile), "packages.config");

            IEnumerable<Package> packages;

            if (File.Exists(packageFile))
            {
                packages = from p in XElement.Load(packageFile).Elements()
                           select new Package((string)p.Attribute("id"), (string)p.Attribute("version"));

            }
            else
            {
                packages = Enumerable.Empty<Package>();
            }

            return packages;
        }
    }
}