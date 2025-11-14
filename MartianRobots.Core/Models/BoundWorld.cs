namespace MartianRobots.Core.Models;

public class BoundWorld
{
    public int X { get; set; }
    public int Y { get; set; }
    public List<MartianRobot> RobotsOnWorld { get; set; } = [];
}
