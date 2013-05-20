using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace NuCheck.Tests
{
    public class PackagesFileLoaderTest
    {
        public class Loads_A_Package_File_From_Disk
        {
            [Test]
            public void Returns_A_List_Containing_All_Packages_In_A_File()
            {
                // Arrange
                string projectFile = "TestData\\Project.csproj";

                var sut = new PackagesFileLoader();
                
                // Act
                IEnumerable<Package> result = sut.Load(projectFile);

                // Assert
                var expected = new[]
                    {
                        new Package("Package1", "1.0.0"),
                        new Package("Package2", "1.1.0"),
                        new Package("Package3", "2.0.0"),
                    };

                result.ShouldBeEquivalentTo(expected);
            }

            [Test]
            public void Returns_An_Empty_List_When_No_Package_File_Is_Found()
            {
                // Arrange
                string projectFile = "FakeTestData\\Project.csproj";

                var sut = new PackagesFileLoader();

                // Act
                IEnumerable<Package> result = sut.Load(projectFile);

                // Assert                
                result.Should().BeEmpty();
            }
        }
    }
}
