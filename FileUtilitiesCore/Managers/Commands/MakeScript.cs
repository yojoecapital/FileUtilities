using CliFramework;
using FileUtilitiesCore.Utilities;
using System.Text.RegularExpressions;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class MakeScript
    {
        public static void Command(string[] _) => Run();

        private static void Run()
        {
            try
            {
                string name = null;
                while (name == null)
                {
                    Console.Write("What is the name of your script?: ");
                    var input = Console.ReadLine().Trim();

                    // Check for any invalid characters
                    char[] invalidChars = Path.GetInvalidFileNameChars();
                    if (input.Length > 0 && !input.Any(c => invalidChars.Contains(c)) && !input.Any(char.IsWhiteSpace))
                    {
                        name = input;
                    }
                    else PrettyConsole.PrintError("Invalid script name.");
                }

                int argCount = 0;
                bool validCount = false;
                while (!validCount)
                {
                    Console.Write("How many arguments does this script take? (click enter to skip argument checking): ");
                    var input = Console.ReadLine().Trim();
                    if (input.Length > 0)
                    {
                        if (!int.TryParse(input, out argCount)) PrettyConsole.PrintError("Invalid input.");
                        else validCount = true;
                    }
                    else
                    {
                        validCount = true;
                        argCount = -1;
                    }
                }

                string[] args = null;
                if (argCount != -1)
                {
                    args = new string[argCount];
                    for (int i = 0; i < args.Length; i++)
                    {
                        bool valid = false;
                        while (!valid)
                        {
                            Console.Write($"Enter a regular expression to check argument {i + 1}. (click enter for @\".*\"): ");
                            var input = Console.ReadLine().Trim();
                            if (input.Length == 0) input = ".*";
                            try
                            {
                                _ = new Regex(input);
                                valid = true;
                                args[i] = input;
                            }
                            catch (Exception ex)
                            {
                                PrettyConsole.PrintError(ex.Message);
                            }
                        }
                    }
                }

                var endBlock = Helpers.fileManager.Settings.endBlock;
                Console.WriteLine($"Enter in your batch script. (enter in \"{endBlock}\") to finish):");
                var lines = new List<string>();
                string lineInput = null;
                while (true)
                {
                    lineInput = Console.ReadLine();
                    if (lineInput.Trim().Equals(endBlock)) break;
                    else lines.Add(lineInput);
                }

                var script = string.Join("\n", lines);
                string id = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");

                var scriptItem = new ScriptItem()
                {
                    id = id,
                    args = args
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
