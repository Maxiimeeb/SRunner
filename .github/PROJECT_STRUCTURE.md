# SRunner Project Structure

## Overview
SRunner is a C# CLI application to run configured services and stacks, built with Terminal.GUI, .NET 10, and System.CommandLine.

## Project Structure

```
SRunner/
├── .github/              # GitHub configuration and AI instructions
│   └── PROJECT_STRUCTURE.md
├── src/                  # Source code directory
│   ├── Core/            # Core business logic (non-UI)
│   │   ├── ServiceConfig.cs      # Service configuration model
│   │   └── ServiceRunner.cs      # Service management logic
│   └── Cli/             # Command-line interface (entry point)
│       ├── Program.cs            # Entry point with System.CommandLine
│       ├── InteractiveUI.cs      # Terminal.GUI interactive interface
│       └── Cli.csproj            # CLI project file
├── SRunner.sln          # Solution file
├── .gitignore           # Git ignore file
└── README.md            # Project documentation
```

## Key Technologies

- **Framework**: .NET 10
- **UI Library**: Terminal.Gui 1.19.0
- **CLI Framework**: System.CommandLine 2.0.0-beta4.22272.1 (Note: RC version 2.0.0-rc.1.25451.107 has API incompatibilities with .NET 10, so using stable beta4 version instead)
- **Language**: C# with nullable reference types enabled

## Architecture

### Core Project (`src/Core/`)
- **Purpose**: Contains business logic and models independent of the user interface
- **Components**:
  - `ServiceConfig`: Data model for service configuration
  - `ServiceRunner`: Manages service lifecycle and state
- **Target Framework**: net10.0
- **Type**: Class Library

### Cli Project (`src/Cli/`)
- **Purpose**: Entry point and user interface implementation
- **Components**:
  - `Program.cs`: Main entry point using System.CommandLine for argument parsing
  - `InteractiveUI.cs`: Terminal.GUI-based interactive interface
- **Target Framework**: net10.0
- **Type**: Console Application
- **Dependencies**: 
  - Core project reference
  - Terminal.Gui package
  - System.CommandLine package

## Usage

### Build the Project
```bash
dotnet build
```

### Run the CLI
```bash
# Non-interactive mode
dotnet run --project src/Cli

# Interactive mode with Terminal.GUI
dotnet run --project src/Cli -- --interactive
# or
dotnet run --project src/Cli -- -i
```

## Development Guidelines

### Adding New Features

1. **Core Logic**: Add new business logic to `src/Core/`
   - Keep UI-independent
   - Follow existing patterns
   - Add models and services as needed

2. **UI Features**: Add new UI features to `src/Cli/`
   - UI code goes in `InteractiveUI.cs` or new UI classes
   - CLI argument handling goes in `Program.cs`

3. **Dependencies**: 
   - Keep Core project minimal with no UI dependencies
   - UI packages only in Cli project

### Code Style
- Use nullable reference types
- Follow C# naming conventions
- Keep methods focused and single-purpose
- Add XML documentation comments for public APIs

### Project References
- Cli project references Core project
- Core project has no dependencies on Cli

## Command-Line Options

- `--interactive` or `-i`: Launch the Terminal.GUI interactive interface
- Without flags: Shows basic help information

## Interactive Interface Features

The Terminal.GUI interface provides:
- Service list view
- Add new services with dialog
- Remove services with confirmation
- View service details
- Menu bar with File and Help options
- Keyboard shortcuts for all actions

## Future Enhancements

Potential areas for expansion:
- Actual service execution (start/stop processes)
- Service status monitoring
- Configuration file persistence (JSON/YAML)
- Logging and diagnostics
- Service dependency management
- Multi-stack support
- Environment variable management
