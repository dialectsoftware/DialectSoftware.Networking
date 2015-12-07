using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace DialectSoftware.Networking
{
    public class MulticastClient
    {
            private event AsyncNetworkCallBack _receive = null;
            private event AsyncNetworkErrorCallBack _errorHandler = null;

            public event AsyncNetworkCallBack Receive
            {
                add 
                {
                    if (this._receive != null)
                        throw new InvalidOperationException("Only one delegate can be assigned.");
                    this._receive += value;
                }

                remove
                {
                    
                    this._receive -= value;

                }
            }

            public event AsyncNetworkErrorCallBack ErrorHandler
            {
                add
                {
                     if (this._errorHandler != null)
                        throw new InvalidOperationException("Only one delegate can be assigned.");
                     this._errorHandler += value;
                }

                remove
                {
                     this._errorHandler -= value;
                }
        
            }

            public MulticastClient()
			{
                //// Create a multicast socket.
                //_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                
                ////Allow multiple
                //_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                //// Get the local IP address used by the listener and the sender to
                //IPEndPoint IPlocal = new IPEndPoint(IPAddress.Any, 0);

                //// Bind this endpoint to the multicast socket.
                //_socket.Bind(IPlocal);
			}
			
			public void Send(IPAddress ip, Int32 Port, byte[] data, Int32 timeOut)
			{
                //Create a multicast socket.
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip));
                socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, timeOut);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, timeOut);

                // Get the local IP address used by the listener and the sender to
                // exchange multicast messages. 
                IPEndPoint IPlocal = new IPEndPoint(IPAddress.Any, 0);

                //EndPoint senderRemote = (EndPoint)IPlocal;
                IPEndPoint IPRemote = new IPEndPoint(ip, Port);

                // Bind this endpoint to the multicast socket.
                socket.Bind(IPlocal);

                //mcastSocket.Connect(IPRemote);
                AsyncSocketAdapter adapter = new AsyncSocketAdapter(socket, new DialectSoftware.Networking.AsyncNetworkErrorCallBack(ThreadProc_Error));

                // Receive brodcast messages.
                adapter.BeginWrite(IPRemote,data, new AsyncNetworkCallBack(ThreadProc_WriteComplete), timeOut);
         	}

            private void ThreadProc_WriteComplete(AsyncNetworkAdapter state)
            {
                EndPoint IPRemote = ((AsyncSocketAdapter)state).RemoteAddress;
                
                //WAIT FOR RESPONSES
                System.Threading.Thread.Sleep(((AsyncSocketAdapter)state).TimeOut);

                ((AsyncSocketAdapter)state).BeginRead(ref IPRemote, new AsyncNetworkCallBack(ThreadProc_ReadComplete), (int)TimeOut.Default);
            }

            private void ThreadProc_ReadComplete(AsyncNetworkAdapter state)
            {
                if (_receive != null)
                    _receive(state);
                state.Dispose();
            }

            private void ThreadProc_Error(AsyncNetworkAdapter state, System.Exception e)
            {
                if (_errorHandler != null)
                    _errorHandler(state, e);
                state.Dispose();
            }

    }
}
