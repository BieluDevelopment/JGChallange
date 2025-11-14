using MartianRobots.Core.Models;

namespace MartianRobots.Core.Services;

public interface IRobotService
{
    public MartianRobot CreateRobot(int x, int y, Direction direction);
    public Task ParseCommands(MartianRobot robot, string commandString);
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

    public async Task ParseCommands(MartianRobot robot, string commandString)
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
                 await   robotNavigationService.RotateLeft(robot);
                    break;

                case 'R':
                    await   robotNavigationService.RotateRight(robot);
                    break;

                case 'F':
                    await     robotNavigationService.MoveForward(robot);
                    break;
            }

            if (robot.Lost)
            {
                break;
            }
        }

        robot.Processed = true;
    }
}
