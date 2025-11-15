using MartianRobots.Core.Models;

namespace MartianRobots.Core.Services;

public class WorldService : IWorldService
{
    private readonly BoundWorld _currentWorld = new();

    public void SetBounds(int x, int y)
    {
        _currentWorld.X = x;
        _currentWorld.Y = y;
    }
    public bool IsBoundSetUp() => _currentWorld is { X: > -1, Y: > -1 };

    public void InsertRobot(MartianRobot robot) => _currentWorld.RobotsOnWorld.Add(robot);

    public bool IsInBound(int x, int y) => (x >= 0 && x <= _currentWorld.X) && (y >= 0 && y <= _currentWorld.Y);

    public bool IsScented(int x, int y) =>
        _currentWorld.RobotsOnWorld.Any(robot => robot.LostAtX == x && robot.LostAtY == y);


    public BoundWorld GetCurrentWorld() =>  _currentWorld;
}