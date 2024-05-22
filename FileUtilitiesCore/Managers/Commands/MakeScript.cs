using CliFramework;
using FileUtilitiesCore.Utilities;
using System.Text.RegularExpressions;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class MakeScript
    {
        public static void Command(string[] args) => Run(args[2]);

        private static void Run(string name)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            if (name.Length == 0 || name.Any(c => invalidChars.Contains(c)) || name.Any(char.IsWhiteSpace))
            {
                PrettyConsole.PrintError("Invalid script name.");
                return;
            }
            try
            {
                string exe = null;
                PrettyConsole.PrintList(Helpers.fileManager.Settings.methods.Keys);
                while (exe == null)
                {
                    Console.Write("What is this script's execution method? (enter one of the above): ");
                    var input = Console.ReadLine().Trim();
                    if (Helpers.fileManager.Settings.methods.ContainsKey(input))
                    {
                        exe = input;
                    }
                    else PrettyConsole.PrintError($"Invalid executable \"{input}\".");
                }

                var endBlock = Helpers.fileManager.Settings.endBlock;
                Console.WriteLine($"Write your script. (enter in \"{endBlock}\") to finish):");
                var lines = new List<string>();
                string lineInput = null;
                while (true)
                {
                    lineInput = Console.ReadLine();
                    if (lineInput.Trim().Equals(endBlock)) break;
                    else lines.Add(lineInput);
                }
                var script = string.Join("\n", lines);

                Console.Write("Enter in a help message: ");
                var help = Console.ReadLine().Trim();


                var scriptItem = new ScriptItem()
                {
                    exe = exe,
                    help = help
                };

                Helpers.fileManager.SaveScriptItem(name, scriptItem, script);
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError($"Could not run script.\n{ex.Message}");
            }
        }
    }
}
