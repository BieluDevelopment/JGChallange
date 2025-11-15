namespace MartianRobots.Core.Services;

public class ConsoleProvider : IConsoleProvider
{
    public string? ReadLine() => Console.ReadLine();

    public void WriteLine(string? message) => Console.WriteLine(message);
}