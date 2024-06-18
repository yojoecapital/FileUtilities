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
                args => args.Length == 1 && args[0].ToLower().Equals("cd"),
                Other.Cd,
                "cd",
                "Prints the current directory."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("ls"),
                List.Command,
                "ls -r -i [include] -e [exclude]",
                "List segements in current directory.\nUse -r to display recursively.\nUse -i [include] to include glob.\nUse -e [exclude] to exclude glob."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("find"),
                Find.Command,
                "find [dirs...] -r -cd -i [include] -e [exclude]",
                "List segements in [dirs...].\nUse -r to display recursively.\nUse -cd to display relative to the current directory.\nUse -i [include] to include glob.\nUse -e [exclude] to exclude glob."
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
                "rm [paths...] -r -f -y -i [include] -e [exclude]",
                "Remove from [paths...].\nUse -r to remove recursively.\nUse -f to bypass recycle bin.\nUse -y to skip the prompt.\nUse -i [include] to include glob.\nUse -e [exclude] to exclude glob."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("mkdir"),
                MakeDirectory.Command,
                "mkdir [dirs...]",
                "Make new directories at [dirs...]."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("touch"),
                Touch.Command,
                "touch [files...]",
                "Update the files at [files...] if it exists. Otherwise, it creates it."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("info"),
                Info.Command,
                "info [paths...]",
                "Display the directory or file information at [paths...]."
            );
            repl.AddCommand(
                args => args.Length > 0 && args[0].ToLower().Equals("exec"),
                Exec.Command,
                "exec [name] [args...]",
                "Executes a script item."
            );
            repl.AddCommand(
                args => args.Length == 3 && args[0].ToLower().Equals("make") && args[1].ToLower().Equals("script"),
                MakeScript.Command,
                "make script [name]",
                "Steps to make a new script item."
            );
            repl.AddCommand(
                args => args.Length == 3 && args[0].ToLower().Equals("remove") && args[1].ToLower().Equals("script"),
                RemoveScript.Command,
                "remove script [name]",
                "Removes a script item."
            );
            repl.AddCommand(
                args => args.Length >= 2 && args[0].ToLower().Equals("dir") && args[1].ToLower().Equals("scripts"),
                Other.DirectoryScripts,
                "dir scripts -o",
                "Prints the script items directory.\nUse -o to open the directory."
            );
            repl.AddCommand(
                args => args.Length == 2 && args[0].ToLower().Equals("list") && args[1].ToLower().Equals("scripts"),
                Other.ListScripts,
                "list scripts",
                "Lists the available scripts."
            );
            repl.AddCommand(
                args => args.Length == 3 && args[0].ToLower().Equals("open") && args[1].ToLower().Equals("script"),
                Other.OpenScript,
                "open script [name]",
                "Opens the file for a script item."
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