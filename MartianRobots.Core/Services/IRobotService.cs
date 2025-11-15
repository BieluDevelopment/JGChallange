using MartianRobots.Core.Models;

namespace MartianRobots.Core.Services;

public interface IRobotService
{
    public MartianRobot CreateRobot(int x, int y, Direction direction);
    public Task ParseCommands(MartianRobot robot, string commandString);
}
