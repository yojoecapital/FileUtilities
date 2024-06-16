using CliFramework;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class MakeDirectory
    {
        public static void Command(string[] args)
        {
            try
            {
                if (Arg.Parse(args.Skip(1), 0, true, out var _, out var spreadResults))
                {
                    foreach (var path in spreadResults) Directory.CreateDirectory(path);
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
