using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

namespace DialectSoftware.Networking
{
    public class BroadcastClient
    {
            private Int32 _timeOut;
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

            public BroadcastClient()
			{
                
			}
			
			public void Send(Int32 port, byte[] data, Int32 timeOut)
			{
                _timeOut = timeOut;

                // Create a multicast socket.
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.SetSocketOption( SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                // Get the local IP address used by the listener and the sender to
                IPEndPoint IPlocal = new IPEndPoint(IPAddress.Any, 0);

                // Bind this endpoint to the multicast socket.
                socket.Bind(IPlocal);
				socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout,timeOut);
				socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout,timeOut);

                //Get the Broadcast IP for remote server
				IPEndPoint IPRemote = new IPEndPoint(IPAddress.Broadcast, port);

				//Create adapter
                AsyncSocketAdapter adapter = new AsyncSocketAdapter(socket, new AsyncNetworkErrorCallBack(ThreadProc_Error));

				//Send request.
                adapter.BeginWrite(IPRemote, data, new AsyncNetworkCallBack(ThreadProc_WriteComplete), timeOut);
			}

            private void ThreadProc_WriteComplete(AsyncNetworkAdapter state)
            {
                EndPoint IPRemote = ((AsyncSocketAdapter)state).RemoteAddress;

                //WAIT FOR RESPONSES
                System.Threading.Thread.Sleep(((AsyncSocketAdapter)state).TimeOut);

                ((AsyncSocketAdapter)state).BeginRead(ref IPRemote, new AsyncNetworkCallBack(ThreadProc_ReadComplete), _timeOut);
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
