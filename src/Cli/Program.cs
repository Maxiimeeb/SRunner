using Core;

namespace Cli;

class Program
{
    static void Main(string[] args)
    {
        // Simple argument parsing for --interactive flag
        bool interactive = args.Contains("--interactive") || args.Contains("-i");

        if (interactive)
        {
            LaunchInteractiveMode();
        }
        else
        {
            Console.WriteLine("SRunner - CLI application to run configured services and stacks");
            Console.WriteLine("Use --interactive or -i to launch the interactive interface");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  --interactive, -i  Launch interactive Terminal.GUI interface");
        }
    }

    static void LaunchInteractiveMode()
    {
        var ui = new InteractiveUI();
        ui.Run();
    }
}
