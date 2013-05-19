using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace NuCheck.Tests
{
    public class PackagesAggregatorTest
    {
        public class Groups_All_Projects_By_Package_Id_And_Version
        {
            private Mock<IProjectExtractor> projectExtractorMock;
            private Mock<IPackagesFileLoader> packagesFileLoaderMock;
            
            private IDictionary<Package, IEnumerable<Project>> result;

            [TestFixtureSetUp]
            public void TestFixtureSetUp()
            {
                // Arrange
                string solutionFile = "C:\\solution.sln";

                var projects = new[]
                    {
                        new Project("PROJ1", "project1.csproj"), new Project("PROJ2", "project2.csproj"), 
                    };

                string project1FileName = Path.Combine(Path.GetDirectoryName(solutionFile), projects[0].FileName);
                string project2FileName = Path.Combine(Path.GetDirectoryName(solutionFile), projects[1].FileName);

                var packagesOfProject1 = new[] { new Package("P1", "1.0.0"), new Package("P2", "1.0.0") };
                var packagesOfProject2 = new[] { new Package("P1", "1.0.0"), new Package("P2", "1.1.0") };                

                projectExtractorMock = new Mock<IProjectExtractor>();
                projectExtractorMock.Setup(m => m.ExtractAll(solutionFile))
                                    .Returns(projects);

                packagesFileLoaderMock = new Mock<IPackagesFileLoader>();
                packagesFileLoaderMock.Setup(m => m.Load(project1FileName))
                                      .Returns(packagesOfProject1);
                packagesFileLoaderMock.Setup(m => m.Load(project2FileName))
                                      .Returns(packagesOfProject2);

                var sut = new PackagesAggregator(projectExtractorMock.Object, packagesFileLoaderMock.Object);

                // Act
                result = sut.Aggregate(solutionFile);
            }

            [Test]
            public void Result_Keys_Should_Match_Expected_Keys()
            {
                var expectedKeys = new[]
                    {
                        new Package("P1", "1.0.0"),
                        new Package("P2", "1.0.0"),
                        new Package("P2", "1.1.0"),
                    };

                result.Keys.ShouldBeEquivalentTo(expectedKeys);
            }

            [Test]
            public void Result_Values_Should_Match_Expected_Values()
            {
                var expectedValues = new[]
                    {
                        new[] { new Project("PROJ1", "project1.csproj"), new Project("PROJ2", "project2.csproj") },
                        new[] { new Project("PROJ1", "project1.csproj") },
                        new[] { new Project("PROJ2", "project2.csproj") },
                    };

                result.Values.ShouldBeEquivalentTo(expectedValues);
            }

            [Test]
            public void IProjectExtractor_ExtractAll_Should_Be_Called_Once()
            {
                projectExtractorMock.Verify(m => m.ExtractAll(It.IsAny<string>()), Times.Once());
            }

            [Test]
            public void IPackagesFileLoader_Load_Should_Be_Called_Twice()
            {
                packagesFileLoaderMock.Verify(m => m.Load(It.IsAny<string>()), Times.Exactly(2));
            }
        }
    }
}
