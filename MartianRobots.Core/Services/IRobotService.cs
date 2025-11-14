using MartianRobots.Core.Models;

namespace MartianRobots.Core.Services;

public interface IRobotService
{
    public MartianRobot CreateRobot(int x, int y, object? direction);
    public void ParseCommands(string commandString);

}
