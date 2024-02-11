namespace ConfigEditor.Domain
{
    public class Server
    {
        public string Name { get; set; }

        public Dictionary<string, string> Overrides { get; set; }
    }
}
