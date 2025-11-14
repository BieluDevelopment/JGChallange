using MartianRobots.Tests.TestHelpers;
using AwesomeAssertions;
namespace MartianRobots.Tests;

public class ConsoleProviderTests
{
    private static StringListConsoleProvider CreateStringListProvider(string[] lines)
    {
        return new StringListConsoleProvider(lines);
    }

    private static StringBuilderConsoleProvider CreateStringBuilderProvider()
    {
        return new StringBuilderConsoleProvider();
    }

    [Fact]
    public void StringListProviderReadsFirstLine()
    {
        // Arrange
        var lines = new[] { "5 3", "1 1 E", "RFRFRFRF" };
        var provider = CreateStringListProvider(lines);

        // Act
        var line = provider.ReadLine();

        // Assert
        line.Should().Be("5 3");
    }

    [Fact]
    public void StringListProviderReadsMultipleLines()
    {
        // Arrange
        var lines = new[] { "5 3", "1 1 E", "RFRFRFRF" };
        var provider = CreateStringListProvider(lines);

        // Act
        var line1 = provider.ReadLine();
        var line2 = provider.ReadLine();
        var line3 = provider.ReadLine();

        // Assert
        line1.Should().Be("5 3");
        line2.Should().Be("1 1 E");
        line3.Should().Be("RFRFRFRF");
    }

    [Fact]
    public void StringListProviderReturnsNullAfterAllLinesRead()
    {
        // Arrange
        var lines = new[] { "5 3", "1 1 E" };
        var provider = CreateStringListProvider(lines);

        // Act
        provider.ReadLine();
        provider.ReadLine();
        var nextLine = provider.ReadLine();

        // Assert
        nextLine.Should().BeNull();
    }

    [Fact]
    public void StringListProviderReturnsNullForEmptyArray()
    {
        // Arrange
        var lines = Array.Empty<string>();
        var provider = CreateStringListProvider(lines);

        // Act
        var line = provider.ReadLine();

        // Assert
        line.Should().BeNull();
    }

    [Fact]
    public void StringBuilderProviderWritesSingleLine()
    {
        // Arrange
        var provider = CreateStringBuilderProvider();
        const string message = "1 1 E";

        // Act
        provider.WriteLine(message);
        var output = provider.GetOutput();

        // Assert
        output.Should().Contain(message);
    }

    [Fact]
    public void StringBuilderProviderWritesMultipleLines()
    {
        // Arrange
        var provider = CreateStringBuilderProvider();

        // Act
        provider.WriteLine("1 1 E");
        provider.WriteLine("3 3 N LOST");
        provider.WriteLine("2 3 S");
        var output = provider.GetOutput();

        // Assert
        output.Should().Contain("1 1 E");
        output.Should().Contain("3 3 N LOST");
        output.Should().Contain("2 3 S");
    }

    [Fact]
    public void StringBuilderProviderWritesNullMessage()
    {
        // Arrange
        var provider = CreateStringBuilderProvider();

        // Act
        provider.WriteLine(null);
        var output = provider.GetOutput();

        // Assert
        output.Should().NotBeEmpty();
    }

    [Fact]
    public void StringBuilderProviderReturnsEmptyStringInitially()
    {
        // Arrange
        var provider = CreateStringBuilderProvider();

        // Act
        var output = provider.GetOutput();

        // Assert
        output.Should().BeEmpty();
    }

    [Fact]
    public void StringBuilderProviderReadLineReturnsNull()
    {
        // Arrange
        var provider = CreateStringBuilderProvider();

        // Act
        var line = provider.ReadLine();

        // Assert
        line.Should().BeNull();
    }

    [Fact]
    public void CompositeProviderReadsFromReader()
    {
        // Arrange
        var lines = new[] { "5 3", "1 1 E" };
        var reader = CreateStringListProvider(lines);
        var writer = CreateStringBuilderProvider();
        var provider = new CompositeConsoleProvider(reader, writer);

        // Act
        var line = provider.ReadLine();

        // Assert
        line.Should().Be("5 3");
    }

    [Fact]
    public void CompositeProviderWritesToWriter()
    {
        // Arrange
        var lines = Array.Empty<string>();
        var reader = CreateStringListProvider(lines);
        var writer = CreateStringBuilderProvider();
        var provider = new CompositeConsoleProvider(reader, writer);

        // Act
        provider.WriteLine("1 1 E");
        var output = writer.GetOutput();

        // Assert
        output.Should().Contain("1 1 E");
    }

    [Fact]
    public void CompositeProviderReadsAndWritesCorrectly()
    {
        // Arrange
        var lines = new[] { "5 3", "1 1 E", "RFRFRFRF", "3 2 N" };
        var reader = CreateStringListProvider(lines);
        var writer = CreateStringBuilderProvider();
        var provider = new CompositeConsoleProvider(reader, writer);

        // Act
        var input1 = provider.ReadLine();
        provider.WriteLine("Processing...");
        var input2 = provider.ReadLine();
        provider.WriteLine("Done");
        var input3 = provider.ReadLine();

        var output = writer.GetOutput();

        // Assert
        input1.Should().Be("5 3");
        input2.Should().Be("1 1 E");
        input3.Should().Be("RFRFRFRF");
        output.Should().Contain("Processing...");
        output.Should().Contain("Done");
    }

    [Fact]
    public void StringListProviderHandlesWhitespace()
    {
        // Arrange
        var lines = new[] { "  5 3  ", "\t1 1 E\t", "" };
        var provider = CreateStringListProvider(lines);

        // Act
        var line1 = provider.ReadLine();
        var line2 = provider.ReadLine();
        var line3 = provider.ReadLine();

        // Assert
        line1.Should().Be("  5 3  ");
        line2.Should().Be("\t1 1 E\t");
        line3.Should().Be("");
    }

    [Fact]
    public void StringBuilderProviderCumulatesOutput()
    {
        // Arrange
        var provider = CreateStringBuilderProvider();

        // Act
        provider.WriteLine("Line 1");
        var output1 = provider.GetOutput();
        provider.WriteLine("Line 2");
        var output2 = provider.GetOutput();

        // Assert
        output1.Should().Contain("Line 1");
        output2.Should().Contain("Line 1");
        output2.Should().Contain("Line 2");
    }

    [Fact]
    public void StringListProviderWithSingleLine()
    {
        // Arrange
        var lines = new[] { "end" };
        var provider = CreateStringListProvider(lines);

        // Act
        var line = provider.ReadLine();
        var nextLine = provider.ReadLine();

        // Assert
        line.Should().Be("end");
        nextLine.Should().BeNull();
    }
}
