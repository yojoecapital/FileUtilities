using CliFramework;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class Find
    {
        public static void Command(string[] args)
        {
            if (Helpers.GetParameters(args, 1, new string[] { "-r" }, new string[] { "-i", "-e" }, out var flags, out var strs))
                Run(args[1], strs["-i"], strs["-e"], flags["-r"]);
            else PrettyConsole.PrintError($"Invalid arguments.");
        }

        private static void Run(string directory, string include, string exclude, bool recurse)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    PrettyConsole.PrintError($"Directory \"{directory}\" does not exist.");
                    return;
                }
                var option = recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                var items = Directory.GetFiles(directory, "*", option).Select(path => Path.GetRelativePath(directory, path)).Union(Directory.GetDirectories(directory, "*", option).Select(path => Path.GetRelativePath(directory, path) + "\\"));
                if (!string.IsNullOrEmpty(include) || !string.IsNullOrEmpty(exclude))
                {
                    if (string.IsNullOrEmpty(include)) include = "**";
                    items = Helpers.Filter(items, include, exclude);
                }
                if (items.Count() < Helpers.fileManager.Settings.resultsPerPage) PrettyConsole.PrintList(items);
                else PrettyConsole.PrintList(items, Helpers.fileManager.Settings.resultsPerPage);
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError($"Could not list segments.\n{ex.Message}");
            }
        }
    }
}
