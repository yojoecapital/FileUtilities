using CliFramework;
using FileUtilitiesCore.Utilities;
using Newtonsoft.Json;

namespace ArabizeCore.Managers
{
    internal class SettingsFileManager : FileManager
    {
        public static string SettingsFilePath
        {
            get => GetDictionaryFilePath("settings.json");
        }

        private Settings settings;
        public Settings Settings
        {
            get
            {
                settings ??= GetObject<Settings>(SettingsFilePath);
                if (settings == null)
                {
                    PrettyConsole.PrintError("Could not parse settings JSON file.");
                    return null;
                }
                else return settings;
            }
            set
            {
                settings = value;
                SetObject(SettingsFilePath, settings, Formatting.Indented);
            }
        }

        public string ScriptsFilePath
        {
            get => Settings?.scriptsPath ?? "scripts";
        }

        public ScriptItem GetScriptItem(string name)
        {
            var path = Path.Combine(ScriptsFilePath, name + ".json");
            if (File.Exists(path)) return GetObject<ScriptItem>(path);
            return null;
        }
        
        public void SaveScriptItem(string name, ScriptItem item, string script)
        {
            Directory.CreateDirectory(ScriptsFilePath);
            var itemPath = Path.Combine(ScriptsFilePath, name + ".json");
            var scriptPath = Path.Combine(ScriptsFilePath, name + "." + Settings.methods[item.exe].extension);
            SetObject(itemPath, item);
            File.WriteAllText(scriptPath, script);
        }
        
        public void DeleteScriptItem(string name, ScriptItem item)
        {
            var itemPath = Path.Combine(ScriptsFilePath, name + ".json");
            var scriptPath = Path.Combine(ScriptsFilePath, name + "." + Settings.methods[item.exe].extension);
            if (File.Exists(itemPath)) File.Delete(itemPath);
            if (File.Exists(scriptPath)) File.Delete(scriptPath);
        }
    }
}
