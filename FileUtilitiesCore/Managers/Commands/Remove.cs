using CliFramework;
using Microsoft.VisualBasic.FileIO;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class Remove
    {
        public static void Command(string[] args)
        {
            try
            {
                if (Arg.Parse(args.Skip(1), 0, true, new [] { "-r", "-f", "-y" }, new [] { "-i", "-e" }, out var _, out var spreadResults, out var flagResults, out var stringResults))
                {
                    fileOperations = new List<string>();
                    dirOperations = new List<string>();
                    Helpers.PerformOnItems(
                        spreadResults, stringResults["-i"], stringResults["-e"], flagResults["-r"],
                        OnFile,
                        OnDir,
                        OnPath
                    );
                    Run(flagResults["-f"], flagResults["-y"]);
                }
                else PrettyConsole.PrintError("Invalid arguments.");
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError(ex.Message);
            }
        }

        private static List<string> fileOperations;
        private static List<string> dirOperations;

        private static void OnFile(string source) => fileOperations.Add(source);
        private static void OnDir(string source) => dirOperations.Add(source);
        private static void OnPath(string _, string source) => fileOperations.Add(source);

        public static void Run(bool force, bool yes)
        {
            if (!yes && (fileOperations.Count > 0 || dirOperations.Count > 0))
            {
                if (force)
                {
                    foreach (var op in fileOperations) Console.WriteLine($"{op}");
                    foreach (var op in dirOperations) 
                    {
                        if (Helpers.IsDirectoryEmpty(op)) Console.WriteLine($"Helpers.EnsureBackslash(op)");
                        else Console.WriteLine($"Helpers.EnsureBackslash(op)*");
                    }
                }
                else 
                {
                    foreach (var op in fileOperations) Console.WriteLine($"{op} → Recycle Bin");
                    foreach (var op in dirOperations) 
                    {
                        if (Helpers.IsDirectoryEmpty(op)) Console.WriteLine($"Helpers.EnsureBackslash(op) → Recycle Bin");
                        else Console.WriteLine($"Helpers.EnsureBackslash(op)* → Recycle Bin");
                    }
                }
                Console.Write("Are you sure you want to remove the above items? (y/n): ");
                if (!Console.ReadLine().Trim().ToLower().Equals("y")) return;
            }
            foreach (var op in fileOperations) FileOperation(op, force);
            foreach (var op in dirOperations) DirectoryOperation(op, force);
        }

        private static void DirectoryOperation(string sourceDir, bool force)
        {
            if (force) Directory.Delete(sourceDir, true);
            else
            {
                var size = Helpers.GetDirectorySize(sourceDir);
                var option = size >= Helpers.fileManager.Settings.dialogueSize * 1024 * 1024 ? UIOption.AllDialogs : UIOption.OnlyErrorDialogs;
                FileSystem.DeleteDirectory(sourceDir, option, RecycleOption.SendToRecycleBin);
            }
        }

        private static void FileOperation(string sourceFile, bool force)
        {
            if (force) File.Delete(sourceFile);
            else 
            {
                var info = new FileInfo(sourceFile);
                var size = info.Length;
                var option = size >= Helpers.fileManager.Settings.dialogueSize * 1024 * 1024 ? UIOption.AllDialogs : UIOption.OnlyErrorDialogs;
                FileSystem.DeleteFile(sourceFile, option, RecycleOption.SendToRecycleBin);
            }
        }
    }
}
