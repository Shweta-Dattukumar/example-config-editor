namespace ConfigEditor.Domain
{
    public class ServerConfigReader : IServerConfigReader
    {
        public async Task<ServerConfig>  GetServerConfig()
        {
            // get the file 
            var configLocation = Path.Combine(Directory.GetCurrentDirectory(), ServerConfigConstants.ConfigFileName);

            var configLines = await File.ReadAllLinesAsync(configLocation);

            var serverConfig = new ServerConfig
            {
                Servers = new List<Server>(),
                Defaults = new Dictionary<string, string>()
            };

            foreach (var configLine in configLines)
            {
                var line = configLine.Trim();

                // ignore comments and blank line from the config text file.
                if (line.StartsWith(';') || line.Length == 0)
                {
                    continue;
                }

                // SERVER_NAME=MRAPPPOOLPORTL01

                var lineParts = line.Split('=');
                if (lineParts.Length != 2)
                {
                    throw new InvalidOperationException($"Invalid config line. {line}");
                }
                var key = lineParts[0];
                var value = lineParts[1];

                // check whether this is a server override, if so then add to the server list
                if (key.Contains('{') && key.Contains('}'))
                {
                    var startIndex = key.IndexOf('{') + 1;
                    var serverName = key.Substring(startIndex, key.IndexOf('}') - startIndex);

                    var server = serverConfig.Servers.FirstOrDefault(x => x.Name == serverName);

                    var keyExcludingServerName = key.Substring(0, key.IndexOf('{'));

                    if (server == null)
                    {
                        server = new Server
                        {
                            Name = serverName,
                            Overrides = new Dictionary<string, string>()
                        };

                        serverConfig.Servers.Add(server);
                    }

                    server.Overrides.Add(keyExcludingServerName, value);
                }
                // if this is default then we add it to the default config
                else
                {
                    serverConfig.Defaults.Add(key, value);
                }
            }

            return serverConfig;
        }
    }
}
