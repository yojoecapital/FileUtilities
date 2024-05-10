using CliFramework;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class Exec
    {
        public static void Command(string[] args)
        {
            if (args.Length < 2) PrettyConsole.PrintError($"Invalid arguments.");
            else Run(args[1], args.Skip(2).ToArray());
        }

        private static void Run(string script, string[] args)
        {
            try
            {
                var scriptItem = Helpers.fileManager.GetScriptItem(script);
                if (scriptItem == null)
                {
                    PrettyConsole.PrintError("Could not parse script item JSON file.");
                    return;
                }
                var path = Path.Combine(Helpers.fileManager.ScriptsFilePath, scriptItem.id + ".bat");
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
                if (scriptItem.args != null && !AreAllMatched(scriptItem.args, args))
                {
                    PrettyConsole.PrintError($"Invalid arguments. Expected {scriptItem.args.Length} arguments.\nexec {script} {string.Join(' ', scriptItem.args.Select(arg => "@\"" + arg + "\""))}");
                    return;
                }

                // Set up the process start information
                var processStartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {path} {string.Join(" ", args.Select(arg => $"\"{arg}\""))}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // Start the process
                using Process process = Process.Start(processStartInfo);

                // Read the output from the process
                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();
                process.WaitForExit();  // Wait for the process to complete

                // Output the results to the console
                Console.WriteLine(output);

                if (!string.IsNullOrEmpty(errors))
                {
                    PrettyConsole.PrintError(errors);
                }
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError($"Could not run script.\n{ex.Message}");
            }
        }

        public static bool AreAllMatched(string[] patterns, string[] strings)
        {
            // Check if the arrays are of the same length
            if (patterns.Length != strings.Length)
            {
                return false;
            }

            // Iterate through the patterns and strings arrays and match each pattern
            for (int i = 0; i < patterns.Length; i++)
            {
                string pattern = patterns[i];
                string input = strings[i];

                // Check if the current input string matches the regex pattern
                if (!Regex.IsMatch(input, pattern))
                {
                    return false;
                }
            }

            // If all strings match their corresponding patterns, return true
            return true;
        }

        private static char[] specialChars = { '&', '<', '>', '|', '(', ')', '^', '"' };
        public static bool ContainsSpecialCharacters(string input)
        {
            return input.IndexOfAny(specialChars) != -1;
        }
    }
}
