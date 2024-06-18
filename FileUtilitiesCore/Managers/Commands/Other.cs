using CliFramework;
using System.Diagnostics;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class Other
    {
        public static void Cd(string[] _) => Console.WriteLine(Directory.GetCurrentDirectory());

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
        
        public static void DirectoryScripts(string[] args)
        {
            if (Arg.Parse(args.Skip(1), 0, new [] { "-o" }, Array.Empty<string>(), out var _, out var flagResults, out var _))
            {
                var path = Helpers.fileManager.ScriptsFilePath;
                if (flagResults["-o"] && Directory.Exists(path))
                {
                    ProcessStartInfo psi = new()
                    {
                        FileName = path,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                Console.WriteLine(Path.GetFullPath(path));
            }
            else PrettyConsole.PrintError("Invalid arguments.");
        }

        public static void OpenScript(string[] args)
        {
            var item = Helpers.fileManager.GetScriptItem(args[2]);
            if (item == null) 
            {
                PrettyConsole.PrintError("Could not parse script item JSON file.");
                return;
            }
            Helpers.fileManager.OpenScriptItem(args[2], item);
        }


        public static void ListScripts(string[] _)
        {
            var path = Helpers.fileManager.ScriptsFilePath;
            if (Directory.Exists(path))
            {
                PrettyConsole.PrintList(Directory.GetFiles(path, "*.json").Select(file => Path.GetRelativePath(path, file)[..^5]));
            }
        }

    }
}
