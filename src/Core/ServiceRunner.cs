namespace Core;

/// <summary>
/// Represents a service configuration
/// </summary>
public class ServiceConfig
{
    private string _name = string.Empty;
    private string _command = string.Empty;

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Service name cannot be empty", nameof(value));
            _name = value;
        }
    }

    public string Command
    {
        get => _command;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Service command cannot be empty", nameof(value));
            _command = value;
        }
    }

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
        ArgumentNullException.ThrowIfNull(service);

        if (_services.Any(s => s.Name.Equals(service.Name, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"A service with the name '{service.Name}' already exists.");
        }

        _services.Add(service);
    }

    public void RemoveService(string name)
    {
        _services.RemoveAll(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public ServiceConfig? GetService(string name)
    {
        return _services.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}
