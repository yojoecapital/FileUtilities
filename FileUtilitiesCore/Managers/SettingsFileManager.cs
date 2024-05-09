using CliFramework;
using Newtonsoft.Json;

namespace ArabizeCore.Managers
{
    internal class Settings
    {
        public int resultsPerPage = 15;
    }

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
    }
}
