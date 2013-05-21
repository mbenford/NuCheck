using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace NuCheck.Tests
{
    public class ProjectExtractorTest
    {
        public class Extracts_All_Projects_From_A_Solution_File
        {
            [Fact]
            public void Returns_All_Projects_Found_In_A_Solution_File()
            {
                // Arrange
                string solutionFile = "TestData\\solution.sln";

                var sut = new ProjectExtractor();

                // Act
                IEnumerable<Project> result = sut.ExtractAll(solutionFile);

                // Assert
                var expected = new[]
                    {
                        new Project("Project1", "Project1.csproj"),
                        new Project("Project2", "Project2.csproj"),
                        new Project("Project3", "Project3.csproj"),
                    };

                result.ShouldBeEquivalentTo(expected);
            }
        }
    }
}
