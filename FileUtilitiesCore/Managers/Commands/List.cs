using CliFramework;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class List
    {
        public static void Command(string[] args)
        {
            try
            {
                if (Arg.Parse(args.Skip(1), 0, new [] { "-r" }, new [] { "-i", "-e" }, out var mandatoryResults, out var flagResults, out var stringResults))
                {
                    Run(stringResults["-i"], stringResults["-e"], flagResults["-r"], true);
                }
                else PrettyConsole.PrintError("Invalid arguments.");
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError(ex.Message);
            }
        }

        public static void Run(string include, string exclude, bool recurse, bool cd)
        {
            var dir = ".";
            var option = recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var items = Directory.GetFiles(dir, "*", option).Select(path => Path.GetRelativePath(dir, path)).Union(Directory.GetDirectories(dir, "*", option).Select(path => Helpers.EnsureBackslash(Path.GetRelativePath(dir, path))));
            if (!string.IsNullOrEmpty(include) || !string.IsNullOrEmpty(exclude))
            {
                if (string.IsNullOrEmpty(include)) include = "**";
                items = Helpers.Filter(items, include, exclude);
            }
            if (cd) items = items.Select(path => Helpers.GetNormalizedPath(Path.Combine(dir, path)));
            PrettyConsole.PrintList(items);
        }
    }
}
