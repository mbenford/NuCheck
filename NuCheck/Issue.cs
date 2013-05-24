using System.Collections.Generic;

namespace NuCheck
{
    public class Issue
    {
        public Issue(string packageId, IDictionary<string, IEnumerable<Project>> versions)
        {
            PackageId = packageId;
            Versions = versions;
        }

        public string PackageId { get; private set; }
        public IDictionary<string, IEnumerable<Project>> Versions { get; private set; }
    }
}