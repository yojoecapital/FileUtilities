using CliFramework;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class RemoveScript
    {
        public static void Command(string[] args)
        {
            try
            {
                if (Arg.Parse(args.Skip(2), 1, out var mandatoryResults))
                {
                    Run(args[0]);
                }
                else PrettyConsole.PrintError("Invalid arguments.");
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError(ex.Message);
            }
        }

        public static void Run(string script)
        {
            var item = Helpers.fileManager.GetScriptItem(script);
            if (item == null) 
            {
                PrettyConsole.PrintError("Could not parse script item JSON file.");
                return;
            }
            Console.Write($"Are you sure you want to remove the script '{script}'? (y/n): ");
            if (!Console.ReadLine().Trim().ToLower().Equals("y")) return;
            Helpers.fileManager.DeleteScriptItem(script, item);
        }
    }
}
