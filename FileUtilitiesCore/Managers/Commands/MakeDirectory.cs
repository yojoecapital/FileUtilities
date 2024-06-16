using CliFramework;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class MakeDirectory
    {
        public static void Command(string[] args)
        {
            try
            {
                if (Arg.Parse(args.Skip(1), 1, out var mandatoryResults))
                {
                    Directory.CreateDirectory(mandatoryResults[0]);
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
