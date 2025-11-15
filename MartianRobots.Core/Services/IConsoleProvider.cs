namespace MartianRobots.Core.Services;

public interface IConsoleProvider
{
    public string? ReadLine();
    public void WriteLine(string? message);
}
