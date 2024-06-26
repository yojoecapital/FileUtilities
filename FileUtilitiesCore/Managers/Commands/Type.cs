using System.Security.Cryptography;
using System.Text;
using CliFramework;

namespace FileUtilitiesCore.Managers.Commands 
{
    public static class Type 
    {
        public static void Command(string[] args)
        {
            try
            {
                if (Arg.Parse(args.Skip(1), 1, Array.Empty<string>(), new[] { "-en" }, out var mandatoryResults, out var _, out var stringResults))
                {
                    Run(mandatoryResults[0], stringResults["-en"]);
                }
                else PrettyConsole.PrintError("Invalid arguments.");
            }
            catch (Exception ex)
            {
                PrettyConsole.PrintError(ex.Message);
            }
        }

        public static void Run(string path, string en)
        {
            if (!File.Exists(path))
            {
                throw new Exception($"File '{path}' does not exist.");
            }
            
            Encoding encoding = Encoding.Default;
            if (!string.IsNullOrWhiteSpace(en))
            {
                en = en.ToUpper();
                if (en.Equals("UTF16-LE") || en.Equals("UTF16")) encoding = Encoding.Unicode;
                else if (en.Equals("UTF16-BE")) encoding = Encoding.BigEndianUnicode;
                else if (en.Equals("UTF8")) encoding = Encoding.UTF8;
                else if (en.Equals("UTF32")) encoding = Encoding.UTF32;
                else throw new Exception($"Unknown encoding '{en}'.");
            }
            else 
            {
                var buffer = new byte[4];
                
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    if (fs.Length < buffer.Length)
                    {
                        buffer = new byte[fs.Length];
                    }

                    fs.Read(buffer, 0, buffer.Length);
                }

                // Check for BOM and return the corresponding encoding
                if (buffer.Length >= 2)
                {
                    // UTF-16 (little-endian)
                    if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                    {
                        encoding = Encoding.Unicode;
                    }
                    // UTF-16 (big-endian)
                    else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                    {
                    encoding = Encoding.BigEndianUnicode;
                    }
                    // UTF-8 with BOM
                    else if (buffer.Length >= 3 && buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)
                    {
                        encoding = Encoding.UTF8;
                    }
                    // UTF-32 (little-endian)
                    else if (buffer.Length >= 4 && buffer[0] == 0xFF && buffer[1] == 0xFE && buffer[2] == 0x00 && buffer[3] == 0x00)
                    {
                        encoding = Encoding.UTF32;
                    }
                }
            }

            var content = File.ReadAllText(path, encoding);
            Console.Write(content);
        }
    }
}