using CliFramework;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class Copy
    {
        public static void Command(string[] args)
        {
            try
            {
                if (Arg.Parse(args.Skip(1), 1, true, new [] { "-r", "-o", "-y" }, new [] { "-i", "-e" }, out var mandatoryResults, out var spreadResults, out var flagResults, out var stringResults))
                {
                    fileOperations = new List<(string, string)>();
                    dirOperations = new List<(string, string)>();
                    var dest = mandatoryResults[0];
                    if (dest.Contains('*')) wildFlag = true;
                    else wildFlag = false;
                    Helpers.PerformOnItems(
                        spreadResults, stringResults["-i"], stringResults["-e"], flagResults["-r"],
                        file => OnFile(file, dest),
                        dir => OnDir(dir, dest),
                        (relativeTo, path) => OnPath(relativeTo, path, dest)
                    );
                    Run(flagResults["-o"], flagResults["-y"]);
                }
                else PrettyConsole.PrintError("Invalid arguments.");
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError(ex.Message);
            }
        }

        private static bool wildFlag;
        private static List<(string, string)> fileOperations;
        private static List<(string, string)> dirOperations;

        private static void OnFile(string source, string dest) => fileOperations.Add((source, Helpers.ProcessDestination(wildFlag, source, dest)));
        private static void OnDir(string source, string dest) => dirOperations.Add((source, Helpers.ProcessDestination(wildFlag, source, dest)));
        private static void OnPath(string relativeTo, string source, string dest) 
        {
            dest = Helpers.ProcessDestination(wildFlag, relativeTo, dest);
            dest = Path.Join(dest, Path.GetRelativePath(relativeTo, source));
            fileOperations.Add((source, dest));
        }

        public static void Run(bool overwrite, bool yes)
        {
            if (!yes && (fileOperations.Count > 0 || dirOperations.Count > 0))
            {
                foreach (var op in fileOperations) Console.WriteLine($"{op.Item1} → {op.Item2}");
                foreach (var op in dirOperations) 
                {
                    if (Helpers.IsDirectoryEmpty(op.Item1)) Console.WriteLine($"{Helpers.EnsureBackslash(op.Item1)} → {Helpers.EnsureBackslash(op.Item2)}");
                    else Console.WriteLine($"{Helpers.EnsureBackslash(op.Item1)}* → {Helpers.EnsureBackslash(op.Item2)}*");
                }
                Console.Write("Are you sure you want to copy the above items? (y/n): ");
                if (!Console.ReadLine().Trim().ToLower().Equals("y")) return;
            }
            foreach (var op in fileOperations) FileOperation(op.Item1, op.Item2, overwrite);
            foreach (var op in dirOperations) DirectoryOperation(op.Item1, op.Item2, overwrite);
        }

        private static void DirectoryOperation(string sourceDir, string destDir, bool overwrite)
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
                DirectoryOperation(subDir, newSubDir, overwrite);
            }
        }

        private static void FileOperation(string sourceFile, string destFile, bool overwrite)
        {
            var dir = Path.GetDirectoryName(destFile);
            if (!string.IsNullOrWhiteSpace(dir)) Directory.CreateDirectory(dir);
            File.Copy(sourceFile, destFile, overwrite);
        }
    }
}
