using CliFramework;
using FileUtilitiesCore.Managers.Commands;

namespace FileUtilitiesCore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Repl repl = new()
            {
                pagifyHelp = int.MaxValue
            };
            repl.AddCommand(
                args => args.Length == 2 && (args[0].Equals("help") || args[0].Equals("h")) && args[1].Equals("-j"),
                Other.Cd,
                "cd",
                "Prints the current directory."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("ls"),
                List.Command,
                "ls -r -i [include] -e [exclude]",
                "List segements of current directory.\nUse -r to display recursively.\nUse -i [include] to include glob.\nUse -e [exclude] to exclude glob."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("find"),
                Find.Command,
                "find [dir] -r -d -i [include] -e [exclude]",
                "List segements of [dir].\nUse -r to display recursively.\nUse -cd to display relative to the current directory.\nUse -i [include] to include glob.\nUse -e [exclude] to exclude glob."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("cp"),
                Copy.Command,
                "cp [src...] [dest] -r -o -y -i [include] -e [exclude]",
                "Copy from [src...] to [dest].\nUse * in [dest] as a macro for the file name of [src].\nUse -r to copy recursively.\nUse -o to overwrite existing items.\nUse -y to skip the prompt.\nUse -i [include] to include glob.\nUse -e [exclude] to exclude glob."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("mv"),
                Move.Command,
                "mv [src...] [dest] -r -o -y -i [include] -e [exclude]",
                "Move from [src...] to [dest].\nUse * in [dest] as a macro for the file name of [src].\nUse -r to move recursively.\nUse -o to overwrite existing items.\nUse -y to skip the prompt.\nUse -i [include] to include glob.\nUse -e [exclude] to exclude glob."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("rm"),
                Remove.Command,
                "rm [path...] -r -f -y -i [include] -e [exclude]",
                "Remove from [path...].\nUse -r to remove recursively.\nUse -f to bypass recycle bin.\nUse -y to skip the prompt.\nUse -i [include] to include glob.\nUse -e [exclude] to exclude glob."
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
                "Update the file at [file] if it exists.\nOtherwise, it creates it."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("info"),
                Other.Info,
                "info [path]",
                "Display the directory or file information at [path]."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("size"),
                Other.Size,
                "size [path]",
                "Display the directory or file size in bytes at [path]."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("exec"),
                Exec.Command,
                "exec [name] [args...]",
                "Executes a script item."
            );
            repl.AddCommand(
                args => args.Length > 2 && args[0].ToLower().Equals("make") && args[1].ToLower().Equals("script"),
                MakeScript.Command,
                "make script [name]",
                "Steps to make a new script item."
            );
            repl.AddCommand(
                args => args.Length > 2 && args[0].ToLower().Equals("remove") && args[1].ToLower().Equals("script"),
                RemoveScript.Command,
                "remove script [name]",
                "Removes a script item."
            );
            repl.AddCommand(
                args => (args.Length == 2 || args.Length == 3) && args[0].ToLower().Equals("dir") && args[1].ToLower().Equals("scripts"),
                Other.DirectoryScripts,
                "dir scripts -o",
                "Prints the script items directory.\nUse -p to open the directory."
            );
            repl.AddCommand(
                args => args.Length == 2 && args[0].ToLower().Equals("list") && args[1].ToLower().Equals("scripts"),
                Other.ListScripts,
                "list scripts",
                "Lists the available scripts."
            );
            repl.AddCommand(
                args => args.Length == 1 && (args[0].ToLower().Equals("open") || args[0].ToLower().Equals("o")),
                Other.OpenSettings,
                "open (o)",
                "Open the settings JSON."
            );
            repl.preprocessArg = Helpers.PreprocessEnvironmentVariables;
            repl.Process(args);
        }
    }
}