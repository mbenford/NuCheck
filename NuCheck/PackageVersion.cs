using System.Collections.Generic;

namespace NuCheck
{
    public class PackageVersion
    {
        public PackageVersion(string version, IEnumerable<Project> projects)
        {
            Version = version;
            Projects = projects;
        }

        public string Version { get; set; }

        public IEnumerable<Project> Projects { get; set; }
    }
}