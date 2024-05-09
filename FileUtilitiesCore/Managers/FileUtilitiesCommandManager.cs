using CliFramework;
using Newtonsoft.Json;
using System.Security.AccessControl;
using System.Security.Principal;
using Microsoft.VisualBasic.FileIO;
using SearchOption = System.IO.SearchOption;
using ArabizeCore.Managers;

namespace fs.Manager
{
    internal static class FileUtilitiesCommandManager
    {
        private static readonly int resultsPerPage = new SettingsFileManager().Settings.resultsPerPage;

        public static void Ls(string[] args)
        {
            if (args.Length > 2)
            {
                PrettyConsole.PrintError($"Unexpected argument \"{args[1]}\".");
                return;
            }
            string dir;
            if (args.Length == 1) dir = Directory.GetCurrentDirectory();
            else if (!IsValid(args[1])) return;
            else dir = args[1];
            var items = Directory.GetDirectories(dir).Select(dir => dir + "\\").Union(Directory.GetFiles(dir));
            if (items.Count() < resultsPerPage) PrettyConsole.PrintList(items);
            else PrettyConsole.PrintList(items, resultsPerPage);
        }

        public static void Cp(string[] args)
        {
            if (args.Length != 3)
            {
                PrettyConsole.PrintError("Expected 2 arguments.");
                return;
            }
            if (!IsValid(args[1]) || !IsValid(args[1], true)) return;
            try
            {
                var sourcePath = args[1];
                var destinationPath = args[2];
                if (Directory.Exists(sourcePath))
                {
                    Directory.CreateDirectory(destinationPath);
                    foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                        Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));

                    foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                        File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
                }
                else if (File.Exists(sourcePath))
                {
                    File.Copy(sourcePath, destinationPath, true);
                }
                Console.WriteLine("Copied successfully.");
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError($"Could not copy.\n{ex}");
            }
        }

        public static void Mv(string[] args)
        {
            if (args.Length != 3)
            {
                PrettyConsole.PrintError("Expected 2 arguments.");
                return;
            }
            if (!IsValid(args[1]) || !IsValid(args[2], true)) return;
            Console.Write($"Are you sure you want to move \"{args[1]}\" to \"{args[2]}\"? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                var sourcePath = args[1];
                var destinationPath = args[2];
                try
                {
                    if (Directory.Exists(sourcePath))
                    {
                        Directory.Move(sourcePath, destinationPath);
                    }
                    else if (File.Exists(sourcePath))
                    {
                        File.Move(sourcePath, destinationPath);
                    }
                    Console.WriteLine("Moved successfully.");
                }
                catch (Exception ex)
                {
                    PrettyConsole.PrintError($"Could not move.\n{ex}");
                }
            }
        }

        public static void Rm(string[] args)
        {
            if (args.Length > 3)
            {
                PrettyConsole.PrintError($"Unexpected argument \"{args[3]}\".");
                return;
            }
            var flags = GetFlags(args, 2, new string[] { "-r" });
            if (!IsValid(args[1], true)) return;
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), args[1], flags["-r"] ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            PrettyConsole.PrintList(files);
            Console.Write($"Are you sure you want to remove the files list above? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                foreach (var file in files) 
                {
                    try
                    {
                        if (Directory.Exists(file))
                        {
                            FileSystem.DeleteDirectory(file, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                        }
                        else if (File.Exists(file))
                        {
                            FileSystem.DeleteFile(file, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                        }
                        Console.WriteLine("Removed successfully.");
                    }
                    catch (Exception ex)
                    {
                        PrettyConsole.PrintError($"Could not remove {file} .\n{ex}");
                    }
                }
            }
        }

        public static void Info(string[] args)
        {
            if (args.Length != 2)
            {
                PrettyConsole.PrintError("Expected 1 argument.");
                return;
            }
            if (!IsValid(args[1])) return;
            var attributes = File.GetAttributes(args[1]);
            var json = JsonConvert.SerializeObject(new {
                Attributes = attributes.ToString(),
                Created = Directory.GetCreationTime(args[1]),
                LastAccessTime = Directory.GetLastAccessTime(args[1]),
                LastWriteTime = Directory.GetLastWriteTime(args[1])
            }, Formatting.Indented);
            PrettyConsole.PrintColor(json, ConsoleColor.Yellow);
        }

        public static void Size(string[] args)
        {
            if (args.Length != 2)
            {
                PrettyConsole.PrintError("Expected 1 argument.");
                return;
            }
            if (!IsValid(args[1])) return;
            var info = new FileInfo(args[1]);
            Console.WriteLine(info.Length);
        }

        public static void Find(string[] args)
        {
            if (args.Length == 1) PrettyConsole.PrintError("Expected at least 1 argument.");
            else if (args.Length > 3) PrettyConsole.PrintError($"Unexpected argument \"{args[3]}\".");
            else
            {
                var flags = GetFlags(args, 2, new string[] { "-r" });
                var files = Directory.GetFiles(Directory.GetCurrentDirectory(), args[1], flags["-r"] ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                if (files.Length < resultsPerPage) PrettyConsole.PrintList(files);
                else PrettyConsole.PrintList(files, resultsPerPage);
            }
        }

        public static void Mkdir(string[] args)
        {
            if (args.Length != 2)
            {
                PrettyConsole.PrintError("Expected 1 argument.");
                return;
            }
            try
            {
                if (!Directory.Exists(args[1]))
                {
                    Directory.CreateDirectory(args[1]);
                    Console.WriteLine($"Directory \"{args[1]}\" created successfully.");
                }
                else
                {
                    PrettyConsole.PrintError("Directory already exists.");
                }
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError($"Could not make directory.\n{ex}");
            }
        }

        public static void Rmdir(string[] args)
        {
            if (args.Length != 2)
            {
                PrettyConsole.PrintError("Expected 1 argument.");
                return;
            }
            if (!IsValid(args[1])) return;
            Console.Write($"Are you sure you want to remove \"{args[1]}\"? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                try
                {
                    FileSystem.DeleteDirectory(args[1], UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    Console.WriteLine($"Directory '{args[1]}' removed successfully.");
                }
                catch (Exception ex)
                {
                    PrettyConsole.PrintError($"Could not remove directory.\n{ex}");
                }
            }
        }

        public static void Touch(string[] args)
        {
            if (args.Length != 2)
            {
                PrettyConsole.PrintError("Expected 1 argument.");
                return;
            }
            if (!IsValid(args[1], true)) return;
            try
            {
                if (File.Exists(args[1]))
                {
                    File.SetLastWriteTime(args[1], DateTime.Now);
                    File.SetLastAccessTime(args[1], DateTime.Now);
                    Console.WriteLine($"Updated timestamps for file \"{args[1]}\".");
                }
                else
                {
                    File.Create(args[1]).Dispose();
                    Console.WriteLine($"Created empty file \"{args[1]}\".");
                }
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError($"Could not touch.\n{ex}");
            }
        }

        public static string PreprocessEnvironmentVariables(string input)
        {
            // replace "~" with the USERPROFILE environment variable
            string userprofile = Environment.GetEnvironmentVariable("USERPROFILE");
            if (!string.IsNullOrEmpty(userprofile))
            {
                input = input.Replace("~", userprofile);
            }

            // find and replace all %<var>% patterns with their environment variable values
            int start = 0;
            while ((start = input.IndexOf('%', start)) != -1)
            {
                int end = input.IndexOf('%', start + 1);
                if (end == -1) break;

                string envVarName = input.Substring(start + 1, end - start - 1);
                string envVarValue = Environment.GetEnvironmentVariable(envVarName);

                if (envVarValue != null)
                {
                    input = string.Concat(input.AsSpan()[..start], envVarValue, input.AsSpan(end + 1));
                    start += envVarValue.Length;
                }
                else
                {
                    start = end + 1;
                }
            }

            return input;
        }

        private static Dictionary<string, bool> GetFlags(IEnumerable<string> args, int skip, IEnumerable<string> flags)
        {
            Dictionary<string, bool> flagResults = new();
            var checking = args.Skip(skip);
            foreach (var flag in flags)
            {
                bool value = false;
                if (checking.Contains(flag)) value = true;
                flagResults[flag] = value;
            }
            return flagResults;
        }

        private static bool IsValid(string path, bool getDirectoryName = false)
        {
            try
            {
                if (getDirectoryName)
                {
                    path = Path.GetDirectoryName(path);
                    if (path.Length == 0) return true;
                }
                if (!Directory.Exists(path) && !File.Exists(path))
                {
                    PrettyConsole.PrintError($"Could not find path \"{path}\".");
                    return false;
                }


                // check if we have write permission on the directory.
                var writeAllow = false;
                var writeDeny = false;
                var accessControlList = FileSystemAclExtensions.GetAccessControl(new DirectoryInfo(path));
                var accessRules = accessControlList.GetAccessRules(true, true, typeof(SecurityIdentifier));

                foreach (FileSystemAccessRule rule in accessRules)
                {
                    if ((FileSystemRights.Write & rule.FileSystemRights) != FileSystemRights.Write) continue;

                    if (rule.AccessControlType == AccessControlType.Allow)
                        writeAllow = true;
                    else if (rule.AccessControlType == AccessControlType.Deny)
                        writeDeny = true;
                }
                if (writeAllow && !writeDeny) return true;
                else
                {
                    PrettyConsole.PrintError("Permission is denied.");
                    return false;
                }
            }
            catch
            {
                PrettyConsole.PrintError("Permission is denied.");
                return false;
            }
        }
    }
}
