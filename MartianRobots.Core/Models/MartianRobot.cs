namespace MartianRobots.Models;

public class MartianRobot
{
    public int CurrentXPosition { get; set; }
    public int CurrentYPosition { get; set; }
    public int LostAtX { get; set; }
    public int LostAtY { get; set; }
    public bool Lost { get; set; }

}

public enum Direction
{
    N = 0,
    E = 90,
    S = 180,
    W = 270
}
public class BoundWorld
{
    public int X { get; set; }
    public int Y { get; set; }
    public List<MartianRobot> RobotsOnWorld { get; set; } = [];
}
