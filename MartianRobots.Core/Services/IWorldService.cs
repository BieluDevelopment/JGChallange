using MartianRobots.Models;

namespace MartianRobots.Core.Services;

public interface IWorldService
{
    public void SetBounds();
    public void InsertRobot();
    public bool IsInBound(int x,int y);
    public bool IsScented(int x,int y);
    public BoundWorld GetCurrentWorld();

}

public class WorldService : IWorldService
{
    public void SetBounds() => throw new NotImplementedException();
    public void InsertRobot() => throw new NotImplementedException();

    public bool IsInBound(int x, int y) => throw new NotImplementedException();

    public bool IsScented(int x, int y) => throw new NotImplementedException();

    public bool IsInBound() => throw new NotImplementedException();
    public BoundWorld GetCurrentWorld() => throw new NotImplementedException();
}
