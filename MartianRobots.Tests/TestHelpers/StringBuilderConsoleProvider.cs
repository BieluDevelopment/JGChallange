using MartianRobots.Core.Services;

namespace MartianRobots.Tests.TestHelpers;

/// <summary>
/// Console provider that captures output to a StringBuilder.
/// </summary>
public class StringBuilderConsoleProvider : IConsoleProvider
{
    private readonly System.Text.StringBuilder _output = new();

    public string? ReadLine() => null;

    public void WriteLine(string? message) => _output.AppendLine(message);

    public string GetOutput() => _output.ToString();
}
