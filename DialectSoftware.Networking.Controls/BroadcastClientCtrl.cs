using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using DialectSoftware.Networking;

/// ******************************************************************************************************************
/// * Copyright (c) 2011 Dialect Software LLC                                                                        *
/// * This software is distributed under the terms of the Apache License http://www.apache.org/licenses/LICENSE-2.0  *
/// *                                                                                                                *
/// ******************************************************************************************************************

namespace DialectSoftware.Networking
{
	namespace Controls
	{
		/// <summary>
		/// Summary description for BroadcastClientCtrl.
		/// </summary>
		
        public class BroadcastClientCtrl : System.Windows.Forms.UserControl
		{ 
			/// <summary> 
			/// Required designer variable.
			/// </summary>
			
			private Socket _socket = null;
	
			private System.ComponentModel.Container components = null;
			public event AsyncNetworkCallBack _receive = null;


			public delegate void BroadcastClientCtrlErrorHandler(System.Exception e);
			public event BroadcastClientCtrlErrorHandler _errorHandler = null;


			public AsyncNetworkCallBack Receive
			{
				set
				{
					if(this._receive != null)
						throw new InvalidOperationException("Only one delegate can be assigned.");

					this._receive += value;
					
				}
			}

			public BroadcastClientCtrlErrorHandler ErrorHandler
			{
				set{
					if(this._errorHandler != null)
						throw new InvalidOperationException("Only one delegate can be assigned.");


					this._errorHandler += value;
				}
			}

            public BroadcastClientCtrl()
			{
				// This call is required by the Windows.Forms Form Designer.
				InitializeComponent();
			}

			
			#region Component Designer generated code
			/// <summary> 
			/// Required method for Designer support - do not modify 
			/// the contents of this method with the code editor.
			/// </summary>
			private void InitializeComponent()
			{
				System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BroadcastClientCtrl));
				// 
				// BroadcastClientCtrl
				// 
				this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
				this.Name = "BroadcastClientCtrl";
				this.Size = new System.Drawing.Size(32, 32);
				this.Load += new System.EventHandler(this.BroadcastClientCtrl_Load_1);

			}
			#endregion

			private void BroadcastClientCtrl_Load(object sender, System.EventArgs e)
			{
				// Create a multicast socket.
				_socket = new Socket(
											AddressFamily.InterNetwork, 
											SocketType.Dgram, 
											ProtocolType.Udp);
			    
				_socket.SetSocketOption(
											SocketOptionLevel.Socket, 
											SocketOptionName.Broadcast,1);
						
				// Get the local IP address used by the listener and the sender to
				IPEndPoint IPlocal = new IPEndPoint(IPAddress.Any , 0);
				
				// Bind this endpoint to the multicast socket.
				_socket.Bind(IPlocal);
			}

			private void BroadcastClientCtrl_AsyncCallback(System.IAsyncResult result)
			{
				object[] args = {result};
				((System.Runtime.Remoting.Messaging.AsyncResult)result).AsyncDelegate.GetType().GetMethod("EndInvoke").Invoke(((System.Runtime.Remoting.Messaging.AsyncResult)result).AsyncDelegate,args);
			}

			private void BroadcastClientCtrl_Error(AsyncNetworkAdapter state,System.Exception e)
			{
				if(this._errorHandler != null)
					this._errorHandler.BeginInvoke(e,new System.AsyncCallback(BroadcastClientCtrl_AsyncCallback),null);
				
			}

			public void BroadcastRequest(Int32 Port, byte[] data,Int32 TimeOut)
			{
				_socket.SetSocketOption(
					SocketOptionLevel.Socket, 
					SocketOptionName.SendTimeout,TimeOut);


				_socket.SetSocketOption(
					SocketOptionLevel.Socket, 
					SocketOptionName.ReceiveTimeout,TimeOut);

                //Get the Broadcast IP for remote server
				IPEndPoint IPRemote = new IPEndPoint(IPAddress.Broadcast,Port);
				//mcastSocket.Connect(IPRemote);
				AsyncSocketAdapter broadcast = new AsyncSocketAdapter(_socket,new AsyncNetworkErrorCallBack(BroadcastClientCtrl_Error));
				// Receive brodcast messages.
				broadcast.BeginWrite(IPRemote,data,new AsyncNetworkCallBack(WriteComplete),TimeOut);
			}

			public void BroadcastRequest(AsyncNetworkAdapter state,byte[] data,Int32 TimeOut)
			{
				_socket.SetSocketOption(
					SocketOptionLevel.Socket, 
					SocketOptionName.SendTimeout,TimeOut);


				_socket.SetSocketOption(
					SocketOptionLevel.Socket, 
					SocketOptionName.ReceiveTimeout,TimeOut);

				//Get the Broadcast IP for remote server
				EndPoint IPRemote = ((AsyncSocketAdapter)state).RemoteAddress;
				// Receive brodcast messages.
				((AsyncSocketAdapter)state).BeginWrite(IPRemote,data,new AsyncNetworkCallBack(WriteComplete),TimeOut);
			}

			private void WriteComplete(AsyncNetworkAdapter state)
			{
				if(this._receive != null)
				{
					EndPoint IPRemote = ((AsyncSocketAdapter)state).RemoteAddress;
					((AsyncSocketAdapter)state).BeginRead(ref IPRemote,this._receive,(int)TimeOut.Default);
				}
				else
				{
					state.Dispose();
				}
			}

		


			//Finalize/Dispose
			protected override void Dispose( bool disposing )
			{
				if( disposing )
				{
					if(components != null)
					{
						components.Dispose();
					}
				}
				base.Dispose( disposing );
			}

			private void BroadcastClientCtrl_Load_1(object sender, System.EventArgs e)
			{
			
			}

		}
	}
}
