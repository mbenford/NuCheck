using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace NuCheck.Tests
{
    public class PackagesAggregatorTest
    {
        public class Groups_All_Packages_Versions_In_Use_By_Package_Id
        {
            private Mock<IProjectExtractor> projectExtractorMock;
            private Mock<IPackagesFileLoader> packagesFileLoaderMock;
            
            private IDictionary<string, IEnumerable<PackageVersion>> result;

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
            public void Result_Count_Should_Be_2()
            {                
                Assert.AreEqual(2, result.Count);
            }

            [Test]
            public void First_Key_Should_Be_P1()
            {
                Assert.AreEqual("P1", result.Keys.ElementAt(0));
            }

            [Test]
            public void Second_Key_Should_Be_P2()
            {
                Assert.AreEqual("P2", result.Keys.ElementAt(1));
            }

            [Test]
            public void Package_P1_Should_Have_One_Version_In_Use()
            {
                Assert.AreEqual(1, result["P1"].Count());
            }

            [Test]
            public void Package_P1_Version_Should_Be_1_0_0()
            {
                Assert.AreEqual("1.0.0", result["P1"].First().Version);
            }

            [Test]
            public void Package_P1_Should_Be_Used_In_Two_Projects()
            {
                Assert.AreEqual(2, result["P1"].ElementAt(0).Projects.Count());
            }

            [Test]
            public void Package_P1_Should_Be_Used_In_Project_PROJ1()
            {
                Assert.True(result["P1"].First().Projects.Any(p => p.Name == "PROJ1"));
            }

            [Test]
            public void Package_P1_Should_Be_Used_In_Project_PROJ2()
            {
                Assert.True(result["P1"].First().Projects.Any(p => p.Name == "PROJ2"));
            }

            [Test]
            public void Package_P2_Should_Have_Two_Versions_In_Use()
            {
                Assert.AreEqual(2, result["P2"].Count());
            }

            [Test]
            public void Package_P2_First_Version_Should_Be_1_0_0()
            {
                Assert.AreEqual("1.0.0", result["P2"].First().Version);
            }

            [Test]
            public void Package_P2_Second_Version_Should_Be_1_1_0()
            {
                Assert.AreEqual("1.1.0", result["P2"].Last().Version);
            }

            [Test]
            public void Package_P2_Version_1_0_0_Should_Be_Used_In_One_Project()
            {
                Assert.AreEqual(1, result["P2"].First().Projects.Count());
            }

            [Test]
            public void Package_P2_Version_1_1_0_Should_Be_Used_In_One_Project()
            {
                Assert.AreEqual(1, result["P2"].Last().Projects.Count());
            }

            [Test]
            public void Package_P2_Version_1_0_0_Should_Be_Used_In_Project_PROJ1()
            {
                Assert.True(result["P2"].First().Projects.Any(p => p.Name == "PROJ1"));
            }

            [Test]
            public void Package_P2_Version_1_1_0_Should_Be_Used_In_Project_PROJ2()
            {
                Assert.True(result["P2"].Last().Projects.Any(p => p.Name == "PROJ2"));
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
