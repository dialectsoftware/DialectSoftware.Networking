using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using DialectSoftware.Networking;
using System.Net.NetworkInformation;

namespace DialectSoftware
{
    class Program
    {
        static DialectSoftware.Networking.BroadcastClient client = new Networking.BroadcastClient();

        static void Main(string[] args)
        {
            Console.WriteLine("//****This program accepts console input***//");
            //client.Receive += new Networking.AsyncNetworkCallBack(client_Receive);
            ConsoleKeyInfo result;
            while ((result = Console.ReadKey()).Key != ConsoleKey.Escape)
            {
                client.Send(1300, System.Text.ASCIIEncoding.ASCII.GetBytes(result.KeyChar.ToString()), -1);
            }
            
        }

        static void client_Receive(Networking.AsyncNetworkAdapter adapter)
        {
            Console.WriteLine(System.Text.ASCIIEncoding.ASCII.GetString(adapter.Buffer));
        }

    }
}
