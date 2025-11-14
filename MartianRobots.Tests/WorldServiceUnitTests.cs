using AwesomeAssertions;
using MartianRobots.Core.Services;
using Xunit;

namespace MartianRobots.Tests;

public class WorldServiceUnitTests
{
    [Fact]
    public void SetBoundsStoresBoundsInCurrentWorld()
    {
        // Arrange
        var worldService = new WorldService();
        const int maxX = 5;
        const int maxY = 3;

        // Act
        worldService.SetBounds(3, 5);
        var currentWorld = worldService.GetCurrentWorld();

        // Assert
        currentWorld.X.Should().Be(maxX);
        currentWorld.Y.Should().Be(maxY);
        currentWorld.RobotsOnWorld.Should().NotBeNull();
    }

    [Fact]
    public void IsInBoundUsesStoredBoundsCorrectly()
    {
        // Arrange
        var worldService = new WorldService();
        worldService.SetBounds(5, 3);

        // Act
        var insideOrigin   = worldService.IsInBound(0, 0);
        var insideMiddle   = worldService.IsInBound(2, 1);
        var onTopRightEdge = worldService.IsInBound(5, 3);
        var outsideRight   = worldService.IsInBound(6, 1);
        var outsideTop     = worldService.IsInBound(2, 4);

        // Assert
        insideOrigin.Should().BeTrue();
        insideMiddle.Should().BeTrue();
        onTopRightEdge.Should().BeTrue();
        outsideRight.Should().BeFalse();
        outsideTop.Should().BeFalse();
    }
    [Fact]
    public void IsBoundSetUpReturnsFalseBeforeBoundsAreSet()
    {
        // Arrange
        var worldService = new WorldService();

        // Act
        var isSetUp = worldService.IsBoundSetUp();

        // Assert
        isSetUp.Should().BeFalse();
    }

    [Fact]
    public void IsBoundSetUpReturnsTrueAfterBoundsAreSet()
    {
        // Arrange
        var worldService = new WorldService();

        // Act
        worldService.SetBounds(5, 3);
        var isSetUp = worldService.IsBoundSetUp();

        // Assert
        isSetUp.Should().BeTrue();
    }
    [Fact]
    public void IsInBoundReturnsFalseForNegativeCoordinates()
    {
        // Arrange
        var worldService = new WorldService();
        worldService.SetBounds(5, 3);

        // Act
        var negativeX = worldService.IsInBound(-1, 0);
        var negativeY = worldService.IsInBound(0, -1);

        // Assert
        negativeX.Should().BeFalse();
        negativeY.Should().BeFalse();
    }

    [Fact]
    public void SetBoundsThrowsWhenBoundsAreNegative()
    {
        // Arrange
        var worldService = new WorldService();

        // Act
        var setNegativeX = () => worldService.SetBounds(-1, 3);
        var setNegativeY = () => worldService.SetBounds(5, -1);

        // Assert
        setNegativeX.Should().Throw<ArgumentOutOfRangeException>();
        setNegativeY.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void IsInBoundThrowsIfCalledBeforeSetBounds()
    {
        // Arrange
        var worldService = new WorldService();

        // Act
        Action act = () => worldService.IsInBound(0, 0);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetCurrentWorldThrowsIfBoundsNotSet()
    {
        // Arrange
        var worldService = new WorldService();

        // Act
        Action act = () => worldService.GetCurrentWorld();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}
