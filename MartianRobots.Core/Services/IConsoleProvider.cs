namespace MartianRobots.Core.Services;

public interface IConsoleProvider
{
    public string? ReadLine();
    public void WriteLine(string? message);
}

public class ConsoleProvider : IConsoleProvider
{
    public string? ReadLine() => Console.ReadLine();

    public void WriteLine(string? message) => Console.WriteLine(message);
}
