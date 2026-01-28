namespace Core;

/// <summary>
/// Represents a service configuration
/// </summary>
public class ServiceConfig
{
    public string Name { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public string WorkingDirectory { get; set; } = string.Empty;
    public bool AutoStart { get; set; }
}

/// <summary>
/// Manages service execution
/// </summary>
public class ServiceRunner
{
    private readonly List<ServiceConfig> _services = new();

    public IReadOnlyList<ServiceConfig> Services => _services.AsReadOnly();

    public void AddService(ServiceConfig service)
    {
        _services.Add(service);
    }

    public void RemoveService(string name)
    {
        _services.RemoveAll(s => s.Name == name);
    }

    public ServiceConfig? GetService(string name)
    {
        return _services.FirstOrDefault(s => s.Name == name);
    }
}
