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
      
        static DialectSoftware.Networking.MulticastServer server = new MulticastServer();
        
        static void Main(string[] args)
        {
            new Thread(() =>
            {
                server.Receive += new AsyncNetworkCallBack(server_Receive);
                server.Listen(IPAddress.Parse("224.5.6.7"), 4567);
                server.ErrorHandler += server_ErrorHandler;
            }).Start();

            Console.ReadKey();
        }

        static void server_ErrorHandler(AsyncNetworkAdapter adapter, Exception e)
        {
            Console.WriteLine(e.ToString());
        }

   
        static void server_Receive(AsyncNetworkAdapter adapter)
        {
            Console.WriteLine(System.Text.ASCIIEncoding.ASCII.GetString(adapter.Buffer));
            //server.Send(adapter, System.Text.ASCIIEncoding.ASCII.GetBytes(AppDomain.CurrentDomain.FriendlyName));
        }

   
    }
}
