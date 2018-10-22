using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

public class Algorithm
{
    public event EventHandler Done;
    public event EventHandler Error;
    public void Init(string options)
    {
    }

    public void Start()
    {
        Task.Run(() => { ExeuteAlgorithm(); });
    }

    private void ExeuteAlgorithm()
    {
        try
        {
            Thread.Sleep(5000);
            if (Done != null)
                Done(this, EventArgs.Empty);
        }
        catch (Exception e)
        {
            if (Error != null)
                Done(Error, EventArgs.Empty);
        }
    }
    public void Stop()
    {

    }

    public void Exit()
    {

    }
}