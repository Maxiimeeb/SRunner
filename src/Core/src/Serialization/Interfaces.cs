namespace SRunner.Core.Serialization;

/// <summary>
/// Provides methods to load an <see cref="SRunnerConfiguration"/> from a configured source.
/// </summary>
public interface ISRunnerConfigurationLoader
{
    /// <summary>
    /// Loads the runner configuration synchronously from the underlying source.
    /// </summary>
    /// <returns>The loaded <see cref="SRunnerConfiguration"/> instance.</returns>
    /// <remarks>
    /// Implementations should avoid long-blocking operations; use <see cref="LoadAsync"/> for IO-bound work.
    /// </remarks>
    /// <exception cref="InvalidOperationException">The loader is misconfigured or configuration is not available.</exception>
    /// <exception cref="UnauthorizedAccessException">The current process does not have sufficient permissions to access the source.</exception>
    /// <exception cref="System.IO.IOException">A general IO error occurred while reading configuration.</exception>
    SRunnerConfiguration Load();

    /// <summary>
    /// Loads the runner configuration asynchronously from the underlying source.
    /// </summary>
    /// <returns>A task that completes with the loaded <see cref="SRunnerConfiguration"/> instance.</returns>
    /// <exception cref="InvalidOperationException">The loader is misconfigured or configuration is not available.</exception>
    /// <exception cref="UnauthorizedAccessException">The current process does not have sufficient permissions to access the source.</exception>
    /// <exception cref="System.IO.IOException">A general IO error occurred while reading configuration.</exception>
    Task<SRunnerConfiguration> LoadAsync();
}

/// <summary>
/// Provides methods to export an <see cref="SRunnerConfiguration"/> to a configured destination.
/// </summary>
public interface ISRunnerConfigurationExporter
{
    /// <summary>
    /// Exports the provided configuration synchronously to the underlying destination.
    /// </summary>
    /// <param name="configuration">The <see cref="SRunnerConfiguration"/> to export. Must not be null.</param>
    /// <remarks>
    /// Implementations should validate the configuration before writing.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="configuration"/> is null.</exception>
    /// <exception cref="InvalidOperationException">The exporter is misconfigured or destination is not available.</exception>
    /// <exception cref="UnauthorizedAccessException">The current process does not have sufficient permissions to write to the destination.</exception>
    /// <exception cref="System.IO.IOException">A general IO error occurred while writing configuration.</exception>
    void Export(SRunnerConfiguration configuration);

    /// <summary>
    /// Exports the provided configuration asynchronously to the underlying destination.
    /// </summary>
    /// <param name="configuration">The <see cref="SRunnerConfiguration"/> to export. Must not be null.</param>
    /// <exception cref="ArgumentNullException"><paramref name="configuration"/> is null.</exception>
    /// <exception cref="InvalidOperationException">The exporter is misconfigured or destination is not available.</exception>
    /// <exception cref="UnauthorizedAccessException">The current process does not have sufficient permissions to write to the destination.</exception>
    /// <exception cref="System.IO.IOException">A general IO error occurred while writing configuration.</exception>
    Task ExportAsync(SRunnerConfiguration configuration);
}
