using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace s_client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var manager = new AlgorithmManager(9876);
            manager.Init();
            Console.ReadLine();
        }
    }
}
