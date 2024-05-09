using CliFramework;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class Copy
    {
        public static void Command(string[] args)
        {
            if (Helpers.GetParameters(args, 2, new string[] { "-r", "-p" }, Array.Empty<string>(), out var flags, out var _))
            {
                Run(args[1], args[2], flags["-p"], flags["-r"]);
            }
            else PrettyConsole.PrintError($"Invalid arguments.");
        }

        private static void Run(string source, string dest, bool pattern, bool recurse)
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
                    matchingFiles = Helpers.Filter(matchingFiles, source).ToArray();
                    if (matchingFiles.Length == 0) return;
                    PrettyConsole.PrintList(matchingFiles.Select(path => Path.GetRelativePath(currentDir, path)));
                    Console.Write($"Are you sure you want to copy the above items to \"{dest}\"? (y/n): ");
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

                        // Copy the file
                        File.Copy(filePath, destinationPath, true);
                    }
                }
                else
                {
                    // When `pattern` is false, handle copying a file or directory
                    if (File.Exists(source))
                    {
                        Console.Write($"Are you sure you want to copy the file \"{source}\" to \"{dest}\"? (y/n): ");
                        if (!Console.ReadLine().ToLower().Equals("y")) return;
                        var destinationFile = Path.Combine(dest, Path.GetFileName(source));
                        // If the source is a file, copy directly to the destination
                        File.Copy(source, destinationFile, true);
                    }
                    else if (Directory.Exists(source))
                    {
                        Console.Write($"Are you sure you want to copy the directory \"{source}\" to \"{dest}\"? (y/n): ");
                        if (!Console.ReadLine().ToLower().Equals("y")) return;
                        // If the source is a directory, copy all contents recursively
                        var destinationDir = Path.Combine(dest, Path.GetFileName(source));
                        CopyDirectory(source, destinationDir);
                    }
                    else PrettyConsole.PrintError($"\"{source}\" does not exist.");
                }
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError($"Could not copy.\n{ex.Message}");
            }
        }

        // Helper method to copy directory contents recursively
        private static void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var destFile = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            foreach (var subDir in Directory.GetDirectories(sourceDir))
            {
                var newSubDir = Path.Combine(destDir, Path.GetFileName(subDir));
                CopyDirectory(subDir, newSubDir);
            }
        }
    }
}
