namespace FileUtilitiesCore.Utilities
{
    internal class Settings
    {
        public string scriptsPath = "scripts";
        public string endBlock = "</done>";
        public long dialogueSize = 100;
        public Dictionary<string, ExecutionMethod> methods = new();
    }

    internal class ExecutionMethod
    {
        public string path = string.Empty;
        public string[] setup  = Array.Empty<string>();
        public string extension  = string.Empty;
    }
}
