using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace NuCheck.Tests
{
    public class PackagesAggregatorTest
    {
        public class Groups_All_Projects_By_Package_Id_And_Version
        {
            private const string solutionFile = @"C:\solution.sln";

            private Mock<IProjectExtractor> projectExtractorMock;
            private Mock<IPackagesFileLoader> packagesFileLoaderMock;
            
            private readonly PackagesAggregator sut;

            public Groups_All_Projects_By_Package_Id_And_Version()
            {
                var projects = new[]
                {
                    new Project("PROJ1", "project1.csproj"), new Project("PROJ2", "project2.csproj"), 
                };

                string project1FileName = Path.Combine(Path.GetDirectoryName(solutionFile), projects[0].FileName);
                string project2FileName = Path.Combine(Path.GetDirectoryName(solutionFile), projects[1].FileName);

                var packagesOfProject1 = new[] { new Package("Package.P1", "1.0.0"), new Package("Package.P2", "1.0.0") };
                var packagesOfProject2 = new[] { new Package("Package.P1", "1.0.0"), new Package("Package.P2", "1.1.0") };                

                projectExtractorMock = new Mock<IProjectExtractor>();
                projectExtractorMock.Setup(m => m.ExtractAll(solutionFile))
                                    .Returns(projects);

                packagesFileLoaderMock = new Mock<IPackagesFileLoader>();
                packagesFileLoaderMock.Setup(m => m.Load(project1FileName))
                                      .Returns(packagesOfProject1);
                packagesFileLoaderMock.Setup(m => m.Load(project2FileName))
                                      .Returns(packagesOfProject2);

                sut = new PackagesAggregator(projectExtractorMock.Object, packagesFileLoaderMock.Object);
            }

            [Fact]
            public void Groups_All_Packages()
            {
                // Arrange
                var expected = new Dictionary<Package, IEnumerable<Project>>
                {
                    { new Package("Package.P1", "1.0.0"), new[] { new Project("PROJ1", "project1.csproj"), new Project("PROJ2", "project2.csproj") } },
                    { new Package("Package.P2", "1.0.0"), new[] { new Project("PROJ1", "project1.csproj") } },
                    { new Package("Package.P2", "1.1.0"), new[] { new Project("PROJ2", "project2.csproj") } }
                };

                // Act/Assert
                sut.Aggregate(solutionFile).ShouldBeEquivalentTo(expected);
            }

            [Theory]
            [InlineData("Package", null)]
            [InlineData("Package.P3", null)]
            [InlineData("Package.P1", new[] { "Package.P1" })]
            [InlineData("package.p1", new[] { "Package.P1" })]
            [InlineData("Package.P1*", new[] { "Package.P1" })]
            [InlineData("Package*", new[] { "Package.P1", "Package.P2" })]
            [InlineData("Package.P?", new[] { "Package.P1", "Package.P2" })]
            [InlineData("P*.P2", new[] { "Package.P2" })]
            public void Groups_Only_Packages_Whose_Ids_Match_The_Provided_Expression(string expression, string[] packageIds)
            {
                // Arrange
                var data = new Dictionary<Package, IEnumerable<Project>>
                {
                    { new Package("Package.P1", "1.0.0"), new[] { new Project("PROJ1", "project1.csproj"), new Project("PROJ2", "project2.csproj") } },
                    { new Package("Package.P2", "1.0.0"), new[] { new Project("PROJ1", "project1.csproj") } },
                    { new Package("Package.P2", "1.1.0"), new[] { new Project("PROJ2", "project2.csproj") } }
                };

                var expected = data.Where(a => packageIds != null && packageIds.Contains(a.Key.Id));

                // Act/Assert
                sut.Aggregate(solutionFile, expression).ShouldBeEquivalentTo(expected);
            }
        }
    }
}
