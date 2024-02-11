using ConfigEditor.Domain;

namespace ConfigEditor.Models
{
    public class ServerConfigViewModel
    {
        public ServerConfigViewModel(ServerConfig serverConfig)
        {
            if(serverConfig == null) throw new ArgumentNullException(nameof(serverConfig));

            if (serverConfig.Servers != null)
            {
                foreach (var server in serverConfig.Servers)
                {
                    if (server != null)
                    {
                        Servers.Add(new ServerViewModel(server));
                    }
                }
            }
            
            Defaults = serverConfig.Defaults;
        }

        public List<ServerViewModel> Servers { get; set; } =  new List<ServerViewModel>();

        public Dictionary<string, string> Defaults { get; set; }
    }
}
