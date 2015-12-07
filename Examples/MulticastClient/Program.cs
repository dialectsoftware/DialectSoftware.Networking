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
        static DialectSoftware.Networking.MulticastClient client = new MulticastClient();

        static void Main(string[] args)
        {
            Console.WriteLine("//***This program accepts console input***//");
            client.Receive += new AsyncNetworkCallBack(client_Receive);
            string result;
            while ((result = Console.ReadLine()) != String.Empty)
            {
                client.Send(IPAddress.Parse("224.5.6.7"), 4567, System.Text.ASCIIEncoding.ASCII.GetBytes(result), 3000);
            }
        }


        static void client_Receive(AsyncNetworkAdapter adapter)
        {
           Console.WriteLine("MultiCast Client Received " + System.Text.ASCIIEncoding.ASCII.GetString(adapter.Buffer));
        }

    }
}
