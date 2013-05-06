using System.Collections.Generic;

namespace NuCheck
{
    public interface IProjectExtractor
    {
        IEnumerable<Project> ExtractAll(string solutionFile);
    }
}