using CliFramework;
using FileUtilitiesCore.Utilities;
using Newtonsoft.Json;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class Info
    {
        public static void Command(string[] args)
        {
            try
            {
                if (Arg.Parse(args.Skip(1), 0, true, out var _, out var spreadResults))
                {
                    Run(spreadResults);
                }
                else PrettyConsole.PrintError("Invalid arguments.");
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError(ex.Message);
            }
        }

        public static void Run(string[] paths)
        {
            var items = new List<FileInfoItem>();
            foreach (var path in paths)
            {
                if (!File.Exists(path) && !Directory.Exists(path))
                {
                    throw new Exception($"'{path}' does not exist.");
                }
                var attributes = File.GetAttributes(path);
                var isDirectory = (attributes & FileAttributes.Directory) == FileAttributes.Directory;
                if (isDirectory)
                {
                    var files = Directory.GetFiles(path).Length;
                    var subdirectories = Directory.GetDirectories(path).Length;
                    items.Add(
                        new DirectoryInfoItem()
                        {
                            type = "Directory",
                            path = Path.GetFullPath(path),
                            created = Directory.GetCreationTime(path),
                            bytes = Helpers.GetDirectorySize(path),
                            lastAccessTime = Directory.GetLastAccessTime(path),
                            lastWriteTime = Directory.GetLastWriteTime(path),
                            items = files + subdirectories,
                            files = files, 
                            subdirectories = subdirectories
                        }
                    );
                }
                else
                {
                    var info = new FileInfo(path);
                    items.Add(
                        new FileInfoItem()
                        {
                            type = "File",
                            path = Path.GetFullPath(path),
                            bytes = info.Length,
                            created = File.GetCreationTime(path),
                            lastAccessTime = File.GetLastAccessTime(path),
                            lastWriteTime = File.GetLastWriteTime(path)
                        }
                    );
                }
            }
            if (items.Count > 0)
            {
                var json = items.Count == 1 ? JsonConvert.SerializeObject(items[0], Formatting.Indented) : JsonConvert.SerializeObject(items, Formatting.Indented);
                PrettyConsole.PrintColor(json, ConsoleColor.Yellow);
            }
        }
    }
}
