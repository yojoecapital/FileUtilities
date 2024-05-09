using CliFramework;
using Newtonsoft.Json;
using ArabizeCore.Managers;
using System.Diagnostics;
using Microsoft.Extensions.FileSystemGlobbing;

namespace FileUtilitiesCore.Managers.CommandManager
{
    internal static class Helpers
    {
        public static readonly int resultsPerPage = new SettingsFileManager().Settings.resultsPerPage;

        public static IEnumerable<string> Filter(IEnumerable<string> paths,  string filter) => new Matcher().AddInclude(filter).Match(paths).Files.Select(file => file.Path);
        public static IEnumerable<string> Filter(IEnumerable<string> paths,  string filter, string exclude) => new Matcher().AddInclude(filter).AddExclude(exclude).Match(paths).Files.Select(file => file.Path);

        public static void Info(string[] args)
        {
            if (args.Length != 2)
            {
                PrettyConsole.PrintError($"Invalid arguments.");
                return;
            }
            if (!File.Exists(args[1]))
            {
                PrettyConsole.PrintError($"File \"{args[1]}\" does not exist.");
                return;
            }
            var attributes = File.GetAttributes(args[1]);
            var json = JsonConvert.SerializeObject(new
            {
                Attributes = attributes.ToString(),
                Created = Directory.GetCreationTime(args[1]),
                LastAccessTime = Directory.GetLastAccessTime(args[1]),
                LastWriteTime = Directory.GetLastWriteTime(args[1])
            }, Formatting.Indented);
            PrettyConsole.PrintColor(json, ConsoleColor.Yellow);
        }

        public static void Size(string[] args)
        {
            if (args.Length != 2)
            {
                PrettyConsole.PrintError($"Invalid arguments.");
                return;
            }
            if (!File.Exists(args[1]))
            {
                PrettyConsole.PrintError($"File \"{args[1]}\" does not exist.");
                return;
            }
            var info = new FileInfo(args[1]);
            Console.WriteLine(info.Length);
        }

        public static void OpenSettings(string[] _)
        {
            var path = SettingsFileManager.SettingsFilePath;
            ProcessStartInfo psi = new()
            {
                FileName = path,
                UseShellExecute = true
            };
            Process.Start(psi);
            Console.WriteLine(path);
        }

        public static string PreprocessEnvironmentVariables(string input)
        {
            // replace "~" with the USERPROFILE environment variable
            string userprofile = Environment.GetEnvironmentVariable("USERPROFILE");
            if (!string.IsNullOrEmpty(userprofile))
            {
                input = input.Replace("~", userprofile);
            }

            // find and replace all %<var>% patterns with their environment variable values
            int start = 0;
            while ((start = input.IndexOf('%', start)) != -1)
            {
                int end = input.IndexOf('%', start + 1);
                if (end == -1) break;

                string envVarName = input.Substring(start + 1, end - start - 1);
                string envVarValue = Environment.GetEnvironmentVariable(envVarName);

                if (envVarValue != null)
                {
                    input = string.Concat(input.AsSpan()[..start], envVarValue, input.AsSpan(end + 1));
                    start += envVarValue.Length;
                }
                else
                {
                    start = end + 1;
                }
            }

            return input;
        }

        public static bool GetParameters(IEnumerable<string> args, int mandatory, IEnumerable<string> flags, IEnumerable<string> strs, out Dictionary<string, bool> flagResults, out Dictionary<string, string> strResults)
        {
            flagResults = null;
            strResults = null;
            if (args.Count() < mandatory + 1) return false;
            args = args.Skip(mandatory + 1);

            // Initialize output dictionaries
            flagResults = flags.ToDictionary(flag => flag, flag => false);
            strResults = strs.ToDictionary(str => str, str => "");

            // Temporary variable to hold the last string parameter name found in args
            string lastStrParam = null;
            var matches = 0;
            var provided = args.Count();

            foreach (var arg in args)
            {
                // Check if the current argument is a flag
                if (flags.Contains(arg))
                {
                    flagResults[arg] = true;
                    matches++;
                }
                else if (strs.Contains(arg))
                {
                    // Check if the current argument is a string parameter
                    lastStrParam = arg;
                    matches++;
                }
                else if (lastStrParam != null)
                {
                    // If the last argument was a string parameter, assign this argument as its value
                    strResults[lastStrParam] = arg;
                    lastStrParam = null; // Reset lastStrParam to ensure it's only used for one value
                    provided--;
                }
            }

            // Return true if all the provided args where matched
            return provided == matches;
        }
    }
}
