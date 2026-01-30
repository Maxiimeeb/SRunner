namespace SRunner.Core;

/// <summary>
/// Represents a service that can be managed by SRunner.
/// </summary>
public class SRunnerService
{
    /// <summary>
    /// Unique ID of the service.
    /// </summary>
    public required string Id { get; init; }
    
    /// <summary>
    /// Shell command to start the service.
    /// </summary>
    public required string StartCommand { get; init; }
}