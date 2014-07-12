using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Xunit;

namespace NuCheck.Tests
{
    public class SolutionAnalyzerTest
    {
        public class Scans_A_Solution_For_Packages_Issues
        {
            private readonly Mock<IPackagesAggregator> packageAggregatorMock;
            private readonly SolutionAnalyzer sut;

            public Scans_A_Solution_For_Packages_Issues()
            {
                packageAggregatorMock = new Mock<IPackagesAggregator>();
                sut = new SolutionAnalyzer(packageAggregatorMock.Object);
            }

            [Fact]
            public void Result_Should_Be_Empty_When_There_Is_Only_One_Version_Of_Each_Package_In_Use()
            {
                // Arrange
                var packageAggregation = new Dictionary<Package, IEnumerable<Project>>
                {
                    { new Package("P1", "1.0.0"), new Project[] { } },
                    { new Package("P2", "1.0.0"), new Project[] { } },
                    { new Package("P3", "1.0.0"), new Project[] { } }
                };

                packageAggregatorMock.Setup(m => m.Aggregate("solution.sln", null))
                                     .Returns(packageAggregation);

                // Act
                IEnumerable<Issue> result = sut.GetIssues("solution.sln");

                // Assert
                result.Should().BeEmpty();
            }

            [Fact]
            public void Result_Should_Contain_All_Packages_With_More_Than_One_Version_In_Use()
            {
                // Arrange
                var package1 = new Package("P1", "1.0.0");
                var package2 = new Package("P1", "1.1.0");
                var package3 = new Package("P2", "1.0.0");
                var package4 = new Package("P2", "2.0.0");
                var package5 = new Package("P3", "1.0.0");

                var project1 = new Project("PROJ1", "PROJ1.csproj");
                var project2 = new Project("PROJ2", "PROJ2.csproj");

                var packageAggregation = new Dictionary<Package, IEnumerable<Project>>
                {
                    { package1, new[] { project1 } },
                    { package2, new[] { project2 } },
                    { package3, new[] { project1 } },
                    { package4, new[] { project2 } },
                    { package5, new[] { project1 } }
                };

                packageAggregatorMock.Setup(m => m.Aggregate("solution.sln", null))
                                     .Returns(packageAggregation);

                // Act
                IEnumerable<Issue> result = sut.GetIssues("solution.sln");

                // Assert
                var versionsForP1 = new Dictionary<string, IEnumerable<Project>>
                {
                    { "1.0.0", new[] { project1 } },
                    { "1.1.0", new[] { project2 } },
                };
                var versionsForP2 = new Dictionary<string, IEnumerable<Project>>
                {
                    { "1.0.0", new[] { project1 } },
                    { "2.0.0", new[] { project2 } },
                };

                IEnumerable<Issue> expected = new[]
                {
                    new Issue("P1", versionsForP1),
                    new Issue("P2", versionsForP2)
                };

                result.ShouldBeEquivalentTo(expected);
            }

            [Fact]
            public void Pattern_Should_Be_Passed_To_Package_Aggregator()
            {
                // Arrange
                packageAggregatorMock.Setup(m => m.Aggregate("solution.sln", "pattern"))
                                     .Returns(new Dictionary<Package, IEnumerable<Project>>());

                // Act
                sut.GetIssues("solution.sln", "pattern");

                // Assert
                packageAggregatorMock.VerifyAll();
            }
        }
    }
}