using System.CommandLine;
using Core;

namespace Cli;

class Program
{
    static async Task<int> Main(string[] args)
    {
        // Create the root command  
        var rootCommand = new RootCommand("SRunner - CLI application to run configured services and stacks");

        // Create the --interactive option
        var interactiveOption = new Option<bool>(
            name: "--interactive",
            description: "Launch interactive Terminal.GUI interface");
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
        var ui = new InteractiveUI();
        ui.Run();
    }
}
