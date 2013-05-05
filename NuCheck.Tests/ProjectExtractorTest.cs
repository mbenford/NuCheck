using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NuCheck.Tests
{
    public class ProjectExtractorTest
    {
        public class Extracts_All_Projects_From_A_Solution_File
        {
            private IEnumerable<Project> result;

            [TestFixtureSetUp]
            public void TestFixtureSetUp()
            {
                // Arrange
                string solutionFile = "Sample.sln";

                var sut = new ProjectExtractor(solutionFile);

                // Act
                result = sut.ExtractAll();
            }

            [Test]
            public void Result_Should_Not_Be_Null()
            {
                Assert.NotNull(result);
            }

            [Test]
            public void Result_Should_Contain_3_Elements()
            {
                Assert.AreEqual(3, result.Count());
            }

            [Test]
            public void First_Element_Name_Should_Be_Project1()
            {
                Assert.AreEqual("Project1", result.ElementAt(0).Name);
            }

            [Test]
            public void Second_Element_Name_Should_Be_Project2()
            {
                Assert.AreEqual("Project2", result.ElementAt(1).Name);
            }

            [Test]
            public void Third_Element_Name_Should_Be_Project3()
            {
                Assert.AreEqual("Project3", result.ElementAt(2).Name);
            }

            [Test]
            public void First_Element_FileName_Should_Be_Project1_dot_csproj()
            {
                Assert.AreEqual("Project1.csproj", result.ElementAt(0).FileName);
            }

            [Test]
            public void Second_Element_FileName_Should_Be_Project2_dot_csproj()
            {
                Assert.AreEqual("Project2.csproj", result.ElementAt(1).FileName);
            }

            [Test]
            public void Third_Element_FileName_Should_Be_Project2_dot_csproj()
            {
                Assert.AreEqual("Project3.csproj", result.ElementAt(2).FileName);
            }
        }
    }

    public class Project
    {
        private readonly string name;
        private readonly string fileName;

        public Project(string name, string fileName)
        {
            this.name = name;
            this.fileName = fileName;
        }

        public string Name { get { return name; } }

        public string FileName { get { return fileName; } }
    }

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
