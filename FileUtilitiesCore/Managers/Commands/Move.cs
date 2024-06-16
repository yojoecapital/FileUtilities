using CliFramework;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class Move
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

        private static string ProcessDest(string source, string dest) 
        {
            if (wildFlag) return dest.Replace("*", Path.GetFileName(source));
            return dest;
        }

        private static void OnFile(string source, string dest) => fileOperations.Add((source, ProcessDest(source, dest)));
        private static void OnDir(string source, string dest) => dirOperations.Add((source, ProcessDest(source, dest)));
        private static void OnPath(string relativeTo, string source, string dest) 
        {
            dest = ProcessDest(relativeTo, dest);
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
                    if (Helpers.IsDirectoryEmpty(op.Item1)) Console.WriteLine($"{op.Item1}\\ → {op.Item2}\\");
                    else Console.WriteLine($"{op.Item1}\\* → {op.Item2}\\*");
                }
                Console.Write("Are you sure you want to move the above items? (y/n): ");
                if (!Console.ReadLine().Trim().ToLower().Equals("y")) return;
            }
            foreach (var op in fileOperations) FileOperation(op.Item1, op.Item2, overwrite);
            foreach (var op in dirOperations) DirectoryOperation(op.Item1, op.Item2, overwrite);
        }

        private static void DirectoryOperation(string sourceDir, string destDir, bool overwrite)
        {
            Directory.CreateDirectory(destDir);

            // Move each file in the source directory to the destination directory
            foreach (string sourceFile in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(sourceFile);
                string dest = Path.Combine(destDir, fileName);
                File.Move(sourceFile, dest, overwrite);
            }

            // Recursively move subdirectories to the destination directory
            foreach (string sourceSubDir in Directory.GetDirectories(sourceDir))
            {
                string dirName = Path.GetFileName(sourceSubDir);
                string destSubDir = Path.Combine(destDir, dirName);

                // Recursively merge subdirectories
                DirectoryOperation(sourceSubDir, destSubDir, overwrite);
            }

            // Delete the source directory if empty or as needed
            if (Directory.GetFileSystemEntries(sourceDir).Length == 0) Directory.Delete(sourceDir);
        }

        private static void FileOperation(string sourceFile, string destFile, bool overwrite)
        {
            var dir = Path.GetDirectoryName(destFile);
            Directory.CreateDirectory(dir);
            File.Move(sourceFile, destFile, overwrite);
        }
    }
}
