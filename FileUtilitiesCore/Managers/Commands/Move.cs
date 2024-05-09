using CliFramework;
 
namespace FileUtilitiesCore.Managers.Commands
{
    internal static class Move
    {
        public static void Command(string[] args)
        {
            if (Helpers.GetParameters(args, 2, new string[] { "-r", "-p", "-o" }, Array.Empty<string>(), out var flags, out var _))
            {
                Run(args[1], args[2], flags["-p"], flags["-r"], flags["-o"]);
            }
            else PrettyConsole.PrintError($"Invalid arguments.");
        }

        private static void Run(string source, string dest, bool pattern, bool recurse, bool overwrite)
        {
            try
            {
                // Ensure the destination directory exists
                if (!Directory.Exists(dest)) Directory.CreateDirectory(dest);

                if (pattern)
                {
                    // When `pattern` is true, treat `source` as a search pattern
                    var currentDir = Directory.GetCurrentDirectory();
                    var option = recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                    var matchingFiles = Directory.GetFiles(currentDir, "*", option);
                    //matchingFiles = Helpers.Filter(matchingFiles, source).ToArray();
                    if (matchingFiles.Length == 0) return;
                    PrettyConsole.PrintList(matchingFiles.Select(path => Path.GetRelativePath(currentDir, path)));
                    Console.Write($"Are you sure you want to move the above items into \"{dest}\"? (y/n): ");
                    if (!Console.ReadLine().ToLower().Equals("y")) return;
                    foreach (var filePath in matchingFiles)
                    {
                        // Determine the relative path of each file within the current directory
                        var relativePath = Path.GetRelativePath(currentDir, filePath);

                        // Combine the destination directory with the relative path to preserve the folder structure
                        var destinationPath = Path.Combine(dest, relativePath);
                        var destinationDir = Path.GetDirectoryName(destinationPath);

                        // Ensure the directory structure is preserved
                        if (destinationDir != null && !Directory.Exists(destinationDir))
                            Directory.CreateDirectory(destinationDir);

                        // Move the file
                        File.Move(filePath, destinationPath, overwrite);
                    }
                }
                else
                {
                    // When `pattern` is false, handle copying a file or directory
                    if (File.Exists(source))
                    {
                        Console.Write($"Are you sure you want to move the file \"{source}\" into \"{dest}\"? (y/n): ");
                        if (!Console.ReadLine().ToLower().Equals("y")) return;
                        var destinationFile = Path.Combine(dest, Path.GetFileName(source));
                        // If the source is a file, move directly to the destination
                        File.Move(source, destinationFile, overwrite);
                    }
                    else if (Directory.Exists(source))
                    {
                        Console.Write($"Are you sure you want to move the directory \"{source}\" into \"{dest}\"? (y/n): ");
                        if (!Console.ReadLine().ToLower().Equals("y")) return;
                        // If the source is a directory, move all contents recursively
                        var destinationDir = Path.Combine(dest, Path.GetFileName(source));
                        MergeDirectories(source, destinationDir, overwrite);
                    }
                    else PrettyConsole.PrintError($"\"{source}\" does not exist.");
                }
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError($"Could not move.\n{ex.Message}");
            }
        }

        public static void MergeDirectories(string sourceDir, string destinationDir, bool overwrite)
        {
            // Move each file in the source directory to the destination directory
            foreach (string sourceFilePath in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(sourceFilePath);
                string destFilePath = Path.Combine(destinationDir, fileName);
                File.Move(sourceFilePath, destFilePath, overwrite);
            }

            // Recursively move subdirectories to the destination directory
            foreach (string sourceSubDirPath in Directory.GetDirectories(sourceDir))
            {
                string directoryName = Path.GetFileName(sourceSubDirPath);
                string destSubDirPath = Path.Combine(destinationDir, directoryName);

                // Recursively merge subdirectories
                MergeDirectories(sourceSubDirPath, destSubDirPath, overwrite);
            }

            // Optionally delete the source directory if empty or as needed
            if (Directory.GetFileSystemEntries(sourceDir).Length == 0)
                Directory.Delete(sourceDir);
        }
    }
}
