using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            var xml = XElement.Load(packageFile);
            return from p in xml.Elements()
                   select new Package((string)p.Attribute("id"), (string)p.Attribute("version"));
        }
    }
}