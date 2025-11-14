using AwesomeAssertions;
using MartianRobots.Core.Models;
using MartianRobots.Core.Services;
using NSubstitute;

namespace MartianRobots.Tests;

public class ConsoleControllerIntegrationTests
{
    [Fact]
    public async Task ExecuteAsyncProcessesSampleInputAndProducesExpectedOutput()
    {
        // Arrange
        var sampleInput = new[]
        {
            "5 3",
            "1 1 E",
            "RFRFRFRF",
            "3 2 N",
            "FRRFLLFFRRFLL",
            "0 3 W",
            "LLFFFLFLFL",
            "end"
        };

        var inputReader = new StringListConsoleProvider(sampleInput);
        var outputWriter = new StringBuilderConsoleProvider();

        var worldService = new WorldService();
        var robotService = CreateMockRobotService();

        var controller = new ConsoleController(worldService, robotService, new CompositeConsoleProvider(inputReader, outputWriter));

        // Act
        await controller.ExecuteAsync();

        // Assert
        var output = outputWriter.GetOutput();

        output.Should().Contain("1 1 E");
        output.Should().Contain("3 3 N LOST");
        output.Should().Contain("2 3 S");
        output.Should().Contain("Closing Simulation.");
    }

    private static IRobotService CreateMockRobotService()
    {
        var robotService = Substitute.For<IRobotService>();

        var robot1 = new MartianRobot
        {
            CurrentXPosition = 1,
            CurrentYPosition = 1,
            Direction = Direction.E,
            Lost = false
        };

        var robot2 = new MartianRobot
        {
            CurrentXPosition = 3,
            CurrentYPosition = 3,
            Direction = Direction.N,
            Lost = true,
            LostAtX = 3,
            LostAtY = 3
        };

        var robot3 = new MartianRobot
        {
            CurrentXPosition = 2,
            CurrentYPosition = 3,
            Direction = Direction.S,
            Lost = false
        };

        var robots = new Queue<MartianRobot>(new[] { robot1, robot2, robot3 });

        robotService.CreateRobot(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<Direction>())
            .Returns(x => robots.Dequeue());

        return robotService;
    }
}

/// <summary>
/// Console provider that reads from a list of strings.
/// </summary>
public class StringListConsoleProvider : IConsoleProvider
{
    private readonly IEnumerator<string> _lines;

    public StringListConsoleProvider(string[] lines)
    {
        _lines = ((IEnumerable<string>)lines).GetEnumerator();
    }

    public string? ReadLine()
    {
        return _lines.MoveNext() ? _lines.Current : null;
    }

    public void WriteLine(string? message)
    {
        // Ignored in this provider (use CompositeConsoleProvider to capture output).
    }
}

/// <summary>
/// Console provider that captures output to a StringBuilder.
/// </summary>
public class StringBuilderConsoleProvider : IConsoleProvider
{
    private readonly System.Text.StringBuilder _output = new();

    public string? ReadLine()
    {
        // This provider only writes, doesn't read.
        return null;
    }

    public void WriteLine(string? message)
    {
        _output.AppendLine(message);
    }

    public string GetOutput() => _output.ToString();
}

/// <summary>
/// Composite provider that reads from one and writes to another.
/// </summary>
public class CompositeConsoleProvider : IConsoleProvider
{
    private readonly IConsoleProvider _reader;
    private readonly IConsoleProvider _writer;

    public CompositeConsoleProvider(IConsoleProvider reader, IConsoleProvider writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public string? ReadLine() => _reader.ReadLine();

    public void WriteLine(string? message) => _writer.WriteLine(message);
}
