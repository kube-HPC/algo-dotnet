using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

public class AlgorithmManager
{
    private WebSocketClient client;
    private Algorithm algorithm;
    public AlgorithmManager(int port)
    {
        client = new WebSocketClient(9876);
        client.Initialize += new EventHandler(InitHandler);
        client.Start += new EventHandler(StartHandler);
        client.Stop += new EventHandler(StopHandler);
        client.Exit += new EventHandler(ExitHandler);

        algorithm = new Algorithm();
        algorithm.Done += new EventHandler(DoneHandler);
        algorithm.Error += new EventHandler(ErrorHandler);
    }
    public void Init()
    {
        client.Connect();
    }
    private void ExitHandler(object sender, EventArgs e)
    {
        Console.WriteLine("Recevied exeit from worker");
        algorithm.Exit();
    }
    private void StopHandler(object sender, EventArgs e)
    {
        Console.WriteLine("Recevied stop from worker");
        algorithm.Stop();
    }
    private async void StartHandler(object sender, EventArgs e)
    {
        Console.WriteLine("Recevied start from worker");
        algorithm.Start();
        await client.Send(new ResponseCommand { command = ResponseCommandType.started });

    }
    private async void InitHandler(object sender, EventArgs e)
    {
        Console.WriteLine("Recevied init from worker");
        algorithm.Init("test");
        await client.Send(new ResponseCommand { command = ResponseCommandType.initialized });

    }
    private async void DoneHandler(object sender, EventArgs e)
    {
        Console.WriteLine("Sending done");
        await client.Send(new ResponseCommand { command = ResponseCommandType.done });
    }
    private async void ErrorHandler(object sender, EventArgs e)
    {
        Console.WriteLine("Recevied error");
        await client.Send(new ResponseCommand { command = ResponseCommandType.error });
    }
}