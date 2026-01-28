# SRunner
CLI application to run configured services and stacks

## Overview

SRunner is a C# CLI application built with .NET 10, Terminal.GUI, and System.CommandLine. It provides both a command-line interface and an interactive Terminal UI for managing and running configured services and stacks.

## Features

- **Interactive Terminal UI**: Launch a full-featured Terminal.GUI interface with `--interactive` flag
- **Service Management**: Add, remove, and view configured services
- **Modern Architecture**: Clean separation between Core business logic and CLI interface
- **.NET 10**: Built on the latest .NET framework

## Project Structure

```
SRunner/
├── src/
│   ├── Core/            # Business logic (non-UI)
│   │   └── ServiceRunner.cs
│   └── Cli/             # CLI and UI
│       ├── Program.cs
│       └── InteractiveUI.cs
├── .github/
│   └── PROJECT_STRUCTURE.md  # Detailed project documentation
└── SRunner.sln
```

## Building

```bash
dotnet build
```

## Usage

### Non-Interactive Mode

```bash
dotnet run --project src/Cli
```

This displays basic information about the CLI.

### Interactive Mode

```bash
dotnet run --project src/Cli -- --interactive
# or
dotnet run --project src/Cli -- -i
```

This launches the Terminal.GUI interactive interface where you can:
- View configured services
- Add new services
- Remove existing services
- View service details
- Navigate using keyboard shortcuts

### Help

```bash
dotnet run --project src/Cli -- --help
```

## Technologies

- **.NET 10.0**: Latest .NET framework
- **Terminal.Gui 1.19.0**: Cross-platform Terminal UI toolkit
- **System.CommandLine 2.0.0-beta4**: Command-line parsing
- **C# 13**: With nullable reference types enabled

## Development

See [.github/PROJECT_STRUCTURE.md](.github/PROJECT_STRUCTURE.md) for detailed development guidelines and architecture documentation.

## License

See [LICENSE](LICENSE) file for details.

