using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

public class WebSocketClient
{
    private ClientWebSocket client;
    private int port;
    public event EventHandler Initialize;
    public event EventHandler Start;
    public event EventHandler Stop;
    public event EventHandler Exit;


    public WebSocketClient(int socketWorkerPort)
    {
        port = socketWorkerPort;
    }

    public async void Connect()
    {
        client = new ClientWebSocket();
        await client.ConnectAsync(new Uri("ws://localhost:" + port), CancellationToken.None);
        Console.WriteLine("Connected!");
        Receiving();
    }

    private async void Receiving()
    {
        var buffer = new byte[1024 * 4];
        while (true)
        {
            var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                RequestCommand c = JsonConvert.DeserializeObject<RequestCommand>(Encoding.UTF8.GetString(buffer, 0, result.Count));
                switch (c.command)
                {
                    case RequestCommandType.exit:
                        if (Exit != null)
                            Exit(this, EventArgs.Empty);
                        break;
                    case RequestCommandType.start:
                        if (Start != null)
                            Start(this, EventArgs.Empty);
                        break;
                    case RequestCommandType.initialize:
                        if (Initialize != null)
                            Initialize(this, EventArgs.Empty);
                        break;
                    case RequestCommandType.stop:
                        if (Stop != null)
                            Stop(this, EventArgs.Empty);
                        break;
                    case RequestCommandType.pingMessage:
                        break;
                    case RequestCommandType.cleanup:
                        break;
                    default:
                        throw new ArgumentException($"could not handle command { c.command }");
                }
            }
        }
    }

    internal async Task Close()
    {
        await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
    }

    internal async Task Send(ResponseCommand responseCommand)
    {
        var command = JsonConvert.SerializeObject(responseCommand);
        byte[] res = Encoding.UTF8.GetBytes(command);
        await client.SendAsync(new ArraySegment<byte>(res), WebSocketMessageType.Text, true, CancellationToken.None);
    }
}