namespace FileUtilitiesCore.Utilities
{
    internal class Settings
    {
        public int resultsPerPage = 15;
        public string scriptsPath = "scripts";
        public string endBlock = "</bat>";
        public Dictionary<string, ExecutionMethod> methods = new();
    }

    internal class ExecutionMethod
    {
        public string path = string.Empty;
        public string[] setup  = Array.Empty<string>();
        public string extension  = string.Empty;
    }
}
