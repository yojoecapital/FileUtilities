using CliFramework;
using Microsoft.VisualBasic.FileIO;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class Remove
    {
        public static void Command(string[] args)
        {
            if (Helpers.GetParameters(args, 1, new string[] { "-r" }, new string[] { "-i", "-e" }, out var flags, out var strs))
            {
                Run(args[1], strs["-i"], strs["-e"], flags["-r"]);
            }
            else PrettyConsole.PrintError($"Invalid arguments.");
        }

        public static void Run(string source, string include, string exclude, bool recurse)
        {
            try
            {
                // When pattern is false, remove a specific file or directory
                if (File.Exists(source))
                {
                    // If it's a file, delete it directly
                    Console.Write($"Are you sure you want to remove the file \"{source}\"? (y/n): ");
                    if (!Console.ReadLine().ToLower().Equals("y")) return;
                    FileSystem.DeleteFile(source, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
                else if (Directory.Exists(source))
                {
                    if (!string.IsNullOrEmpty(include) || !string.IsNullOrEmpty(include))
                    {
                        source = Path.GetFullPath(source);
                        Directory.SetCurrentDirectory(source);

                        // If using a pattern then we'll filter
                        var option = recurse ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly;

                        // Find files and directories matching the pattern
                        var files = Directory.GetFiles(source, "*", option);

                        var matchingFiles = Helpers.Filter(files.Select(path => Path.GetRelativePath(source, path)), include, exclude);

                        if (!matchingFiles.Any()) return;
                        PrettyConsole.PrintList(matchingFiles);
                        Console.Write($"Are you sure you want to remove the above items? (y/n): ");
                        if (!Console.ReadLine().ToLower().Equals("y")) return;

                        // Remove all matching files
                        foreach (string file in matchingFiles)
                            FileSystem.DeleteFile(file, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    }
                    else
                    {

                        // If it's a directory, remove it recursively
                        Console.Write($"Are you sure you want to remove the {(Helpers.IsDirectoryEmpty(source) ? "" : "* ")}directory \"{source}\"? (y/n): ");
                        if (!Console.ReadLine().ToLower().Equals("y")) return;
                        FileSystem.DeleteDirectory(source, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
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
