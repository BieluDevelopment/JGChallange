namespace MartianRobots.Core.Services;

public interface IRobotNavigationService
{
    public bool MoveForward();
    public void RotateLeft();
    public void RotateRight();
}
