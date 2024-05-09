using CliFramework;
using FileUtilitiesCore.Managers.Commands;

namespace FileUtilitiesCore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Repl repl = new();
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("ls"),
                List.Command,
                "ls -r -i [include] -e [exclude]",
                "List segements of current directory. Use -r to display recursively. Use -i [include] to include glob. Use -e [exclude] to exclude glob."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("find"),
                Find.Command,
                "find [dir] -r -i [include] -e [exclude]",
                "List segements of [dir]. Use -r to display recursively. Use -i [include] to include glob. Use -e [exclude] to exclude glob."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("cp"),
                Copy.Command,
                "cp [src] [dest] -r -o -i [include] -e [exclude]",
                "Copy from [src] to [dest]. Use -r to copy recursively. Use -o to overwrite existing items. Use -i [include] to include glob. Use -e [exclude] to exclude glob."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("mv"),
                Move.Command,
                "mv [src] [dest] -r -o -i [include] -e [exclude]",
                "Move from [src] to [dest]. Use -r to move recursively. Use -o to overwrite existing items. Use -i [include] to include glob. Use -e [exclude] to exclude glob."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("rm"),
                Remove.Command,
                "rm [path] -r -i [include] -e [exclude]",
                "Remove from [path]. Use -r to remove recursively. Use -i [include] to include glob. Use -e [exclude] to exclude glob."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("nm"),
                Rename.Command,
                "nm [path] [name]",
                "Rename item at [path] to [name]."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("mkdir"),
                MakeDirectory.Command,
                "mkdir [dir]",
                "Make a new directory at [dir]."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("touch"),
                Touch.Command,
                "touch [file]",
                "Update the file at [file] if it exists. Otherwise, it creates it."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("info"),
                Helpers.Info,
                "info [file]",
                "Display the file information at [file]."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("size"),
                Helpers.Size,
                "size [file]",
                "Display the file size in bytes at [file]."
            );
            repl.AddCommand(
                args => args.Length == 1 && (args[0].ToLower().Equals("open") || args[0].ToLower().Equals("o")),
                Helpers.OpenSettings,
                "open (o)",
                "Open the settings JSON."
            );
            repl.preprocessArg = Helpers.PreprocessEnvironmentVariables;
            repl.Process(args);
        }
    }
}