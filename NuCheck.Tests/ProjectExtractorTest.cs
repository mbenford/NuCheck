using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
}
