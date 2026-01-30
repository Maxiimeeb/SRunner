using System.CommandLine;
using Microsoft.Extensions.Logging;
using SRunner.Core;
using SRunner.Core.Serialization;

namespace SRunner.Cli;

class Program
{
    private static async Task<int> Main(string[] args)
    {
        Test();
        return 0;
        // Create the root command  
        var rootCommand = new RootCommand("SRunner - CLI application to run configured services and stacks");

        // Create the --interactive option
        var interactiveOption = new Option<bool>(
            name: "--interactive",
            description: "Launch interactive Terminal.GUI interface"
        );
        interactiveOption.AddAlias("-i");
        rootCommand.AddOption(interactiveOption);

        // Set the handler for the root command
        rootCommand.SetHandler((interactive) =>
        {
            if (interactive)
            {
                LaunchInteractiveMode();
            }
            else
            {
                Console.WriteLine("SRunner - CLI application to run configured services and stacks");
                Console.WriteLine("Use --interactive or -i to launch the interactive interface");
            }
        }, interactiveOption);

        return await rootCommand.InvokeAsync(args);
    }

    static void LaunchInteractiveMode()
    {
        var ui = new InteractiveUi();
    }

    private static void Test()
    {
        var logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        }).CreateLogger<HomeDirectoryLoader>();
        var x = new HomeDirectoryLoader(logger);

        x.Load();
    }
}