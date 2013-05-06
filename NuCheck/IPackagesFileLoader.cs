using System.Collections.Generic;

namespace NuCheck
{
    public interface IPackagesFileLoader
    {
        IEnumerable<Package> Load(string projectFile);
    }
}