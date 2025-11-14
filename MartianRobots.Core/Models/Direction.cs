using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace MartianRobots.Core.Models;
//editor config bug: https://github.com/dotnet/roslyn/issues/24209 (never addressed)
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum Direction
{
    N = 0,
    E = 90,
    S = 180,
    W = 270
}
