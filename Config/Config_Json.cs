namespace FTP_console.Config
{
    internal class Config_Json
    {
        public Config_Json() { 
            verbose = false;
            color = ConsoleColor.White;
        }
        public bool verbose { get; set; }
        public ConsoleColor color { get; set; }
    }
}