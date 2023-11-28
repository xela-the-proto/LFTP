namespace FTP_console.Config
{
    internal class FTP_Json
    {
        public string host { get; set; }
        public int port { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public List<string> logon_type { get; set; }
    }
}