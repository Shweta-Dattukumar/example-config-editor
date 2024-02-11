namespace ConfigEditor.Domain;

public interface IServerConfigReader
{
    Task<ServerConfig>  GetServerConfig();
}