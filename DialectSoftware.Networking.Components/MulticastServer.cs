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
    public class MulticastServer
    {
            /// <summary> 
			/// Required designer variable.
			/// </summary>
            private int _port;
            private IPAddress _ip;
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

            public MulticastServer()
			{
                
			}    

            public void Listen(IPAddress ip, Int32 port)
			{
                _ip = ip;
                _port = port;
                Accept();
                //System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ThreadProc_Accept), mc);
			}

            private void Accept()
            {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(_ip, IPAddress.Any));
                s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                // Creates an IpEndPoint to capture the identity of the sending host.
                IPEndPoint IPRemote = new IPEndPoint(IPAddress.Any, _port);

                // Binding is required with ReceiveFrom calls.
                s.Bind(IPRemote);

                EndPoint remoteIP = (EndPoint)IPRemote;

                AsyncSocketAdapter adapter = new AsyncSocketAdapter(s, new DialectSoftware.Networking.AsyncNetworkErrorCallBack(Error));
                adapter.BeginRead(ref remoteIP, new AsyncNetworkCallBack(ThreadProc_ReadComplete), (int)TimeOut.Default);
            }

            private void ThreadProc_Accept(object stateInfo)
            {
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 1300);
                EndPoint senderRemote = (EndPoint)sender;
                ((AsyncSocketAdapter)stateInfo).BeginRead(ref senderRemote, new AsyncNetworkCallBack(ThreadProc_ReadComplete), (int)TimeOut.Default);
            }

            private void ThreadProc_ReadComplete(AsyncNetworkAdapter state)
            {
                Accept();
                if (this._receive != null)
                    this._receive.BeginInvoke(state, new System.AsyncCallback(EndInvoke), state);
            }

            private void ThreadProc_WriteComplete(AsyncNetworkAdapter state)
            {
                
            }

            private void Error(AsyncNetworkAdapter state, System.Exception e)
            {
                Accept();
                if (this._errorHandler != null)
                    this._errorHandler.BeginInvoke(state, e, new System.AsyncCallback(EndInvoke), state);
            }

            private void EndInvoke(System.IAsyncResult result)
            {
                object[] args = { result };
                ((System.Runtime.Remoting.Messaging.AsyncResult)result).AsyncDelegate.GetType().GetMethod("EndInvoke").Invoke(((System.Runtime.Remoting.Messaging.AsyncResult)result).AsyncDelegate, args);
            }

            public void Send(AsyncNetworkAdapter state, byte[] data)
            {
                ((AsyncSocketAdapter)state).BeginWrite(((AsyncSocketAdapter)state).RemoteAddress, data, new AsyncNetworkCallBack(ThreadProc_WriteComplete), (int)TimeOut.Default);
            }

    }
}
