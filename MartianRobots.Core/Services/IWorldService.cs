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

public class WorldService : IWorldService
{
    public void SetBounds(int x,int y) => throw new NotImplementedException();
    public bool IsBoundSetUp() => throw new NotImplementedException();

    public void InsertRobot(MartianRobot robot) => throw new NotImplementedException();

    public bool IsInBound(int x, int y) => throw new NotImplementedException();

    public bool IsScented(int x, int y) => throw new NotImplementedException();

    public bool IsInBound() => throw new NotImplementedException();
    public BoundWorld GetCurrentWorld() => throw new NotImplementedException();
}
