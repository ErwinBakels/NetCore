using System.Collections;
using System.Diagnostics.CodeAnalysis;
using DSharp.NetCore.Extensions;
using DSharp.NetCore.IO;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSharp.NetCore.SystemTests;

[ExcludeFromCodeCoverage]
[TestClass]
public class DDirectoryTests
{
    private static readonly string TestFolderPath =  Path.Combine(Directory.GetCurrentDirectory(), $"TestFolder{Guid.NewGuid()}");

    [TestInitialize]
    public void TestInitialize()
    {
        // Create a test folder before each test
        Directory.CreateDirectory(TestFolderPath);

        
    }

    [TestCleanup]
    public void TestCleanup()
    {
        // Clean up the test folder after each test
        Directory.Delete(TestFolderPath, true);
    }

    [TestMethod]
    public void CleanUp_NonexistentDirectory_CreatesDirectory()
    {
        // Act
        var result = DDirectory.CleanUp(TestFolderPath);

        // Assert
        result.Should().BeTrue();
        Directory.Exists(TestFolderPath).Should().BeTrue();
    }

    [TestMethod]
    public void CleanUp_ExistingDirectory_DeletesFiles()
    {
        // Arrange
        File.WriteAllText(Path.Combine(TestFolderPath, "file1.txt"), "Test content");
        File.WriteAllText(Path.Combine(TestFolderPath, "file2.txt"), "Test content");

        // Act
        var result = DDirectory.CleanUp(TestFolderPath);

        // Assert
        result.Should().BeTrue();
        Directory.GetFiles(TestFolderPath).Should().BeEmpty();
    }

    [TestMethod]
    public void GetFiles_ReturnsCorrectFiles()
    {
        // Arrange
        File.WriteAllText(Path.Combine(TestFolderPath, "file1.txt"), "Test content");
        File.WriteAllText(Path.Combine(TestFolderPath, "file2.txt"), "Test content");

        // Act
        var files = DDirectory.GetFiles(TestFolderPath, SearchOption.TopDirectoryOnly, "*.txt");

        // Assert
        files.Should().HaveCount(2);
        files.Should().Contain(Path.Combine(TestFolderPath, "file1.txt"));
        files.Should().Contain(Path.Combine(TestFolderPath, "file2.txt"));
    }

    [TestMethod]
    public void ReplaceTokens_NoMatchingFiles_DoesNotThrow()
    {
        // Act
        Action act = () => DDirectory.ReplaceTokens(TestFolderPath, "*.txt");

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void ReplaceTokens_ReplacesTokensInFiles()
    {
        // Arrange
        var filePath = Path.Combine(TestFolderPath, "test.txt");
        File.WriteAllText(filePath, "Hello #{USERNAME}#!");

        // Act
        DDirectory.ReplaceTokens(TestFolderPath, "*.txt");

        // Assert
        File.ReadAllText(filePath).Should().Be($"Hello {Environment.UserName}!");
    }

    [TestMethod]
    public void ReplaceTokens_ReplacesTokensInString()
    {
        // Arrange
        const string content = "Hello, #{USERNAME}#!";
        var tokenDictionary = new Hashtable { { "USERNAME", "John" } };

        // Act
        var result = content.ReplaceTokens(tokenDictionary);

        // Assert
        result.Should().Be("Hello, John!");
    }

    [TestMethod]
    public void ReplaceTokens_JsonFile_ReplacesTokensInString()
    {
        // Arrange
        const string content = "{ \"name\": \"#{USERNAME}#\", \"age\": #{AGE}# }";
        var tokenDictionary = new Hashtable { { "USERNAME", "Alice" }, { "AGE", 25 } };

        // Act
        var result = content.ReplaceTokens(tokenDictionary);

        // Assert
        result.Should().Be("{ \"name\": \"Alice\", \"age\": 25 }");
    }

    [TestMethod]
    public void ReplaceTokens_XmlFile_ReplacesTokensInString()
    {
        // Arrange
        const string content = "<root><name>#{USERNAME}#</name><age>#{AGE}#</age></root>";
        var tokenDictionary = new Hashtable { { "USERNAME", "Bob" }, { "AGE", 30 } };

        // Act
        var result = content.ReplaceTokens(tokenDictionary);

        // Assert
        result.Should().Be("<root><name>Bob</name><age>30</age></root>");
    }

    [TestMethod]
    public void ReplaceTokens_NoTokens_NoChanges()
    {
        // Arrange
        const string content = "Hello, World!";
        var tokenDictionary = new Hashtable();

        // Act
        var result = content.ReplaceTokens(tokenDictionary);

        // Assert
        result.Should().Be("Hello, World!");
    }
}
