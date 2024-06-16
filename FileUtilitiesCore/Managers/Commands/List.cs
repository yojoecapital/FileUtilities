using CliFramework;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class List
    {
        public static void Command(string[] args)
        {
            try
            {
                if (Arg.Parse(args.Skip(1), 0, new [] { "-r" }, new [] { "-i", "-e" }, out var mandatoryResults, out var flagResults, out var stringResults))
                {
                    Find.Run(".", stringResults["-i"], stringResults["-e"], flagResults["-r"], true);
                }
                else PrettyConsole.PrintError("Invalid arguments.");
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError(ex.Message);
            }
        }
    }
}
