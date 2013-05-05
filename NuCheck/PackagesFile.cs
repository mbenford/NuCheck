using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NuCheck
{
    public class PackagesFile
    {
        public PackagesFile(string projectFile)
        {
            LoadFile(projectFile);
        }

        private void LoadFile(string projectFile)
        {
            string packageFile = Path.Combine(Path.GetDirectoryName(projectFile), "packages.config");
            
            var xml = XElement.Load(packageFile);
            var packages = from p in xml.Elements()
                           select new Package((string)p.Attribute("id"), (string)p.Attribute("version"));

            Packages = new ReadOnlyCollection<Package>(packages.ToList());
        }

        public ReadOnlyCollection<Package> Packages { get; private set; }
    }
}