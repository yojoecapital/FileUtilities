using CliFramework;
using System.Diagnostics;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class Exec
    {
        public static void Command(string[] args)
        {
            try
            {
                if (Arg.Parse(args.Skip(1), 1, false, out var mandatoryResults, out var spreadResults))
                {
                    Run(mandatoryResults[0], spreadResults);
                }
                else PrettyConsole.PrintError("Invalid arguments.");
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError(ex.Message);
            }
        }

        public static void Run(string name, string[] args)
        {
            var item = Helpers.fileManager.GetScriptItem(name);
            if (item == null)
            {
                PrettyConsole.PrintError("Script item does not exist.");
                return;
            }
            if (!Helpers.fileManager.Settings.methods.ContainsKey(item.exe)) 
            {
                PrettyConsole.PrintError("Could not find execution method in settings JSON file.");
                return;
            }
            var method = Helpers.fileManager.Settings.methods[item.exe];
            if (item == null)
            {
                PrettyConsole.PrintError("Could not parse script item JSON file.");
                return;
            }
            var path = Path.Combine(Helpers.fileManager.ScriptsFilePath, name + "." + method.extension);
            if (!File.Exists(path))
            {
                PrettyConsole.PrintError("Script does not exist.");
                return;
            }
            if (args.Any(arg => ContainsSpecialCharacters(arg)))
            {
                PrettyConsole.PrintError($"Invalid arguments. No argument should contain the characters {string.Join(", ", specialChars.Select(c => "\"" + c + "\""))}.");
                return;
            }
            if (args.Length == 1 && (args[0].Equals("help") || args[0].Equals("h")))
            {
                Console.Write(item.help);
                return;
            }

            // Set up the process start information
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = method.path,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            foreach(var arg in method.setup) processStartInfo.ArgumentList.Add(arg);
            processStartInfo.ArgumentList.Add(path);
            foreach(var arg in args) processStartInfo.ArgumentList.Add(arg);

            // Start the process
            using Process process = Process.Start(processStartInfo);

            // Read the output from the process
            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();
            
            // Wait for the process to complete
            process.WaitForExit();

            // Output the results to the console
            Console.Write(output);

            if (!string.IsNullOrEmpty(errors))
            {
                Console.Write(errors);
            }
        }

        private static char[] specialChars = { '&', '<', '>', '|', '(', ')', '^', '"' };
        public static bool ContainsSpecialCharacters(string input) => input.IndexOfAny(specialChars) != -1;
    }
}
