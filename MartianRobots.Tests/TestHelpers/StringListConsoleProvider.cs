using MartianRobots.Core.Services;

namespace MartianRobots.Tests.TestHelpers;

/// <summary>
/// Console provider that reads from a list of strings.
/// </summary>
public class StringListConsoleProvider(string[] lines) : IConsoleProvider
{
    private readonly IEnumerator<string> _lines = ((IEnumerable<string>)lines).GetEnumerator();

    public string? ReadLine() => _lines.MoveNext() ? _lines.Current : null;

    public void WriteLine(string? message)
    {
        // Ignored in this provider (use CompositeConsoleProvider to capture output).
    }
}
