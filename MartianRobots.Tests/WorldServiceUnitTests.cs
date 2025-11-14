using AwesomeAssertions;
using MartianRobots.Core.Models;
using MartianRobots.Core.Services;
using Xunit;

namespace MartianRobots.Tests;

public class WorldServiceUnitTests
{
    private static WorldService CreateWorldService()
    {
        return new WorldService();
    }

    [Fact]
    public void SetBoundsStoresBoundsInCurrentWorld()
    {
        // Arrange
        var worldService = CreateWorldService();
        const int maxX = 5;
        const int maxY = 3;

        // Act
        worldService.SetBounds(maxX, maxY);
        var currentWorld = worldService.GetCurrentWorld();

        // Assert
        currentWorld.X.Should().Be(maxX);
        currentWorld.Y.Should().Be(maxY);
        currentWorld.RobotsOnWorld.Should().NotBeNull();
    }

    [Fact]
    public void IsBoundSetUpReturnsFalseInitially()
    {
        // Arrange
        var worldService = CreateWorldService();

        // Act
        var isSetUp = worldService.IsBoundSetUp();

        // Assert
        isSetUp.Should().BeFalse();
    }

    [Fact]
    public void IsBoundSetUpReturnsTrueAfterBoundsAreSet()
    {
        // Arrange
        var worldService = CreateWorldService();

        // Act
        worldService.SetBounds(5, 3);
        var isSetUp = worldService.IsBoundSetUp();

        // Assert
        isSetUp.Should().BeTrue();
    }

    [Fact]
    public void IsInBoundReturnsTrueForPositionInsideBounds()
    {
        // Arrange
        var worldService = CreateWorldService();
        worldService.SetBounds(5, 3);

        // Act
        var insideMiddle = worldService.IsInBound(2, 1);

        // Assert
        insideMiddle.Should().BeTrue();
    }

    [Fact]
    public void IsInBoundReturnsTrueForPositionOnRightEdge()
    {
        // Arrange
        var worldService = CreateWorldService();
        worldService.SetBounds(5, 3);

        // Act
        var onRightEdge = worldService.IsInBound(5, 1);

        // Assert
        onRightEdge.Should().BeTrue();
    }

    [Fact]
    public void IsInBoundReturnsTrueForPositionOnTopEdge()
    {
        // Arrange
        var worldService = CreateWorldService();
        worldService.SetBounds(5, 3);

        // Act
        var onTopEdge = worldService.IsInBound(2, 3);

        // Assert
        onTopEdge.Should().BeTrue();
    }

    [Fact]
    public void IsInBoundReturnsFalseForOrigin()
    {
        // Arrange
        var worldService = CreateWorldService();
        worldService.SetBounds(5, 3);

        // Act
        var atOrigin = worldService.IsInBound(-1, 0);

        // Assert
        atOrigin.Should().BeFalse();
    }

    [Fact]
    public void IsInBoundReturnsFalseForNegativeX()
    {
        // Arrange
        var worldService = CreateWorldService();
        worldService.SetBounds(5, 3);

        // Act
        var negativeX = worldService.IsInBound(-1, 1);

        // Assert
        negativeX.Should().BeFalse();
    }

    [Fact]
    public void IsInBoundReturnsFalseForNegativeY()
    {
        // Arrange
        var worldService = CreateWorldService();
        worldService.SetBounds(5, 3);

        // Act
        var negativeY = worldService.IsInBound(1, -1);

        // Assert
        negativeY.Should().BeFalse();
    }

    [Fact]
    public void IsInBoundReturnsFalseForXExceedingBound()
    {
        // Arrange
        var worldService = CreateWorldService();
        worldService.SetBounds(5, 3);

        // Act
        var exceedsX = worldService.IsInBound(6, 1);

        // Assert
        exceedsX.Should().BeFalse();
    }

    [Fact]
    public void IsInBoundReturnsFalseForYExceedingBound()
    {
        // Arrange
        var worldService = CreateWorldService();
        worldService.SetBounds(5, 3);

        // Act
        var exceedsY = worldService.IsInBound(1, 4);

        // Assert
        exceedsY.Should().BeFalse();
    }

    [Fact]
    public void InsertRobotAddsRobotToWorld()
    {
        // Arrange
        var worldService = CreateWorldService();
        worldService.SetBounds(5, 3);

        var robot = new MartianRobot
        {
            CurrentXPosition = 1,
            CurrentYPosition = 1,
            Direction = Direction.N,
            Lost = false
        };

        // Act
        worldService.InsertRobot(robot);
        var currentWorld = worldService.GetCurrentWorld();

        // Assert
        currentWorld.RobotsOnWorld.Should().HaveCount(1);
        currentWorld.RobotsOnWorld.First().Should().Be(robot);
    }

    [Fact]
    public void InsertRobotAddsMultipleRobotsToWorld()
    {
        // Arrange
        var worldService = CreateWorldService();
        worldService.SetBounds(5, 3);

        var robot1 = new MartianRobot
        {
            CurrentXPosition = 1,
            CurrentYPosition = 1,
            Direction = Direction.N,
            Lost = false
        };

        var robot2 = new MartianRobot
        {
            CurrentXPosition = 3,
            CurrentYPosition = 2,
            Direction = Direction.E,
            Lost = false
        };

        // Act
        worldService.InsertRobot(robot1);
        worldService.InsertRobot(robot2);
        var currentWorld = worldService.GetCurrentWorld();

        // Assert
        currentWorld.RobotsOnWorld.Should().HaveCount(2);
        currentWorld.RobotsOnWorld.Should().Contain(robot1);
        currentWorld.RobotsOnWorld.Should().Contain(robot2);
    }

    [Fact]
    public void GetCurrentWorldReturnsBoundWorld()
    {
        // Arrange
        var worldService = CreateWorldService();
        worldService.SetBounds(5, 3);

        // Act
        var currentWorld = worldService.GetCurrentWorld();

        // Assert
        currentWorld.Should().NotBeNull();
        currentWorld.X.Should().Be(5);
        currentWorld.Y.Should().Be(3);
    }

    [Fact]
    public void IsScentedReturnsFalseWhenNoRobotsLost()
    {
        // Arrange
        var worldService = CreateWorldService();
        worldService.SetBounds(5, 3);

        // Act
        var hasScent = worldService.IsScented(1, 1);

        // Assert
        hasScent.Should().BeFalse();
    }

    [Fact]
    public void IsScentedReturnsTrueWhenRobotLostAtPosition()
    {
        // Arrange
        var worldService = CreateWorldService();
        worldService.SetBounds(5, 3);

        var lostRobot = new MartianRobot
        {
            CurrentXPosition = 3,
            CurrentYPosition = 3,
            Direction = Direction.N,
            Lost = true,
            LostAtX = 3,
            LostAtY = 3
        };

        worldService.InsertRobot(lostRobot);

        // Act
        var hasScent = worldService.IsScented(3, 3);

        // Assert
        hasScent.Should().BeTrue();
    }

    [Fact]
    public void IsScentedReturnsFalseWhenRobotLostAtDifferentPosition()
    {
        // Arrange
        var worldService = CreateWorldService();
        worldService.SetBounds(5, 3);

        var lostRobot = new MartianRobot
        {
            CurrentXPosition = 3,
            CurrentYPosition = 3,
            Direction = Direction.N,
            Lost = true,
            LostAtX = 3,
            LostAtY = 3
        };

        worldService.InsertRobot(lostRobot);

        // Act
        var hasScent = worldService.IsScented(2, 2);

        // Assert
        hasScent.Should().BeFalse();
    }

    [Fact]
    public void IsScentedReturnsTrueWhenMultipleRobotsLost()
    {
        // Arrange
        var worldService = CreateWorldService();
        worldService.SetBounds(5, 3);

        var robot1 = new MartianRobot
        {
            CurrentXPosition = 3,
            CurrentYPosition = 3,
            Direction = Direction.N,
            Lost = true,
            LostAtX = 3,
            LostAtY = 3
        };

        var robot2 = new MartianRobot
        {
            CurrentXPosition = 2,
            CurrentYPosition = 2,
            Direction = Direction.E,
            Lost = true,
            LostAtX = 2,
            LostAtY = 2
        };

        worldService.InsertRobot(robot1);
        worldService.InsertRobot(robot2);

        // Act
        var hasScent1 = worldService.IsScented(3, 3);
        var hasScent2 = worldService.IsScented(2, 2);

        // Assert
        hasScent1.Should().BeTrue();
        hasScent2.Should().BeTrue();
    }
}
