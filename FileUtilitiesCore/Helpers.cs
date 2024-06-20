using CliFramework;
using Microsoft.Extensions.FileSystemGlobbing;

namespace FileUtilitiesCore.Managers.Commands
{
    internal static class Helpers
    {
        public static readonly SettingsFileManager fileManager = new SettingsFileManager();

        public static long GetDirectorySize(string path)
        {
            long size = 0;

            // Get all files in the directory and add their sizes
            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                size += fileInfo.Length;
            }

            return size;
        }

        public static bool IsDirectoryEmpty(string path) => !Directory.EnumerateFileSystemEntries(path).Any();

        public static string PreprocessEnvironmentVariables(string input)
        {
            // replace "~" with the USERPROFILE environment variable
            string userprofile = Environment.GetEnvironmentVariable("USERPROFILE");
            if (!string.IsNullOrEmpty(userprofile))
            {
                input = ReplaceInPath(input, "~", userprofile);
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

        public static string GetNormalizedPath(string path) => Path.GetRelativePath(Directory.GetCurrentDirectory(), Path.GetFullPath(path));

        public static string EnsureBackslash(string path)
        {
            if (!path.EndsWith("\\") && !path.EndsWith("/")) path += "\\";
            return path;
        }

        public static string ProcessDestination(bool wildFlag, string source, string dest) 
        {
            if (wildFlag) 
            {

                var replacement = Path.GetFileName(Path.GetFullPath(source.TrimEnd('\\')));
                return ReplaceInPath(dest, "*", replacement);
            }
            return dest;
        }

        public static IEnumerable<string> Filter(IEnumerable<string> paths,  string include, string exclude)
        {
            var matcher = new Matcher();
            if (!string.IsNullOrEmpty(include)) matcher.AddInclude(include);
            else matcher.AddInclude("**");
            if (!string.IsNullOrEmpty(exclude)) matcher.AddExclude(exclude);
            return matcher.Match(paths).Files.Select(file => file.Path);
        }

        public static void PerformOnItems(string[] sources, string include, string exclude, bool recurse, Action<string> onFile, Action<string> onDir, Action<string, string> onPath)
        {
            if (!string.IsNullOrWhiteSpace(include) || !string.IsNullOrWhiteSpace(exclude))
            {
                var matcher = new Matcher();
                if (!string.IsNullOrWhiteSpace(include)) matcher.AddInclude(include);
                else matcher.AddInclude("**");
                if (!string.IsNullOrWhiteSpace(exclude)) matcher.AddExclude(exclude);
                var searchOption = recurse ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly;
                foreach (var source in sources)
                {
                    if (File.Exists(source)) onFile(GetNormalizedPath(source));
                    else if (Directory.Exists(source))
                    {
                        var fullSource = Path.GetFullPath(source);
                        var files = Directory.GetFiles(source, "*", searchOption).Select(file => Path.GetRelativePath(fullSource, file));
                        var glob = matcher.Match(files).Files.Select(file => file.Path);
                        foreach(var file in glob) onPath(source, GetNormalizedPath(Path.Join(source, file)));
                    }
                    else
                    {
                        throw new Exception($"'{source}' does not exist.");
                    }
                }
            }
            else 
            {
                foreach (var source in sources)
                {
                    if (File.Exists(source)) onFile(GetNormalizedPath(source));
                    else if (Directory.Exists(source)) onDir(GetNormalizedPath(source));
                    else
                    {
                        throw new Exception($"'{source}' does not exist.");
                    }
                }
            }
        }

        public static string ReplaceInPath(string path, string searchString, string replacementString)
        {
            // Split the path into its components
            string[] pathComponents = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            // Replace components that match the search string with the replacement string
            string[] updatedComponents = pathComponents.Select(component => 
                component.Equals(searchString, StringComparison.OrdinalIgnoreCase) ? replacementString : component).ToArray();

            // Join the components back into a single path
            string updatedPath = string.Join(Path.DirectorySeparatorChar.ToString(), updatedComponents);

            return updatedPath;
        }
    }
}
