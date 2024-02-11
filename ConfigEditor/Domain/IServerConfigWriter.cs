namespace ConfigEditor.Domain;

public interface IServerConfigWriter
{
    Task WriteConfig(ServerConfig serverConfig);
}