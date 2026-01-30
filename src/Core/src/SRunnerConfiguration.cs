namespace SRunner.Core;

public record SRunnerConfiguration(
    SRunnerServicesConfiguration Services,
    SRunnerStacksConfiguration Stacks
);

public record SRunnerServicesConfiguration()
{
    public static readonly SRunnerServicesConfiguration Empty = new()
    {
        Services = []
    };

    public required IReadOnlyList<SRunnerService> Services { get; init; }
}

public record SRunnerStacksConfiguration()
{
    public static readonly SRunnerStacksConfiguration Empty = new()
    {
        Stacks = []
    };

    public required IReadOnlyList<SRunnerStack> Stacks { get; init; }
}