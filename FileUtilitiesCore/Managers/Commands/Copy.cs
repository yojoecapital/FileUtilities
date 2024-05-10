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
                var dirName = Path.GetDirectoryName(dest);
                if (!string.IsNullOrEmpty(dirName) && !Directory.Exists(dirName)) Directory.CreateDirectory(dirName);

                if (File.Exists(source))
                {
                    Console.Write($"Are you sure you want to copy the file \"{source}\" to \"{dest}\"? (y/n): ");
                    if (!Console.ReadLine().Trim().ToLower().Equals("y")) return;
                    // If the source is a file, copy directly to the destination
                    File.Copy(source, dest, overwrite);
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
                        if (!matchingFiles.Any()) return;

                        PrettyConsole.PrintList(matchingFiles);
                        Console.Write($"Are you sure you want to copy the above items to \"{displayDest}\"? (y/n): ");
                        if (!Console.ReadLine().Trim().ToLower().Equals("y")) return;

                        foreach (var filePath in matchingFiles)
                        {
                            // Combine the destination directory with the relative path to preserve the folder structure
                            var destinationPath = Path.Combine(dest, filePath);
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
                        if (!Console.ReadLine().Trim().ToLower().Equals("y")) return;
                        // If the source is a directory, copy all contents recursively
                        CopyDirectory(source, dest, overwrite);
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
