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
                "List segements. Use -r to display recursively. Use -i [include] to include glob. Use -e [exclude] to exclude glob."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("cp"),
                Copy.Command,
                "cp [source] [destination] -p -r",
                "Copy from [source] to [destination]. Use -p for patterns. Use -r to search recursively."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("mv"),
                Move.Command,
                "mv [source] [destination] -p -r -o",
                "Move from [source] to [destination]. Use -p for patterns. Use -r to search recursively. Use -o to overwrite existing files."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("rm"),
                Remove.Command,
                "rm [path] -p -r",
                "Remove [path]. Use -p for patterns. Use -r to search recursively."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("nm"),
                Rename.Command,
                "nm [path] [name]",
                "Rename [path] to [name]."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("mkdir"),
                MakeDirectory.Command,
                "mkdir [path]",
                "Make a new directory at [path]."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("touch"),
                Touch.Command,
                "touch [path]",
                "Update the file at [path] if it exists. Otherwise, it creates it."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("info"),
                Helpers.Info,
                "info [path]",
                "Display the file information at [path]."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("size"),
                Helpers.Size,
                "size [path]",
                "Display the file size in bytes at [path]."
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