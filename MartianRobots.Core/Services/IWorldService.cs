using MartianRobots.Core.Models;

namespace MartianRobots.Core.Services;

public interface IWorldService
{
    public void SetBounds(int x,int y);
    public bool IsBoundSetUp();
    public void InsertRobot(MartianRobot robot);
    public bool IsInBound(int x,int y);
    public bool IsScented(int x,int y);
    public BoundWorld GetCurrentWorld();

}
