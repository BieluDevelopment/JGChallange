using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using MartianRobots.Core.Models;
using Microsoft.Extensions.Hosting;

namespace MartianRobots.Core.Services;

public class ConsoleController(IWorldService worldService, IRobotService service, IConsoleProvider console)
    : IHostedService
{
    private MartianRobot? _currentRobot;

    public Task ExecuteAsync()
    {
        var shouldStop = false;
        while (!shouldStop)
        {
            var readLine = console.ReadLine();
            if (string.IsNullOrWhiteSpace(readLine))
            {
                continue;
            }

            if (readLine.Equals("end", StringComparison.OrdinalIgnoreCase))
            {
                console.WriteLine("Closing Simulation.");

                shouldStop = true;
                continue;
            }

            if (!worldService.IsBoundSetUp())
            {
                var coord = GetWorldCoordFromInput(readLine);
                if (coord.Length != 0)
                {
                    worldService.SetBounds(coord[0], coord[1]);

                }
                continue;
            }

            HandleRobotCommands(readLine);
        }

        return Task.CompletedTask;
    }

    private int[] GetWorldCoordFromInput(string readLine)
    {
        try
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
        catch (Exception e)
        {
            console.WriteLine("Unexpected Input, please insert Mars bound coordinates i.e. 1 5");
        }

        return [];
    }

    private int[] GetRobotCoordFromInput(IEnumerable<string> readLine)
    {
        var coord = readLine.Select(x => int.Parse(x, CultureInfo.InvariantCulture)).ToArray();
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
            var input = readLine.Split(" ");
            if (input.Length > 3)
            {
                console.WriteLine("Unexpected Input, please insert Mars bound coordinates with direction i.e. 1 5 E");
            }
            var coord = GetRobotCoordFromInput(input.Take(2));
            if (!worldService.IsInBound(coord[0], coord[1]))
            {
                console.WriteLine("Robot placed outside of boundary");
                return;
            }

            if (!Enum.TryParse<Direction>(input[2].ToString(CultureInfo.InvariantCulture), out var direction))
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
            console.WriteLine("Incorrect input, robots support up to 100 commands");
        }

        service.ParseCommands(_currentRobot, readLine);
        console.WriteLine(
            $"{_currentRobot.CurrentXPosition} {_currentRobot.CurrentYPosition} {_currentRobot.Direction.ToString()}{(_currentRobot.Lost ? " LOST" : "")}");
    }

    public async Task StartAsync(CancellationToken cancellationToken) => await ExecuteAsync();

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
