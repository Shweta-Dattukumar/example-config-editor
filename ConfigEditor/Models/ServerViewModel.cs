using ConfigEditor.Domain;

namespace ConfigEditor.Models
{
    public class ServerViewModel
    {
        public ServerViewModel(Server server)
        {
            Name = server.Name;
            Overrides = server.Overrides;
        }

        public string Name { get; set; }

        public Dictionary<string, string> Overrides { get; set; }
    }
}
