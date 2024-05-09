﻿using CliFramework;

namespace FileUtilitiesCore.Managers.CommandManager
{
    internal static class List
    {
        public static void Command(string[] args)
        {
            if (Helpers.GetParameters(args, 0, new string[] { "-r" }, new string[] { "-f", "-e" }, out var flags, out var strs))
                Run(strs["-f"], strs["-e"], flags["-r"]);
            else PrettyConsole.PrintError($"Invalid arguments.");
        }

        private static void Run(string filter, string exclude, bool recurse)
        {
            var option = recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var currentDir = Directory.GetCurrentDirectory();
            var items = Directory.GetFiles(currentDir, "*", option).Select(path => Path.GetRelativePath(currentDir, path)).Union(Directory.GetDirectories(currentDir, "*", option).Select(path => Path.GetRelativePath(currentDir, path) + "\\"));
            if (!string.IsNullOrEmpty(filter))
            {
                if (!string.IsNullOrEmpty(exclude)) items = Helpers.Filter(items, filter, exclude);
                else items = Helpers.Filter(items, filter);

            }
            if (items.Count() < Helpers.resultsPerPage) PrettyConsole.PrintList(items);
            else PrettyConsole.PrintList(items, Helpers.resultsPerPage);
        }
    }
}