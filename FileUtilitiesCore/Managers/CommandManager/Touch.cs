using CliFramework;

namespace FileUtilitiesCore.Managers.CommandManager
{
    internal static class Touch
    {
        public static void Command(string[] args)
        {
            if (Helpers.GetParameters(args, 1, Array.Empty<string>(), Array.Empty<string>(), out var _, out var _))
                Run(args[1]);
            else PrettyConsole.PrintError($"Invalid arguments.");
        }

        private static void Run(string filePath)
        {
            try
            {
                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Update timestamps (last write and access time)
                    DateTime currentTime = DateTime.Now;
                    File.SetLastWriteTime(filePath, currentTime);
                    File.SetLastAccessTime(filePath, currentTime);
                }
                else
                {
                    // Create any intermediate directories if necessary
                    string directory = Path.GetDirectoryName(filePath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    // Create the new empty file
                    using FileStream fs = File.Create(filePath);
                    // Immediately dispose of the file stream
                }
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError($"Could not touch.\n{ex.Message}");
            }
        }
    }
}
