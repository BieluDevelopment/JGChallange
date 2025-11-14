using MartianRobots.Core.Models;
using MartianRobots.Core.Services;
using AwesomeAssertions;
using NSubstitute;

namespace MartianRobots.Tests;

public class RobotServiceUnitTests
{
    private static IWorldService CreateMockWorldService() => Substitute.For<IWorldService>();

    private static IRobotNavigationService CreateMockRobotNavigationService() =>
        Substitute.For<IRobotNavigationService>();

    private static RobotService CreateRobotService(
        IWorldService? worldService = null,
        IRobotNavigationService? navigationService = null)
    {
        worldService ??= CreateMockWorldService();
        navigationService ??= CreateMockRobotNavigationService();
        return new RobotService(worldService, navigationService);
    }

    private static MartianRobot CreateRobot(
        int x = 1,
        int y = 1,
        Direction direction = Direction.N,
        bool lost = false) =>
        new()
        {
            CurrentXPosition = x,
            CurrentYPosition = y,
            Direction = direction,
            Lost = lost,
            LostAtX = -1,
            LostAtY = -1
        };

    [Fact]
    public void CreateRobotReturnsValidRobotWhenPositionIsInBound()
    {
        // Arrange
        var worldService = CreateMockWorldService();
        worldService.IsInBound(1, 1).Returns(true);

        var robotService = CreateRobotService(worldService);

        // Act
        var robot = robotService.CreateRobot(1, 1, Direction.N);

        // Assert
        robot.Should().NotBeNull();
        robot.CurrentXPosition.Should().Be(1);
        robot.CurrentYPosition.Should().Be(1);
        robot.Direction.Should().Be(Direction.N);
        robot.Lost.Should().BeFalse();
        robot.LostAtX.Should().Be(-1);
        robot.LostAtY.Should().Be(-1);
    }

    [Fact]
    public void CreateRobotThrowsWhenPositionIsOutOfBound()
    {
        // Arrange
        var worldService = CreateMockWorldService();
        worldService.IsInBound(10, 10).Returns(false);

        var robotService = CreateRobotService(worldService);

        // Act
        Action act = () => robotService.CreateRobot(10, 10, Direction.E);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Robot position (10, 10) is out of bounds*");
    }

    [Fact]
    public void CreateRobotReturnsRobotWithCorrectDirection()
    {
        // Arrange
        var worldService = CreateMockWorldService();
        worldService.IsInBound(2, 3).Returns(true);

        var robotService = CreateRobotService(worldService);

        // Act
        var robotNorth = robotService.CreateRobot(2, 3, Direction.N);
        var robotEast = robotService.CreateRobot(2, 3, Direction.E);
        var robotSouth = robotService.CreateRobot(2, 3, Direction.S);
        var robotWest = robotService.CreateRobot(2, 3, Direction.W);

        // Assert
        robotNorth.Direction.Should().Be(Direction.N);
        robotEast.Direction.Should().Be(Direction.E);
        robotSouth.Direction.Should().Be(Direction.S);
        robotWest.Direction.Should().Be(Direction.W);
    }

    [Fact]
    public async Task ParseCommandsThrowsWhenRobotIsNull()
    {
        // Arrange
        var robotService = CreateRobotService();

        // Act & Assert
        var act = () => robotService.ParseCommands(null!, "LRF");

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*No current robot set*");
    }

    [Fact]
    public async Task ParseCommandsDoesNotProcessWhenRobotIsLost()
    {
        // Arrange
        var navigationService = CreateMockRobotNavigationService();
        var robotService = CreateRobotService(navigationService: navigationService);

        var lostRobot = CreateRobot(lost: true);

        // Act
        await robotService.ParseCommands(lostRobot, "LRF");

        // Assert
        await navigationService.DidNotReceive().RotateLeft(Arg.Any<MartianRobot>());
        await navigationService.DidNotReceive().RotateRight(Arg.Any<MartianRobot>());
        await navigationService.DidNotReceive().MoveForward(Arg.Any<MartianRobot>());
    }

    [Fact]
    public async Task ParseCommandsExecutesLeftCommand()
    {
        // Arrange
        var navigationService = CreateMockRobotNavigationService();
        var robotService = CreateRobotService(navigationService: navigationService);

        var robot = CreateRobot();

        // Act
        await robotService.ParseCommands(robot, "L");

        // Assert
        await navigationService.Received(1).RotateLeft(robot);
    }

    [Fact]
    public async Task ParseCommandsExecutesRightCommand()
    {
        // Arrange
        var navigationService = CreateMockRobotNavigationService();
        var robotService = CreateRobotService(navigationService: navigationService);

        var robot = CreateRobot();

        // Act
        await  robotService.ParseCommands(robot, "R");

        // Assert
        await   navigationService.Received(1).RotateRight(robot);
    }

    [Fact]
    public async Task ParseCommandsExecutesForwardCommand()
    {
        // Arrange
        var navigationService = CreateMockRobotNavigationService();
        var robotService = CreateRobotService(navigationService: navigationService);

        var robot = CreateRobot();

        // Act
        await   robotService.ParseCommands(robot, "F");

        // Assert
        await navigationService.Received(1).MoveForward(robot);
    }

    [Fact]
    public async Task ParseCommandsExecutesMultipleCommands()
    {
        // Arrange
        var navigationService = CreateMockRobotNavigationService();
        var robotService = CreateRobotService(navigationService: navigationService);

        var robot = CreateRobot();

        // Act
        await   robotService.ParseCommands(robot, "RFRFRFRF");

        // Assert
        await   navigationService.Received(4).RotateRight(robot);
        await   navigationService.Received(4).MoveForward(robot);
    }

    [Fact]
    public async Task ParseCommandsIgnoresCaseInCommands()
    {
        // Arrange
        var navigationService = CreateMockRobotNavigationService();
        var robotService = CreateRobotService(navigationService: navigationService);

        var robot = CreateRobot();

        // Act
        await   robotService.ParseCommands(robot, "lRf");

        // Assert
        await  navigationService.Received(1).RotateLeft(robot);
        await   navigationService.Received(1).RotateRight(robot);
        await  navigationService.Received(1).MoveForward(robot);
    }

    [Fact]
    public async Task ParseCommandsStopsWhenRobotBecomesLost()
    {
        // Arrange
        var navigationService = CreateMockRobotNavigationService();
        var robotService = CreateRobotService(navigationService: navigationService);

        var robot = CreateRobot();

        navigationService.When(x => x.MoveForward(robot))
            .Do(_ => robot.Lost = true);

        // Act
        await    robotService.ParseCommands(robot, "FRRFLLFFRRFLL");

        // Assert
        await   navigationService.Received(1).MoveForward(robot);
        await  navigationService.DidNotReceive().RotateRight(Arg.Any<MartianRobot>());
    }

    [Fact]
    public async Task ParseCommandsProcessesEmptyCommandString()
    {
        // Arrange
        var navigationService = CreateMockRobotNavigationService();
        var robotService = CreateRobotService(navigationService: navigationService);

        var robot = CreateRobot();

        // Act
        await   robotService.ParseCommands(robot, "");

        // Assert
        await    navigationService.DidNotReceive().RotateLeft(Arg.Any<MartianRobot>());
        await   navigationService.DidNotReceive().RotateRight(Arg.Any<MartianRobot>());
        await    navigationService.DidNotReceive().MoveForward(Arg.Any<MartianRobot>());
    }

    [Fact]
    public async Task ParseCommandsIgnoresInvalidCommands()
    {
        // Arrange
        var navigationService = CreateMockRobotNavigationService();
        var robotService = CreateRobotService(navigationService: navigationService);

        var robot = CreateRobot();

        // Act
        await robotService.ParseCommands(robot, "LXF");

        // Assert
        await navigationService.Received(1).RotateLeft(robot);
        await navigationService.Received(1).MoveForward(robot);
    }

    [Fact]
    public async Task ParseCommandsProcessesComplexSequence()
    {
        // Arrange
        var navigationService = CreateMockRobotNavigationService();
        var robotService = CreateRobotService(navigationService: navigationService);

        var robot1 = CreateRobot(x: 1, y: 1, direction: Direction.E);
        var robot2 = CreateRobot(x: 3, y: 2, direction: Direction.N);

        // Act
        await robotService.ParseCommands(robot1, "RFRFRFRF");
        await robotService.ParseCommands(robot2, "FRRFLLFFRRFLL");

        // Assert
        await navigationService.Received(9).MoveForward(Arg.Any<MartianRobot>());

        await navigationService.Received(8).RotateRight(Arg.Any<MartianRobot>());
        await navigationService.Received(4).RotateLeft(Arg.Any<MartianRobot>());
    }

    [Fact]
    public void CreateRobotValidatesPositionWithWorldService()
    {
        // Arrange
        var worldService = CreateMockWorldService();
        var robotService = CreateRobotService(worldService);

        // Act
        try
        {
            robotService.CreateRobot(5, 5, Direction.N);
        }
        catch
        {
            // Expected to potentially throw
        }

        // Assert
        worldService.Received(1).IsInBound(5, 5);
    }

    [Fact]
    public async Task ParseCommandsWithMixedValidAndInvalidCommands()
    {
        // Arrange
        var navigationService = CreateMockRobotNavigationService();
        var robotService = CreateRobotService(navigationService: navigationService);

        var robot = CreateRobot();

        // Act
        await  robotService.ParseCommands(robot, "LXRYFZL");

        // Assert
        await  navigationService.Received(2).RotateLeft(robot);
        await  navigationService.Received(1).RotateRight(robot);
    }
}
