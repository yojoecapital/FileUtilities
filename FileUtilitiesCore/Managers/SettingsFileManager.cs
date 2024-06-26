﻿using System.Diagnostics;
using CliFramework;
using FileUtilitiesCore.Managers.Commands;
using FileUtilitiesCore.Utilities;
using Newtonsoft.Json;

namespace FileUtilitiesCore.Managers
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
            get => Helpers.PreprocessEnvironmentVariables(Settings?.scriptsPath ?? Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "scripts"));
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
            SetObject(itemPath, item, Formatting.Indented);
            File.WriteAllText(scriptPath, script);
        }
        
        public void DeleteScriptItem(string name, ScriptItem item)
        {
            var itemPath = Path.Combine(ScriptsFilePath, name + ".json");
            var scriptPath = Path.Combine(ScriptsFilePath, name + "." + Settings.methods[item.exe].extension);
            if (File.Exists(itemPath)) File.Delete(itemPath);
            if (File.Exists(scriptPath)) File.Delete(scriptPath);
        }

        public void OpenScriptItem(string name, ScriptItem item)
        {
            var itemPath = Path.Combine(ScriptsFilePath, name + ".json");
            var scriptPath = Path.Combine(ScriptsFilePath, name + "." + Settings.methods[item.exe].extension);
            ProcessStartInfo psi = new()
            {
                FileName = scriptPath,
                UseShellExecute = true
            };
            Process.Start(psi);
            Console.WriteLine(scriptPath);
        }
    }
}
