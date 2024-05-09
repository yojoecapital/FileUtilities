using CliFramework;
using Microsoft.VisualBasic.FileIO;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class Remove
    {
        public static void Command(string[] args)
        {
            if (Helpers.GetParameters(args, 1, new string[] { "-r" }, Array.Empty<string>(), out var flags, out var _))
            {
                Run(args[1], flags["-p"], flags["-r"]);
            }
            else PrettyConsole.PrintError($"Invalid arguments.");
        }

        public static void Run(string path, bool pattern, bool recurse)
        {
            try
            {
                if (pattern)
                {
                    var option = recurse ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly;

                    // When pattern is true, treat `path` as a wildcard pattern
                    var currentDir = Directory.GetCurrentDirectory();

                    // Find files and directories matching the pattern
                    var files = Directory.GetFiles(currentDir, "*", option);
                    var dirs = Directory.GetDirectories(currentDir, "*", option);

                    var matchingFiles = Helpers.Filter(files.Select(path => Path.GetRelativePath(currentDir, path)), path);
                    var matchingDirectories = Helpers.Filter(dirs.Select(path => Path.GetRelativePath(currentDir, path) + "\\"), path);

                    if (!matchingFiles.Any() && !matchingDirectories.Any()) return;
                    PrettyConsole.PrintList(matchingFiles.Union(matchingDirectories));
                    Console.Write($"Are you sure you want to remove the above items? (y/n): ");
                    if (!Console.ReadLine().ToLower().Equals("y")) return;
                    
                    // Remove all matching files
                    foreach (string file in matchingFiles)
                        FileSystem.DeleteFile(file, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);

                    // Remove all matching directories
                    foreach (string directory in matchingDirectories)
                        FileSystem.DeleteDirectory(directory, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
                else
                {
                    // When pattern is false, remove a specific file or directory
                    if (File.Exists(path))
                    {
                        // If it's a file, delete it directly
                        Console.Write($"Are you sure you want to remove the file \"{path}\"? (y/n): ");
                        if (!Console.ReadLine().ToLower().Equals("y")) return;
                        FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    }
                    else if (Directory.Exists(path))
                    {
                        // If it's a directory, remove it recursively
                        Console.Write($"Are you sure you want to remove the directory \"{path}\"? (y/n): ");
                        if (!Console.ReadLine().ToLower().Equals("y")) return;
                        FileSystem.DeleteDirectory(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    }
                }
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError($"Could not remove.\n{ex.Message}");
            }
        }
    }
}
