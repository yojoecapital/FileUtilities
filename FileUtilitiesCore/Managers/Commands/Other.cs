using CliFramework;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.VisualBasic.FileIO;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class Other
    {
        public static void Info(string[] args)
        {
            if (args.Length != 2)
            {
                PrettyConsole.PrintError("Invalid arguments.");
                return;
            }
            
            var path = args[1];
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                PrettyConsole.PrintError($"'{path}' does not exist.");
                return;
            }
            
            try
            {
                var attributes = File.GetAttributes(path);
                var isDirectory = (attributes & FileAttributes.Directory) == FileAttributes.Directory;

                string json;
                if (isDirectory)
                {
                    var files = Directory.GetFiles(path).Length;
                    var subdirectories = Directory.GetDirectories(path).Length;
                    json = JsonConvert.SerializeObject(new
                    {
                        Type = "Directory",
                        Path = Path.GetFullPath(path),
                        Created = Directory.GetCreationTime(path),
                        LastAccessTime = Directory.GetLastAccessTime(path),
                        LastWriteTime = Directory.GetLastWriteTime(path),
                        Items = files + subdirectories,
                        Files = files,
                        Subdirectories = subdirectories
                    }, Formatting.Indented);
                }
                else
                {
                    json = JsonConvert.SerializeObject(new
                    {
                        Type = "File",
                        Path = Path.GetFullPath(path),
                        Created = File.GetCreationTime(path),
                        LastAccessTime = File.GetLastAccessTime(path),
                        LastWriteTime = File.GetLastWriteTime(path)
                    }, Formatting.Indented);
                }

                PrettyConsole.PrintColor(json, ConsoleColor.Yellow);
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError(ex.Message);
            }
        }


        public static void Size(string[] args)
        {
            if (!File.Exists(args[1]) && !Directory.Exists(args[1]))
            {
                PrettyConsole.PrintError($"'{args[1]}' does not exist.");
                return;
            }

            try
            {
                long size;

                if (File.Exists(args[1]))
                {
                    // If it's a file, get the file size
                    var info = new FileInfo(args[1]);
                    size = info.Length;
                }
                else
                {
                    // If it's a directory, get the directory size
                    size = Helpers.GetDirectorySize(args[1]);
                }

                Console.WriteLine(size);
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError(ex.Message);
            }
        }
        
        public static void Cd(string[] _) => Console.WriteLine(Directory.GetCurrentDirectory());

        public static void OpenSettings(string[] _)
        {
            var path = SettingsFileManager.SettingsFilePath;
            ProcessStartInfo psi = new()
            {
                FileName = path,
                UseShellExecute = true
            };
            Process.Start(psi);
            Console.WriteLine(path);
        }
        
        public static void DirectoryScripts(string[] args)
        {
            bool proc = false;
            if (args.Length == 3)
            {
                if (args[2].Trim().ToLower().Equals("-o"))
                    proc = true;
                else
                {
                    PrettyConsole.PrintError("Invalid arguments.");
                    return;
                }
            }
            var path = Helpers.fileManager.ScriptsFilePath;
            if (proc && Directory.Exists(path))
            {
                ProcessStartInfo psi = new()
                {
                    FileName = path,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            Console.WriteLine(Path.GetFullPath(path));
        }


        public static void ListScripts(string[] _)
        {
            var path = Helpers.fileManager.ScriptsFilePath;
            if (Directory.Exists(path))
            {
                PrettyConsole.PrintList(Directory.GetFiles(path, "*.json").Select(file => Path.GetRelativePath(path, file)[..^5]));
            }
        }

    }
}
