using MartianRobots.Core.Models;

namespace MartianRobots.Core.Services;

public interface IRobotService
{
    public MartianRobot CreateRobot(int x, int y, Direction direction);
    public void ParseCommands(MartianRobot robot, string commandString);
}

public class RobotService(IWorldService worldService, IRobotNavigationService robotNavigationService) : IRobotService
{
    public MartianRobot CreateRobot(int x, int y, Direction direction)
    {
        if (!worldService.IsInBound(x, y))
        {
            throw new InvalidOperationException($"Robot position ({x}, {y}) is out of bounds.");
        }

        return new MartianRobot
        {
            CurrentXPosition = x,
            CurrentYPosition = y,
            Direction = direction,
            Lost = false,
            LostAtX = -1,
            LostAtY = -1
        };
    }

    public void ParseCommands(MartianRobot robot, string commandString)
    {
        if (robot is null)
        {
            throw new InvalidOperationException("No current robot set.");
        }

        if (robot.Lost)
        {
            return;
        }

        foreach (var command in commandString)
        {
            switch (char.ToUpperInvariant(command))
            {
                case 'L':
                    robotNavigationService.RotateLeft(robot);
                    break;

                case 'R':
                    robotNavigationService.RotateRight(robot);
                    break;

                case 'F':
                    robotNavigationService.MoveForward(robot);
                    break;
            }

            if (robot.Lost)
            {
                break;
            }
        }
    }
}
