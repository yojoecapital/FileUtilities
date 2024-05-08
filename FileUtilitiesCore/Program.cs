using CliFramework;
using fs.Manager;

namespace fs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Repl repl = new();
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("ls"),
                FileUtilitiesCommandManager.Ls,
                "ls [path]",
                "List segments of [path] or current directory."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("cp"),
                FileUtilitiesCommandManager.Cp,
                "cp [source] [destination]",
                "Copy from [source] to [destination]."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("mv"),
                FileUtilitiesCommandManager.Mv,
                "mv [source] [destination]",
                "Move from [source] to [destination]."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("rm"),
                FileUtilitiesCommandManager.Rm,
                "rm [path]",
                "Remove [path]."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("info"),
                FileUtilitiesCommandManager.Info,
                "info [path]",
                "Prints the info of [path]."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("size"),
                FileUtilitiesCommandManager.Size,
                "size [path]",
                "Prints the size of [path]."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("find"),
                FileUtilitiesCommandManager.Find,
                "find [pattern] [path]",
                "Finds all segments matching [pattern] in [path] or current directory."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("mkdir"),
                FileUtilitiesCommandManager.Mkdir,
                "mkdir [path]",
                "Make a new directory [path]."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("rmdir"),
                FileUtilitiesCommandManager.Rmdir,
                "rmdir [path]",
                "Remove an empty directory [path]."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("touch"),
                FileUtilitiesCommandManager.Touch,
                "touch [path]",
                "Touches [path] if exists or makes a new file there."
            );
            repl.preprocessArg = FileUtilitiesCommandManager.PreprocessEnvironmentVariables;
            repl.Process(args);
        }
    }
}