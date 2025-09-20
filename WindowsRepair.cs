using System.Diagnostics;

namespace WindowsRepair;

public abstract class WindowsRepair
{
    private static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("====================================");
            Console.WriteLine("  Windows System Maintenance Repair");
            Console.WriteLine("====================================");
            Console.WriteLine();
            Console.WriteLine("  1. Run SFC /scannow");
            Console.WriteLine("  2. Run DISM /CheckHealth");
            Console.WriteLine("  3. Run DISM /ScanHealth");
            Console.WriteLine("  4. Run DISM /RestoreHealth");
            Console.WriteLine("  5. Run SFC and DISM sequentially");
            Console.WriteLine("  6. Run Restart Recommended after doing all 1-4 or 5");
            Console.WriteLine("  7. Exit System or Select 6 to Restart Recommended");
            Console.WriteLine();
            Console.Write("Enter your choice (1-7): ");
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    RunSfc();
                    break;
                case "2":
                    RunDism("/Online /Cleanup-Image /CheckHealth", "DISM check health complete.");
                    break;
                case "3":
                    RunDism("/Online /Cleanup-Image /ScanHealth", "DISM scan health complete.");
                    break;
                case "4":
                    RunDism("/Online /Cleanup-Image /RestoreHealth", "DISM restore health complete.");
                    break;
                case "5":
                    RunSfcAndDism();
                    break;
                case "6":
                    RestartSystem();
                    return;
                case "7":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void RunSfc()
    {
        Console.Clear();
        Console.WriteLine("Running SFC /scannow...");
        RunCommand("sfc", "/scannow");
        Console.WriteLine();
        Console.WriteLine("System File Checker complete.");
        Pause();
    }

    private static void RunDism(string arguments, string completionMessage)
    {
        Console.Clear();
        Console.WriteLine($"Running DISM {arguments}...");
        RunCommand("Dism", arguments);
        Console.WriteLine();
        Console.WriteLine(completionMessage);
        Pause();
    }

    private static void RunSfcAndDism()
    {
        Console.Clear();
        Console.WriteLine("Running SFC /scannow and DISM /RestoreHealth...");
        RunCommand("sfc", "/scannow");
        Console.WriteLine();
        RunCommand("Dism", "/Online /Cleanup-Image /RestoreHealth");
        Console.WriteLine();
        Console.WriteLine("SFC and DISM operations complete.");
        Pause();
    }

    private static void RestartSystem()
    {
        Console.Clear();
        Console.WriteLine("Running Restarting System...");
        RunCommand("shutdown", "/r /t 0");
        Console.WriteLine();
        Console.WriteLine("Restart complete.");
        Pause();
    }

    private static void RunCommand(string fileName, string arguments)
    {
        try
        {
            var process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            Console.WriteLine(output);
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine("Error:");
                Console.WriteLine(error);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to run command {fileName} {arguments}: {ex.Message}");
        }
    }

    private static void Pause()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}