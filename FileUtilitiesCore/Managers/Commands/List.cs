using CliFramework;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class List
    {
        public static void Command(string[] args)
        {
            if (Helpers.GetParameters(args, 0, new string[] { "-r" }, new string[] { "-i", "-e" }, out var flags, out var strs))
                Run(strs["-i"], strs["-e"], flags["-r"]);
            else PrettyConsole.PrintError($"Invalid arguments.");
        }

        private static void Run(string include, string exclude, bool recurse)
        {
            try
            {
                var option = recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                var currentDir = Directory.GetCurrentDirectory();
                var items = Directory.GetFiles(currentDir, "*", option).Select(path => Path.GetRelativePath(currentDir, path)).Union(Directory.GetDirectories(currentDir, "*", option).Select(path => Path.GetRelativePath(currentDir, path) + "\\"));
                if (!string.IsNullOrEmpty(include))
                {
                    if (!string.IsNullOrEmpty(exclude)) items = Helpers.Filter(items, include, exclude);
                    else items = Helpers.Filter(items, include);

                }
                if (!string.IsNullOrEmpty(exclude))
                {
                    items = Helpers.Filter(items, "**", exclude);
                }
                if (items.Count() < Helpers.resultsPerPage) PrettyConsole.PrintList(items);
                else PrettyConsole.PrintList(items, Helpers.resultsPerPage);
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError($"Could not list segments.\n{ex.Message}");
            }
        }
    }
}
