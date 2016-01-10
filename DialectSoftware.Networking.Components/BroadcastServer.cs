using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

/// ******************************************************************************************************************
/// * Copyright (c) 2011 Dialect Software LLC                                                                        *
/// * This software is distributed under the terms of the Apache License http://www.apache.org/licenses/LICENSE-2.0  *
/// *                                                                                                                *
/// ******************************************************************************************************************

namespace DialectSoftware.Networking
{
    public class BroadcastServer
    {

            private Int32 _port;
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
                    this._errorHandler += value; ;
                }

                remove
                {
                    this._errorHandler -= value;
                }
            }

        	public BroadcastServer()
			{
				
			}

			public void Listen(Int32 port)
            {
                _port = port;
                Accept();
                //System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(Accept),null);
			}

            private void Accept()
            {
                //IPHostEntry hostEntry = Dns.Resolve(Dns.GetHostName());
                //IPEndPoint IPLocal = new IPEndPoint(hostEntry.AddressList[0], _port);
                //IPEndPoint IPLocal = new IPEndPoint(IPAddress.Any, _port);

                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                // Creates an IpEndPoint to capture the identity of the sending host.
                IPEndPoint IPRemote = new IPEndPoint(IPAddress.Any, _port);
                
                // Binding is required with ReceiveFrom calls.
                s.Bind(IPRemote);
               
                EndPoint sender = (EndPoint)IPRemote;

                AsyncSocketAdapter adapter = new AsyncSocketAdapter(s, new DialectSoftware.Networking.AsyncNetworkErrorCallBack(ThreadProc_Error));
                adapter.BeginRead(ref sender, new AsyncNetworkCallBack(ThreadProc_ReadComplete), (int)TimeOut.Default);
            }

            private void ThreadProc_ReadComplete(AsyncNetworkAdapter state)
            {
                Accept();
                if (_receive != null)
                    _receive.BeginInvoke(state, EndInvoke, null);
            }

            private void ThreadProc_WriteComplete(AsyncNetworkAdapter state)
            {
                state.Dispose();
            }

            private void ThreadProc_Error(AsyncNetworkAdapter state, System.Exception e)
            {
                if (_errorHandler != null)
                    _errorHandler.BeginInvoke(state, e, EndInvoke, state);
                state.Dispose();
            }

            public void Send(AsyncNetworkAdapter state, byte[] data)
            {
                ((AsyncSocketAdapter)state).BeginWrite(((AsyncSocketAdapter)state).RemoteAddress, data, new AsyncNetworkCallBack(ThreadProc_WriteComplete), (int)TimeOut.Default);
            }

            private void EndInvoke(System.IAsyncResult result)
            {
                object[] args = { result };
                ((System.Runtime.Remoting.Messaging.AsyncResult)result).AsyncDelegate.GetType().GetMethod("EndInvoke").Invoke(((System.Runtime.Remoting.Messaging.AsyncResult)result).AsyncDelegate, args);
                IDisposable disposer = ((System.Runtime.Remoting.Messaging.AsyncResult)result).AsyncState as IDisposable;
                if (disposer != null)
                    disposer.Dispose();

            }
    }
}
