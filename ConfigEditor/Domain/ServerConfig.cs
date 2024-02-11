namespace ConfigEditor.Domain;

public class ServerConfig
{
    public List<Server> Servers { get; set; }

    public Dictionary<string, string> Defaults { get; set; }
}