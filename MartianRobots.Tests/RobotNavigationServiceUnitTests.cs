using AwesomeAssertions;
using MartianRobots.Core.Models;
using MartianRobots.Core.Services;
using NSubstitute;
using Xunit;

namespace MartianRobots.Tests;

public class RobotNavigationServiceUnitTests
{
    private static IWorldService CreateMockWorldService()
    {
        return Substitute.For<IWorldService>();
    }

    private static RobotNavigationService CreateNavigationService(IWorldService? worldService = null)
    {
        worldService ??= CreateMockWorldService();
        return new RobotNavigationService(worldService);
    }

    private static MartianRobot CreateRobot(int x = 1, int y = 1, Direction direction = Direction.N, bool lost = false)
    {
        return new MartianRobot
        {
            CurrentXPosition = x,
            CurrentYPosition = y,
            Direction = direction,
            Lost = lost,
            LostAtX = -1,
            LostAtY = -1
        };
    }

    [Fact]
    public void RotateLeftChangesDirectionCounterClockwise()
    {
        // Arrange
        var navigationService = CreateNavigationService();
        var robot = CreateRobot(direction: Direction.N);

        // Act
        navigationService.RotateLeft(robot);

        // Assert
        robot.Direction.Should().Be(Direction.W);
    }

    [Fact]
    public void RotateLeftFromWestToSouth()
    {
        // Arrange
        var navigationService = CreateNavigationService();
        var robot = CreateRobot(direction: Direction.W);

        // Act
        navigationService.RotateLeft(robot);

        // Assert
        robot.Direction.Should().Be(Direction.S);
    }

    [Fact]
    public void RotateLeftFromSouthToEast()
    {
        // Arrange
        var navigationService = CreateNavigationService();
        var robot = CreateRobot(direction: Direction.S);

        // Act
        navigationService.RotateLeft(robot);

        // Assert
        robot.Direction.Should().Be(Direction.E);
    }

    [Fact]
    public void RotateLeftFromEastToNorth()
    {
        // Arrange
        var navigationService = CreateNavigationService();
        var robot = CreateRobot(direction: Direction.E);

        // Act
        navigationService.RotateLeft(robot);

        // Assert
        robot.Direction.Should().Be(Direction.N);
    }

    [Fact]
    public void RotateRightChangesDirectionClockwise()
    {
        // Arrange
        var navigationService = CreateNavigationService();
        var robot = CreateRobot(direction: Direction.N);

        // Act
        navigationService.RotateRight(robot);

        // Assert
        robot.Direction.Should().Be(Direction.E);
    }

    [Fact]
    public void RotateRightFromEastToSouth()
    {
        // Arrange
        var navigationService = CreateNavigationService();
        var robot = CreateRobot(direction: Direction.E);

        // Act
        navigationService.RotateRight(robot);

        // Assert
        robot.Direction.Should().Be(Direction.S);
    }

    [Fact]
    public void RotateRightFromSouthToWest()
    {
        // Arrange
        var navigationService = CreateNavigationService();
        var robot = CreateRobot(direction: Direction.S);

        // Act
        navigationService.RotateRight(robot);

        // Assert
        robot.Direction.Should().Be(Direction.W);
    }

    [Fact]
    public void RotateRightFromWestToNorth()
    {
        // Arrange
        var navigationService = CreateNavigationService();
        var robot = CreateRobot(direction: Direction.W);

        // Act
        navigationService.RotateRight(robot);

        // Assert
        robot.Direction.Should().Be(Direction.N);
    }

    [Fact]
    public void RotateLeftThrowsWhenRobotIsNull()
    {
        // Arrange
        var navigationService = CreateNavigationService();

        // Act
        Action act = () => navigationService.RotateLeft(null!);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void RotateRightThrowsWhenRobotIsNull()
    {
        // Arrange
        var navigationService = CreateNavigationService();

        // Act
        Action act = () => navigationService.RotateRight(null!);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void MoveForwardMovesNorthCorrectly()
    {
        // Arrange
        var worldService = CreateMockWorldService();
        worldService.IsInBound(1, 2).Returns(true);

        var navigationService = CreateNavigationService(worldService);
        var robot = CreateRobot(x: 1, y: 1, direction: Direction.N);

        // Act
        navigationService.MoveForward(robot);

        // Assert
        robot.CurrentXPosition.Should().Be(1);
        robot.CurrentYPosition.Should().Be(2);
        robot.Lost.Should().BeFalse();
    }

    [Fact]
    public void MoveForwardMovesEastCorrectly()
    {
        // Arrange
        var worldService = CreateMockWorldService();
        worldService.IsInBound(2, 1).Returns(true);

        var navigationService = CreateNavigationService(worldService);
        var robot = CreateRobot(x: 1, y: 1, direction: Direction.E);

        // Act
        navigationService.MoveForward(robot);

        // Assert
        robot.CurrentXPosition.Should().Be(2);
        robot.CurrentYPosition.Should().Be(1);
        robot.Lost.Should().BeFalse();
    }

    [Fact]
    public void MoveForwardMovesSouthCorrectly()
    {
        // Arrange
        var worldService = CreateMockWorldService();
        worldService.IsInBound(1, 0).Returns(true);

        var navigationService = CreateNavigationService(worldService);
        var robot = CreateRobot(x: 1, y: 1, direction: Direction.S);

        // Act
        navigationService.MoveForward(robot);

        // Assert
        robot.CurrentXPosition.Should().Be(1);
        robot.CurrentYPosition.Should().Be(0);
        robot.Lost.Should().BeFalse();
    }

    [Fact]
    public void MoveForwardMovesWestCorrectly()
    {
        // Arrange
        var worldService = CreateMockWorldService();
        worldService.IsInBound(0, 1).Returns(true);

        var navigationService = CreateNavigationService(worldService);
        var robot = CreateRobot(x: 1, y: 1, direction: Direction.W);

        // Act
        navigationService.MoveForward(robot);

        // Assert
        robot.CurrentXPosition.Should().Be(0);
        robot.CurrentYPosition.Should().Be(1);
        robot.Lost.Should().BeFalse();
    }

    [Fact]
    public void MoveForwardMarksRobotLostWhenMovingOutOfBoundsWithoutScent()
    {
        // Arrange
        var worldService = CreateMockWorldService();
        worldService.IsInBound(1, 4).Returns(false);
        worldService.IsScented(1, 3).Returns(false);

        var navigationService = CreateNavigationService(worldService);
        var robot = CreateRobot(x: 1, y: 3, direction: Direction.N);

        // Act
        navigationService.MoveForward(robot);

        // Assert
        robot.Lost.Should().BeTrue();
        robot.LostAtX.Should().Be(1);
        robot.LostAtY.Should().Be(3);
    }

    [Fact]
    public void MoveForwardIgnoresMoveWhenOutOfBoundsButScentPresent()
    {
        // Arrange
        var worldService = CreateMockWorldService();
        worldService.IsInBound(1, 4).Returns(false);
        worldService.IsScented(1, 3).Returns(true);

        var navigationService = CreateNavigationService(worldService);
        var robot = CreateRobot(x: 1, y: 3, direction: Direction.N);

        // Act
        navigationService.MoveForward(robot);

        // Assert
        robot.CurrentXPosition.Should().Be(1);
        robot.CurrentYPosition.Should().Be(3);
        robot.Lost.Should().BeFalse();
    }

    [Fact]
    public void MoveForwardThrowsWhenRobotIsNull()
    {
        // Arrange
        var navigationService = CreateNavigationService();

        // Act
        Action act = () => navigationService.MoveForward(null!);

        // Assert
        act.Should().Throw<NullReferenceException>();
    }
}
