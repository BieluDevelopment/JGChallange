using MartianRobots.Core.Models;

namespace MartianRobots.Core.Services;

public interface IRobotNavigationService
{
    public Task MoveForward(MartianRobot robot);
    public Task RotateLeft(MartianRobot robot);
    public Task RotateRight(MartianRobot robot);
}

public class RobotNavigationService(IWorldService worldService): IRobotNavigationService
{

    public Task RotateLeft(MartianRobot robot)
    {
        if (robot == null)
        {
            throw new InvalidOperationException("no robot");
        }
        robot.Direction = (Direction)(((int)robot.Direction - 90 + 360) % 360);
        return Task.CompletedTask;
    }

    public Task RotateRight(MartianRobot robot)
    {
        if (robot == null)
        {
            throw new InvalidOperationException("no robot");
        }
        robot.Direction = (Direction)(((int)robot.Direction + 90) % 360);
        return Task.CompletedTask;
    }

    public Task MoveForward(MartianRobot robot)
    {
        var (newX, newY) = robot.Direction switch
        {
            Direction.N => (robot.CurrentXPosition, robot.CurrentYPosition + 1),
            Direction.E => (robot.CurrentXPosition + 1, robot.CurrentYPosition),
            Direction.S => (robot.CurrentXPosition, robot.CurrentYPosition - 1),
            Direction.W => (robot.CurrentXPosition - 1, robot.CurrentYPosition),
            _ => throw new InvalidOperationException($"Invalid direction: {robot.Direction}")
        };

        // Check if new position is out of bounds.
        if (!worldService.IsInBound(newX, newY))
        {
            // Check if there's a scent at the current position.
            if (worldService.IsScented(robot.CurrentXPosition, robot.CurrentYPosition))
            {
                // Scent present: ignore the move.
                return Task.CompletedTask;
            }

            robot.Lost = true;
            robot.LostAtX = robot.CurrentXPosition;
            robot.LostAtY = robot.CurrentYPosition;
            return Task.CompletedTask;
        }

        robot.CurrentXPosition = newX;
        robot.CurrentYPosition = newY;
        return Task.CompletedTask;
    }
}
