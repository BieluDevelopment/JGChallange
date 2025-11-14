using MartianRobots.Core.Models;

namespace MartianRobots.Core.Services;

public interface IRobotNavigationService
{
    public void MoveForward(MartianRobot robot);
    public void RotateLeft(MartianRobot robot);
    public void RotateRight(MartianRobot robot);
}

public class RobotNavigationService(IWorldService worldService): IRobotNavigationService
{

    public void RotateLeft(MartianRobot robot)
    {
        if (robot == null)
        {
            throw new InvalidOperationException("no robot");
        }
        robot.Direction = (Direction)(((int)robot.Direction - 90 + 360) % 360);
    }

    public void RotateRight(MartianRobot robot)
    {
        if (robot == null)
        {
            throw new InvalidOperationException("no robot");
        }
        robot.Direction = (Direction)(((int)robot.Direction + 90) % 360);
    }

    public void MoveForward(MartianRobot robot)
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
                return;
            }

            robot.Lost = true;
            robot.LostAtX = robot.CurrentXPosition;
            robot.LostAtY = robot.CurrentYPosition;
            return;
        }

        robot.CurrentXPosition = newX;
        robot.CurrentYPosition = newY;
    }
}
