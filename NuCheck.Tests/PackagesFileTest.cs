using NUnit.Framework;

namespace NuCheck.Tests
{
    public class PackagesFileTest
    {
        public class Loads_A_Package_File_From_Disk
        {
            private PackagesFile sut;

            [TestFixtureSetUp]
            public void TestFixtureSetUp()
            {
                // Arrange
                string projectFile = "TestData\\Project.csproj";
                
                // Act
                sut = new PackagesFile(projectFile);
            }

            [Test]
            public void Packages_Count_Should_Be_3()
            {
                Assert.AreEqual(3, sut.Packages.Count);
            }

            [Test]
            public void First_Package_Id_Should_Be_Package1()
            {
                Assert.AreEqual("Package1", sut.Packages[0].Id);
            }

            [Test]
            public void Second_Package_Id_Should_Be_Package2()
            {
                Assert.AreEqual("Package2", sut.Packages[1].Id);
            }

            [Test]
            public void Third_Package_Id_Should_Be_Package3()
            {
                Assert.AreEqual("Package3", sut.Packages[2].Id);
            }

            [Test]
            public void First_Package_Version_Should_Be_1_0_0()
            {
                Assert.AreEqual("1.0.0", sut.Packages[0].Version);
            }

            [Test]
            public void Second_Package_Version_Should_Be_1_1_0()
            {
                Assert.AreEqual("1.1.0", sut.Packages[1].Version);
            }

            [Test]
            public void Third_Package_Version_Should_Be_2_0_0()
            {
                Assert.AreEqual("2.0.0", sut.Packages[2].Version);
            }

        }
    }
}
