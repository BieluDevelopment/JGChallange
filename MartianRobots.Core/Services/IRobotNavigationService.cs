using MartianRobots.Core.Models;

namespace MartianRobots.Core.Services;

public interface IRobotNavigationService
{
    public Task MoveForward(MartianRobot robot);
    public Task RotateLeft(MartianRobot robot);
    public Task RotateRight(MartianRobot robot);
}
