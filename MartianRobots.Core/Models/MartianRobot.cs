namespace MartianRobots.Core.Models;

public class MartianRobot
{
    public int CurrentXPosition { get; set; }
    public int CurrentYPosition { get; set; }
    public int LostAtX { get; set; }
    public int LostAtY { get; set; }
    public bool Lost { get; set; }
    public Direction Direction { get; set; }

}
