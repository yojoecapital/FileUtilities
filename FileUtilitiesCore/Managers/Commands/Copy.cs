using CliFramework;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class Copy
    {
        public static void Command(string[] args)
        {
            if (Helpers.GetParameters(args, 2, new string[] { "-r", "-o" }, new string[] { "-i", "-e" }, out var flags, out var strs))
            {
                Run(args[1], args[2], strs["-i"], strs["-e"], flags["-r"], flags["-o"]);
            }
            else PrettyConsole.PrintError($"Invalid arguments.");
        }

        private static void Run(string source, string dest, string include, string exclude, bool recurse, bool overwrite)
        {
            try
            {
                // Ensure the destination directory exists
                if (!Directory.Exists(dest)) Directory.CreateDirectory(dest);

                // When `pattern` is false, handle copying a file or directory
                if (File.Exists(source))
                {
                    Console.Write($"Are you sure you want to copy the file \"{source}\" to \"{dest}\"? (y/n): ");
                    if (!Console.ReadLine().ToLower().Equals("y")) return;
                    var destinationFile = Path.Combine(dest, Path.GetFileName(source));
                    // If the source is a file, copy directly to the destination
                    File.Copy(source, destinationFile, overwrite);
                }
                else if (Directory.Exists(source))
                {
                    if (!string.IsNullOrEmpty(include) || !string.IsNullOrEmpty(exclude))
                    {
                        source = Path.GetFullPath(source);
                        var displayDest = dest;
                        dest = Path.GetFullPath(dest);
                        Directory.SetCurrentDirectory(source);

                        var option = recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                        var files = Directory.GetFiles(source, "*", option);
                        var matchingFiles = Helpers.Filter(files.Select(path => Path.GetRelativePath(source, path)), include, exclude);
                        source = Path.GetDirectoryName(source);
                        if (!matchingFiles.Any()) return;

                        PrettyConsole.PrintList(matchingFiles);
                        Console.Write($"Are you sure you want to copy the above items to \"{displayDest}\"? (y/n): ");
                        if (!Console.ReadLine().ToLower().Equals("y")) return;

                        foreach (var filePath in matchingFiles)
                        {
                            // Determine the relative path of each file within the current directory
                            var relativePath = Path.GetRelativePath(source, filePath);

                            // Combine the destination directory with the relative path to preserve the folder structure
                            var destinationPath = Path.Combine(dest, relativePath);
                            var destinationDir = Path.GetDirectoryName(destinationPath);

                            // Ensure the directory structure is preserved
                            if (destinationDir != null && !Directory.Exists(destinationDir))
                                Directory.CreateDirectory(destinationDir);

                            // Copy the file
                            File.Copy(filePath, destinationPath, overwrite);
                        }
                    }
                    else
                    {
                        Console.Write($"Are you sure you want to copy the {(Helpers.IsDirectoryEmpty(source) ? "" : "* ")}directory \"{source}\" into \"{dest}\"? (y/n): ");
                        if (!Console.ReadLine().ToLower().Equals("y")) return;
                        // If the source is a directory, copy all contents recursively
                        var destinationDir = Path.Combine(dest, Path.GetFileName(source));
                        CopyDirectory(source, destinationDir, overwrite);
                    }
                }
                else PrettyConsole.PrintError($"\"{source}\" does not exist.");
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError($"Could not copy.\n{ex.Message}");
            }
        }

        // Helper method to copy directory contents recursively
        private static void CopyDirectory(string sourceDir, string destDir, bool overwrite)
        {
            Directory.CreateDirectory(destDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var destFile = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, destFile, overwrite);
            }

            foreach (var subDir in Directory.GetDirectories(sourceDir))
            {
                var newSubDir = Path.Combine(destDir, Path.GetFileName(subDir));
                CopyDirectory(subDir, newSubDir, overwrite);
            }
        }
    }
}
