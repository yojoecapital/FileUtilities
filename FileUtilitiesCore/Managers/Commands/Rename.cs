using CliFramework;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class Rename
    {
        public static void Command(string[] args)
        {
            if (Helpers.GetParameters(args, 2, Array.Empty<string>(), Array.Empty<string>(), out var _, out var _))
            {
                Run(args[1], args[2]);
            }
            else PrettyConsole.PrintError($"Invalid arguments.");
        }

        public static void Run(string path, string name)
        {
            var isValid = !string.IsNullOrEmpty(name) &&
                name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 &&
                !File.Exists(Path.Combine(name, name));

            if (!isValid)
            {
                PrettyConsole.PrintError($"\"{name}\" is not a valid file name.");
                return;
            }
            try
            {
                if (File.Exists(path))
                {
                    // If the path is a file, construct the new file path
                    string directory = Path.GetDirectoryName(path);
                    string newFilePath = Path.Combine(directory ?? string.Empty, name);

                    // Rename the file by moving it to the new file path
                    File.Move(path, newFilePath);
                }
                else if (Directory.Exists(path))
                {
                    // If the path is a directory, construct the new directory path
                    string parentDirectory = Path.GetDirectoryName(path);
                    string newDirectoryPath = Path.Combine(parentDirectory ?? string.Empty, name);

                    // Rename the directory by moving it to the new directory path
                    Directory.Move(path, newDirectoryPath);
                }
                else
                {
                    // Handle the case where the file or directory does not exist
                    PrettyConsole.PrintError($"Path \"{path}\" does not exist.");
                }
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError($"Could not rename.\n{ex.Message}");
            }
        }
    }
}
