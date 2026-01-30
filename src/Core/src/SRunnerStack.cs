namespace SRunner.Core;

/// <summary>
/// Represents a stack of service that should be handled together.
/// </summary>
public record SRunnerStack
{
    /// <summary>
    /// Unique ID of the stack.
    /// </summary>
    public required string Id { get; init; }
    
    /// <summary>
    /// List of service IDs that are part of this stack.
    /// </summary>
    public required List<string> Services { get; init; }
}