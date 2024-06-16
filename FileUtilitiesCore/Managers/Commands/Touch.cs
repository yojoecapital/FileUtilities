using CliFramework;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class Touch
    {
        public static void Command(string[] args)
        {
            try
            {
                if (Arg.Parse(args.Skip(1), 1, out var mandatoryResults))
                {
                    Run(mandatoryResults[0]);
                }
                else PrettyConsole.PrintError("Invalid arguments.");
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError(ex.Message);
            }
        }

        public static void Run(string filePath)
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
    }
}
