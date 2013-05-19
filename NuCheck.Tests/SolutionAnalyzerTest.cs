using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace NuCheck.Tests
{
    public class SolutionAnalyzerTest
    {
        public class Scans_A_Solution_For_Packages_Issues
        {
            [Test]
            public void Result_Should_Be_Empty_When_There_Is_Only_One_Version_Of_Each_Package_In_Use()
            {
                // Arrange
                var packageAggregation = new Dictionary<Package, IEnumerable<Project>>
                    {
                        { new Package("P1", "1.0.0"), new Project[] { } },
                        { new Package("P2", "1.0.0"), new Project[] { } },
                        { new Package("P3", "1.0.0"), new Project[] { } }
                    };

                var packageAggregatorMock = new Mock<IPackagesAggregator>();
                packageAggregatorMock.Setup(m => m.Aggregate("solution.sln"))
                                     .Returns(packageAggregation);

                var sut = new SolutionAnalyzer(packageAggregatorMock.Object);

                // Act
                IEnumerable<Issue> result = sut.GetIssues("solution.sln");

                // Assert
                result.Should().BeEmpty();
            }

            [Test]
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

                var packageAggregatorMock = new Mock<IPackagesAggregator>();
                packageAggregatorMock.Setup(m => m.Aggregate("solution.sln"))
                                     .Returns(packageAggregation);

                var sut = new SolutionAnalyzer(packageAggregatorMock.Object);

                // Act
                IEnumerable<Issue> result = sut.GetIssues("solution.sln");

                // Assert
                IEnumerable<Issue> expected = new[]
                    {
                        new Issue(package1, new[] { project1 }), 
                        new Issue(package2, new[] { project2 }), 
                        new Issue(package3, new[] { project1 }), 
                        new Issue(package4, new[] { project2 }), 
                    };

                result.ShouldBeEquivalentTo(expected);
            }

        }
    }
}
