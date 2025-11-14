namespace MartianRobots.Core.Models;

public class BoundWorld
{
    public int X { get; set; } = -1;
    public int Y { get; set; }= -1;
    public List<MartianRobot> RobotsOnWorld { get; set; } = [];
}
