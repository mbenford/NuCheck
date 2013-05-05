using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NuCheck
{
    public class ProjectExtractor
    {        
        private readonly string solutionFile;

        public ProjectExtractor(string solutionFile)
        {
            this.solutionFile = solutionFile;
        }

        public IEnumerable<Project> ExtractAll()
        {
            string content = File.ReadAllText(solutionFile);
            string pattern = @"=\s*""(?<ProjectName>.+?)""\s*,\s*""(?<ProjectFile>.+?)""\s*,\s*""(?<ProjectGUID>.+?)""";

            var matches = Regex.Matches(content, pattern);

            return from Match match in matches 
                   select new Project(match.Groups["ProjectName"].Value, match.Groups["ProjectFile"].Value);
        }
    }
}