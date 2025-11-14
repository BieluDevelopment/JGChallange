using MartianRobots.Models;

namespace MartianRobots.Core.Services;

public interface IRobotService
{
    public MartianRobot CreateRobot(int x, int y);
    
}