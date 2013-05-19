using System.Collections.Generic;

namespace NuCheck
{
    public class Issue
    {
        public Issue(Package package, IEnumerable<Project> projects)
        {
            Package = package;
            Projects = projects;
        }

        public Package Package { get; private set; }
        public IEnumerable<Project> Projects { get; private set; }
    }
}