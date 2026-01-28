using Terminal.Gui;
using Core;

namespace Cli;

/// <summary>
/// Interactive Terminal.GUI interface for SRunner
/// </summary>
public class InteractiveUI
{
    private readonly ServiceRunner _serviceRunner;
    private ListView? _servicesListView;
    private List<string> _serviceNames = new();

    public InteractiveUI()
    {
        _serviceRunner = new ServiceRunner();
        InitializeSampleServices();
    }

    private void InitializeSampleServices()
    {
        // Add some sample services for demonstration
        _serviceRunner.AddService(new ServiceConfig
        {
            Name = "Web Server",
            Command = "dotnet run",
            WorkingDirectory = "/app/web",
            AutoStart = true
        });

        _serviceRunner.AddService(new ServiceConfig
        {
            Name = "API Service",
            Command = "npm start",
            WorkingDirectory = "/app/api",
            AutoStart = false
        });

        _serviceRunner.AddService(new ServiceConfig
        {
            Name = "Database",
            Command = "docker-compose up",
            WorkingDirectory = "/app/database",
            AutoStart = true
        });

        UpdateServiceList();
    }

    private void UpdateServiceList()
    {
        _serviceNames = _serviceRunner.Services.Select(s => s.Name).ToList();
    }

    public void Run()
    {
        Application.Init();

        try
        {
            var top = Application.Top;

            // Create main window
            var win = new Window("SRunner - Service Manager")
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            // Create menu bar
            var menu = new MenuBar(new MenuBarItem[]
            {
                new MenuBarItem("_File", new MenuItem[]
                {
                    new MenuItem("_Quit", "Exit SRunner", () => Application.RequestStop())
                }),
                new MenuBarItem("_Help", new MenuItem[]
                {
                    new MenuItem("_About", "About SRunner", () => ShowAbout())
                })
            });

            top.Add(menu);

            // Create label
            var label = new Label("Configured Services:")
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill() - 2,
                Height = 1
            };
            win.Add(label);

            // Create services list
            _servicesListView = new ListView(_serviceNames)
            {
                X = 1,
                Y = 2,
                Width = Dim.Fill() - 2,
                Height = Dim.Fill() - 6
            };
            win.Add(_servicesListView);

            // Create buttons
            var addButton = new Button("_Add Service")
            {
                X = 1,
                Y = Pos.Bottom(_servicesListView) + 1
            };
            addButton.Clicked += OnAddService;
            win.Add(addButton);

            var removeButton = new Button("_Remove Service")
            {
                X = Pos.Right(addButton) + 2,
                Y = Pos.Bottom(_servicesListView) + 1
            };
            removeButton.Clicked += OnRemoveService;
            win.Add(removeButton);

            var detailsButton = new Button("_Details")
            {
                X = Pos.Right(removeButton) + 2,
                Y = Pos.Bottom(_servicesListView) + 1
            };
            detailsButton.Clicked += OnShowDetails;
            win.Add(detailsButton);

            var quitButton = new Button("_Quit")
            {
                X = Pos.Right(detailsButton) + 2,
                Y = Pos.Bottom(_servicesListView) + 1
            };
            quitButton.Clicked += () => Application.RequestStop();
            win.Add(quitButton);

            top.Add(win);

            Application.Run();
        }
        finally
        {
            Application.Shutdown();
        }
    }

    private void OnAddService()
    {
        var dialog = new Dialog("Add Service", 60, 15);

        var nameLabel = new Label("Name:")
        {
            X = 1,
            Y = 1
        };
        dialog.Add(nameLabel);

        var nameField = new TextField("")
        {
            X = Pos.Right(nameLabel) + 1,
            Y = 1,
            Width = Dim.Fill() - 2
        };
        dialog.Add(nameField);

        var commandLabel = new Label("Command:")
        {
            X = 1,
            Y = 3
        };
        dialog.Add(commandLabel);

        var commandField = new TextField("")
        {
            X = 1,
            Y = 4,
            Width = Dim.Fill() - 2
        };
        dialog.Add(commandField);

        var workDirLabel = new Label("Working Directory:")
        {
            X = 1,
            Y = 6
        };
        dialog.Add(workDirLabel);

        var workDirField = new TextField("")
        {
            X = 1,
            Y = 7,
            Width = Dim.Fill() - 2
        };
        dialog.Add(workDirField);

        var okButton = new Button("OK")
        {
            X = Pos.Center() - 10,
            Y = Pos.Bottom(dialog) - 4,
            IsDefault = true
        };
        okButton.Clicked += () =>
        {
            var name = nameField.Text?.ToString() ?? "";
            var command = commandField.Text?.ToString() ?? "";
            var workDir = workDirField.Text?.ToString() ?? "";

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(command))
            {
                _serviceRunner.AddService(new ServiceConfig
                {
                    Name = name,
                    Command = command,
                    WorkingDirectory = workDir,
                    AutoStart = false
                });
                UpdateServiceList();
                if (_servicesListView != null)
                {
                    _servicesListView.SetSource(_serviceNames);
                }
                Application.RequestStop();
            }
            else
            {
                MessageBox.ErrorQuery("Error", "Name and Command are required!", "OK");
            }
        };
        dialog.AddButton(okButton);

        var cancelButton = new Button("Cancel");
        cancelButton.Clicked += () => Application.RequestStop();
        dialog.AddButton(cancelButton);

        Application.Run(dialog);
    }

    private void OnRemoveService()
    {
        if (_servicesListView?.SelectedItem >= 0 && _servicesListView.SelectedItem < _serviceNames.Count)
        {
            var serviceName = _serviceNames[_servicesListView.SelectedItem];
            var result = MessageBox.Query("Confirm", $"Remove service '{serviceName}'?", "Yes", "No");

            if (result == 0)
            {
                _serviceRunner.RemoveService(serviceName);
                UpdateServiceList();
                _servicesListView.SetSource(_serviceNames);
            }
        }
        else
        {
            MessageBox.ErrorQuery("Error", "Please select a service to remove", "OK");
        }
    }

    private void OnShowDetails()
    {
        if (_servicesListView?.SelectedItem >= 0 && _servicesListView.SelectedItem < _serviceNames.Count)
        {
            var serviceName = _serviceNames[_servicesListView.SelectedItem];
            var service = _serviceRunner.GetService(serviceName);

            if (service != null)
            {
                var details = $"Name: {service.Name}\n" +
                            $"Command: {service.Command}\n" +
                            $"Working Directory: {service.WorkingDirectory}\n" +
                            $"Auto Start: {service.AutoStart}";

                MessageBox.Query("Service Details", details, "OK");
            }
        }
        else
        {
            MessageBox.ErrorQuery("Error", "Please select a service to view", "OK");
        }
    }

    private void ShowAbout()
    {
        MessageBox.Query("About SRunner",
            "SRunner v1.0\n\n" +
            "CLI application to run configured services and stacks\n\n" +
            "Built with Terminal.GUI and System.CommandLine",
            "OK");
    }
}
