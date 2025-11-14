using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using MartianRobots.Core.Models;

namespace MartianRobots.Core.Services;

public class ConsoleController(IWorldService worldService, IRobotService service, IConsoleProvider console)
{
    private MartianRobot? _currentRobot;
    public Task ExecuteAsync()
    {
        bool shouldStop = false;
        while (shouldStop)
        {
            var readLine = console.ReadLine();
            if (readLine.Equals("end", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Closing Simulation.");

                shouldStop = true;
                continue;
            }

            if (!worldService.IsBoundSetUp())
            {
                var coord = GetWorldCoordFromInput(readLine);

                worldService.SetBounds(coord[0], coord[1]);
                continue;
            }

            HandleRobotCommands(readLine);


        }

        return Task.CompletedTask;
    }

    private int[] GetWorldCoordFromInput(string readLine)
    {
        var coord = readLine.Split(" ").Select(x => int.Parse(x, CultureInfo.InvariantCulture)).ToArray();
        if (coord.Length > 2)
        {
            console.WriteLine("Unexpected Input, please insert Mars bound coordinates i.e. 1 5");
        }

        if (coord[0] > 50 || coord[1] > 50)
        {
            console.WriteLine("Incorrect Input, all coordinates have to be up to 50");
        }

        return coord;
    }
    private int[] GetRobotCoordFromInput(string readLine)
    {
        var coord = readLine.Split(",").Select(x => int.Parse(x, CultureInfo.InvariantCulture)).ToArray();
        if (coord.Length > 3)
        {
            console.WriteLine("Unexpected Input, please insert Mars bound coordinates with direction i.e. 1 5 E");
        }

        if (coord[0] > 50 || coord[1] > 50)
        {
            console.WriteLine("Incorrect Input, all coordinates have to be up to 50");
        }

        return coord;
    }

    private void HandleRobotCommands(string readLine)
    {
        if (_currentRobot == null)
        {
            var coord = GetRobotCoordFromInput(readLine);
            if (!worldService.IsInBound(coord[0], coord[1]))
            {
                console.WriteLine("Robot placed outside of boundary");
                return;
            }

            if (!Enum.TryParse(typeof(Direction), coord[2].ToString(CultureInfo.InvariantCulture), out var direction))
            {
                console.WriteLine("Incorrect direction, currently supported directions :  N E S W");
            }
            var robot = service.CreateRobot(coord[0], coord[1], direction);
            _currentRobot = robot;
            worldService.InsertRobot(robot);
            return;
        }

        if (readLine.Length > 100)
        {
            console.WriteLine("Inccorect input, robots support up to 100 commands");
        }
        service.ParseCommands(readLine);
            console.WriteLine($"{_currentRobot.CurrentXPosition} {_currentRobot.CurrentYPosition} {_currentRobot.Direction.ToString()}{(_currentRobot.Lost ? " LOST" : "")}");

    }
}
