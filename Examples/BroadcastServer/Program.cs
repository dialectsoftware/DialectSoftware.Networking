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
        static DialectSoftware.Networking.BroadcastServer server = new Networking.BroadcastServer();
        
        static void Main(string[] args)
        {
            server.Receive += new DialectSoftware.Networking.AsyncNetworkCallBack(server_Receive);
            server.Listen(1300);
            Console.ReadLine();
        }

    
        static void server_Receive(Networking.AsyncNetworkAdapter adapter)
        {
            Console.Write(Convert.ToChar(adapter.Buffer[0]));
            //server.Send(adapter, System.Text.ASCIIEncoding.ASCII.GetBytes(AppDomain.CurrentDomain.FriendlyName));
        }

      

    }
}
