using CliFramework;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class RemoveScript
    {
        public static void Command(string[] args)
        {
            if (args.Length != 3)
            {
                PrettyConsole.PrintError($"Invalid arguments.");
            }
            else Run(args[2]);
        }

        private static void Run(string script)
        {
            try
            {
                var item = Helpers.fileManager.GetScriptItem(script);
                if (item == null) 
                {
                    PrettyConsole.PrintError("Could not parse script item JSON file.");
                    return;
                }
                Console.Write($"Are you sure you want to remove the script \"{script}\"? (y/n): ");
                if (!Console.ReadLine().Trim().ToLower().Equals("y")) return;
                Helpers.fileManager.DeleteScriptItem(script, item);
            }
            catch (Exception ex) 
            {
                PrettyConsole.PrintError($"Could not remove script.\n{ex.Message}");
            }
        }
    }
}
