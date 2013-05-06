using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NuCheck
{
    public class ProjectExtractor : IProjectExtractor
    {        
        public IEnumerable<Project> ExtractAll(string solutionFile)
        {
            string content = File.ReadAllText(solutionFile);
            string pattern = @"=\s*""(?<ProjectName>.+?)""\s*,\s*""(?<ProjectFile>.+?)""\s*,\s*""(?<ProjectGUID>.+?)""";

            var matches = Regex.Matches(content, pattern);

            return from Match match in matches 
                   select new Project(match.Groups["ProjectName"].Value, match.Groups["ProjectFile"].Value);
        }
    }
}