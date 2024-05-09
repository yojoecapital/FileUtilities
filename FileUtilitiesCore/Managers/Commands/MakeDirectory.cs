using CliFramework;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class MakeDirectory
    {
        public static void Command(string[] args)
        {
            if (Helpers.GetParameters(args, 1, Array.Empty<string>(), Array.Empty<string>(), out var _, out var _))
            {
                Run(args[1]);
            }
            else PrettyConsole.PrintError($"Invalid arguments.");
        }

        public static void Run(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
                    Directory.CreateDirectory(path);
                else PrettyConsole.PrintError($"Path \"{path}\" already exists.");
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError($"Could not make directory.\n{ex.Message}");
            }
        }
    }
}
