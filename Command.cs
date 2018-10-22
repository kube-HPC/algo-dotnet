using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class RequestCommand
{
    public RequestCommandType command { get; set; }
}

public class ResponseCommand
{
    public ResponseCommandType command { get; set; }
}

public enum RequestCommandType
{
    exit,
    initialize,
    start,
    cleanup,
    stop,
    pingMessage
}

[JsonConverter(typeof(StringEnumConverter))]
public enum ResponseCommandType
{
    done,
    initialized,
    started,
    progress,
    stopped,
    error,
    pongMessage
}