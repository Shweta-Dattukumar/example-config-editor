using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace ConfigEditor.Domain
{
    public class ServerConfigWriter : IServerConfigWriter
    {
        public async Task WriteConfig(ServerConfig serverConfig)
        {
            var lines = new List<string>();

            WriteDefaults(serverConfig, lines);

            WriteOverrides(serverConfig, lines);

            var configLocation = Path.Combine(Directory.GetCurrentDirectory(), ServerConfigConstants.ConfigFileName);
            await File.WriteAllLinesAsync(configLocation, lines);
        }

        private static void WriteDefaults(ServerConfig serverConfig, List<string> lines)
        {
            lines.Add(";START DEFAULTS");

            foreach (var serverConfigDefault in serverConfig.Defaults)
            {
                var line = $"{serverConfigDefault.Key}={serverConfigDefault.Value}";

                lines.Add(line);
            }

            lines.Add(";END DEFAULTS");
            lines.Add("");
        }

        private static void WriteOverrides(ServerConfig serverConfig, List<string> lines)
        {
            foreach (var server in serverConfig.Servers)
            {
                lines.Add($";START {server.Name}");

                foreach (var serverConfigOverrides in server.Overrides)
                {
                    var line = $"{serverConfigOverrides.Key}{{{server.Name}}}={serverConfigOverrides.Value}";

                    lines.Add(line);
                }

                lines.Add($";END {server.Name}");
                lines.Add("");
            }

        }
    }
}
