using AwesomeAssertions;
using MartianRobots.Core.Models;
using MartianRobots.Core.Services;
using MartianRobots.Tests.TestHelpers;
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
