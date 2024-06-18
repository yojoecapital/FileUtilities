using System.Runtime.InteropServices;
using System.Text;
using CliFramework;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class Remove
    {
        public static void Command(string[] args)
        {
            try
            {
                if (Arg.Parse(args.Skip(1), 0, true, new [] { "-r", "-f", "-y" }, new [] { "-i", "-e" }, out var _, out var spreadResults, out var flagResults, out var stringResults))
                {
                    fileOperations = new List<string>();
                    dirOperations = new List<string>();
                    Helpers.PerformOnItems(
                        spreadResults, stringResults["-i"], stringResults["-e"], flagResults["-r"],
                        OnFile,
                        OnDir,
                        OnPath
                    );
                    Run(flagResults["-f"], flagResults["-y"]);
                }
                else PrettyConsole.PrintError("Invalid arguments.");
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError(ex.Message);
            }
        }

        private static List<string> fileOperations;
        private static List<string> dirOperations;

        private static void OnFile(string source) => fileOperations.Add(source);
        private static void OnDir(string source) => dirOperations.Add(source);
        private static void OnPath(string _, string source) => fileOperations.Add(source);

        public static void Run(bool force, bool yes)
        {
            if (!yes && (fileOperations.Count > 0 || dirOperations.Count > 0))
            {
                if (force)
                {
                    foreach (var op in fileOperations) Console.WriteLine($"{op}");
                    foreach (var op in dirOperations) 
                    {
                        if (Helpers.IsDirectoryEmpty(op)) Console.WriteLine($"{Helpers.EnsureBackslash(op)}");
                        else Console.WriteLine($"{Helpers.EnsureBackslash(op)}*");
                    }
                }
                else 
                {
                    foreach (var op in fileOperations) Console.WriteLine($"{op} → Recycle Bin");
                    foreach (var op in dirOperations) 
                    {
                        if (Helpers.IsDirectoryEmpty(op)) Console.WriteLine($"{Helpers.EnsureBackslash(op)} → Recycle Bin");
                        else Console.WriteLine($"{Helpers.EnsureBackslash(op)}* → Recycle Bin");
                    }
                }
                Console.Write("Are you sure you want to remove the above items? (y/n): ");
                if (!Console.ReadLine().Trim().ToLower().Equals("y")) return;
            }
            DirectoryOperation(dirOperations, force);
            FileOperation(fileOperations, force);   
        }

        private static void DirectoryOperation(IEnumerable<string> dirs, bool force)
        {
            
            if (force) 
            {
                foreach (var dir in dirs) Directory.Delete(dir);
            }
            else Delete(dirs);
        }

        private static void FileOperation(IEnumerable<string> files, bool force)
        {
            if (force) 
            {
                foreach (var file in files) File.Delete(file);
            }
            else Delete(files);
        }

        public static void Delete(IEnumerable<string> paths)
        {
            // Convert the list of paths into a null-terminated string
            StringBuilder pathsBuilder = new StringBuilder();
            foreach (string path in paths)
            {
                pathsBuilder.Append(path).Append('\0');
            }
            pathsBuilder.Append('\0'); // Double null-terminate the string

            // Set up the SHFILEOPSTRUCT structure
            SHFILEOPSTRUCT fileOp = new SHFILEOPSTRUCT
            {
                wFunc = FO_Func.FO_DELETE,
                pFrom = pathsBuilder.ToString(),
                fFlags = FILEOP_FLAGS.FOF_ALLOWUNDO // | FILEOP_FLAGS.FOF_NOCONFIRMATION
            };

            // Perform the file operation
            int result = SHFileOperation(ref fileOp);

            // Check the result
            if (result != 0)
            {
                throw new Exception("Error in deletion.");
            }
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            public FO_Func wFunc;
            public string pFrom;
            public string pTo;
            public FILEOP_FLAGS fFlags;
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }

        private enum FO_Func : uint
        {
            FO_MOVE = 0x0001,
            FO_COPY = 0x0002,
            FO_DELETE = 0x0003,
            FO_RENAME = 0x0004
        }

        [Flags]
        private enum FILEOP_FLAGS : ushort
        {
            FOF_MULTIDESTFILES = 0x0001,
            FOF_CONFIRMMOUSE = 0x0002,
            FOF_SILENT = 0x0004,
            FOF_RENAMEONCOLLISION = 0x0008,
            FOF_NOCONFIRMATION = 0x0010, // Don't prompt the user.
            FOF_WANTMAPPINGHANDLE = 0x0020,
            FOF_ALLOWUNDO = 0x0040, // Allow undo
            FOF_FILESONLY = 0x0080,
            FOF_SIMPLEPROGRESS = 0x0100,
            FOF_NOCONFIRMMKDIR = 0x0200,
            FOF_NOERRORUI = 0x0400,
            FOF_NOCOPYSECURITYATTRIBS = 0x0800,
            FOF_NORECURSION = 0x1000,
            FOF_NO_CONNECTED_ELEMENTS = 0x2000,
            FOF_WANTNUKEWARNING = 0x4000,
            FOF_NORECURSEREPARSE = 0x8000
        }
    }
}
