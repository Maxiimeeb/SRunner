using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SRunner.Core.Serialization;

/// <inheritdoc cref="ISRunnerConfigurationLoader" />
/// <inheritdoc cref="ISRunnerConfigurationExporter" />
public class HomeDirectorySerializer(
    ILogger<HomeDirectorySerializer> logger
) : ISRunnerConfigurationLoader, ISRunnerConfigurationExporter
{
    private static readonly string RootPath = Path.Join([
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".srunner",
        ]
    );

    private static readonly string StackPath = Path.Join(RootPath, "stacks");
    private static readonly string ServicePath = Path.Join(RootPath, "services");

    private static readonly IDeserializer YamlDeserializer = new DeserializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    // Add serializer for export operations
    private static readonly ISerializer YamlSerializer = new SerializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .Build();

    /// <inheritdoc />
    public SRunnerConfiguration Load()
    {
        logger.LogInformation("Loading configuration from home directory at '{Path}'", RootPath);

        if (!Directory.Exists(RootPath))
        {
            logger.LogWarning(
                "Configuration directory does not exist at '{Path}'. Will default to empty configuration",
                RootPath
            );
            return new SRunnerConfiguration(
                new SRunnerServicesConfiguration()
                {
                    Services = []
                },
                new SRunnerStacksConfiguration()
                {
                    Stacks = []
                }
            );
        }

        var stacks = LoadStacks();
        var services = LoadServices();

        return new SRunnerConfiguration(services, stacks);
    }

    /// <inheritdoc />
    public Task<SRunnerConfiguration> LoadAsync()
    {
        // Placeholder implementation: indicate configuration not available in a documented way.
        return Task.FromException<SRunnerConfiguration>(
            new InvalidOperationException("Configuration source is not set or unavailable."));
    }

    /// <inheritdoc />
    public void Export(SRunnerConfiguration configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        logger.LogInformation("Exporting configuration to home directory at '{Path}'", RootPath);

        // Ensure root and subdirectories exist
        try
        {
            Directory.CreateDirectory(RootPath);
            Directory.CreateDirectory(StackPath);
            Directory.CreateDirectory(ServicePath);
        }
        catch (UnauthorizedAccessException)
        {
            // Let documented exception bubble
            throw;
        }
        catch (IOException)
        {
            // Let documented exception bubble
            throw;
        }
        catch (Exception ex)
        {
            // Map unexpected failures to InvalidOperationException as per contract
            throw new InvalidOperationException("Destination is not available or misconfigured.", ex);
        }

        // Write services
        var services = configuration.Services?.Services ?? [];
        foreach (var service in services)
        {
            var filePath = Path.Join(ServicePath, service.Id + ".yaml");
            var yaml = YamlSerializer.Serialize(service);
            File.WriteAllText(filePath, yaml);
        }

        // Write stacks
        var stacks = configuration.Stacks?.Stacks ?? [];
        foreach (var stack in stacks)
        {
            if (string.IsNullOrWhiteSpace(stack.Id))
            {
                logger.LogWarning("Encountered stack with missing Id. Skipping export of this entry.");
                continue;
            }

            var filePath = Path.Join(StackPath, stack.Id + ".yaml");
            var yaml = YamlSerializer.Serialize(stack);
            File.WriteAllText(filePath, yaml);
        }
    }

    public Task ExportAsync(SRunnerConfiguration configuration)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Loads stack configurations from YAML files in the stacks directory.
    /// If the directory does not exist or contains no valid files, returns an empty configuration.
    /// </summary>
    private SRunnerStacksConfiguration LoadStacks()
    {
        if (!Directory.Exists(StackPath))
        {
            logger.LogWarning(
                "Stacks directory does not exist at '{Path}'. Will default to empty stacks",
                StackPath
            );

            return SRunnerStacksConfiguration.Empty;
        }

        var files = Directory.EnumerateFiles(StackPath, "*.*", SearchOption.TopDirectoryOnly)
            .ToList();

        if (files.Count == 0)
        {
            logger.LogInformation("No stack YAML files found in '{Path}'. Using empty stacks.", StackPath);
            return SRunnerStacksConfiguration.Empty;
        }

        var stacks = new List<SRunnerStack>(files.Count);
        
        foreach (var file in files)
        {
            try
            {
                var yaml = File.ReadAllText(file);
                var stack = YamlDeserializer.Deserialize<SRunnerStack>(yaml);

                if (string.IsNullOrWhiteSpace(stack.Id))
                {
                    logger.LogWarning("Invalid or missing 'Id' in stack file '{File}'. Skipping.", file);
                    continue;
                }

                if (stack.Services is null)
                {
                    logger.LogWarning("Missing 'Services' in stack file '{File}'. Skipping.", file);
                    continue;
                }

                stacks.Add(stack);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to parse stack YAML file '{File}'. Skipping.", file);
            }
        }

        return stacks.Count == 0
            ? SRunnerStacksConfiguration.Empty
            : new SRunnerStacksConfiguration { Stacks = stacks };
    }

    /// <summary>
    /// Loads service configurations from YAML files in the services directory.
    /// If the directory does not exist or contains no valid files, returns an empty configuration.
    /// </summary>
    private SRunnerServicesConfiguration LoadServices()
    {
        if (!Directory.Exists(ServicePath))
        {
            logger.LogWarning(
                "Services directory does not exist at '{Path}'. Will default to empty services",
                ServicePath
            );

            return SRunnerServicesConfiguration.Empty;
        }

        var files = Directory.EnumerateFiles(ServicePath, "*.*", SearchOption.TopDirectoryOnly)
            .ToList();

        if (files.Count == 0)
        {
            logger.LogInformation("No service YAML files found in '{Path}'. Using empty services.", ServicePath);
            return SRunnerServicesConfiguration.Empty;
        }

        var services = new List<SRunnerService>(files.Count);
        foreach (var file in files)
        {
            try
            {
                var yaml = File.ReadAllText(file);
                var service = YamlDeserializer.Deserialize<SRunnerService>(yaml);

                if (string.IsNullOrWhiteSpace(service.Id))
                {
                    logger.LogWarning("Invalid or missing 'Id' in service file '{File}'. Skipping.", file);
                    continue;
                }

                if (string.IsNullOrWhiteSpace(service.StartCommand))
                {
                    logger.LogWarning("Missing 'StartCommand' in service file '{File}'. Skipping.", file);
                    continue;
                }

                services.Add(service);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to parse service YAML file '{File}'. Skipping.", file);
            }
        }

        return services.Count == 0
            ? SRunnerServicesConfiguration.Empty
            : new SRunnerServicesConfiguration { Services = services };
    }
}