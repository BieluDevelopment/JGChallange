using MartianRobots.Core.Services;

namespace MartianRobots.Tests.TestHelpers;

/// <summary>
/// Composite provider that reads from one and writes to another.
/// </summary>
public class CompositeConsoleProvider(IConsoleProvider reader, IConsoleProvider writer) : IConsoleProvider
{
    public string? ReadLine() => reader.ReadLine();

    public void WriteLine(string? message) => writer.WriteLine(message);
}
