using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NuCheck.Tests
{
    public class PackagesFileLoaderTest
    {
        public class Loads_A_Package_File_From_Disk
        {
            private IEnumerable<Package> result;

            [TestFixtureSetUp]
            public void TestFixtureSetUp()
            {
                // Arrange
                string projectFile = "TestData\\Project.csproj";

                var sut = new PackagesFileLoader();
                
                // Act
                result = sut.Load(projectFile);
            }

            [Test]
            public void Packages_Count_Should_Be_3()
            {
                Assert.AreEqual(3, result.Count());
            }

            [Test]
            public void First_Package_Id_Should_Be_Package1()
            {
                Assert.AreEqual("Package1", result.ElementAt(0).Id);
            }

            [Test]
            public void Second_Package_Id_Should_Be_Package2()
            {
                Assert.AreEqual("Package2", result.ElementAt(1).Id);
            }

            [Test]
            public void Third_Package_Id_Should_Be_Package3()
            {
                Assert.AreEqual("Package3", result.ElementAt(2).Id);
            }

            [Test]
            public void First_Package_Version_Should_Be_1_0_0()
            {
                Assert.AreEqual("1.0.0", result.ElementAt(0).Version);
            }

            [Test]
            public void Second_Package_Version_Should_Be_1_1_0()
            {
                Assert.AreEqual("1.1.0", result.ElementAt(1).Version);
            }

            [Test]
            public void Third_Package_Version_Should_Be_2_0_0()
            {
                Assert.AreEqual("2.0.0", result.ElementAt(2).Version);
            }

        }
    }
}
