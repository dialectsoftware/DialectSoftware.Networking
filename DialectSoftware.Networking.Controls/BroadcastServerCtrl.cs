using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

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
		/// Summary description for BroadcastServer.
		/// </summary>
		public class BroadcastServerCtrl : System.Windows.Forms.UserControl
		{
			/// <summary> 
			/// Required designer variable.
			/// </summary>
			private Socket _socket = null;
			private AsyncSocketAdapter _server =null;

			private System.ComponentModel.Container components = null;
			public event AsyncNetworkCallBack _receive = null;		

			public delegate void BroadcastServerCtrlErrorHandler(AsyncNetworkAdapter state,System.Exception e);
			public event BroadcastServerCtrlErrorHandler _errorHandler = null;


			public AsyncNetworkCallBack Receive
			{
				set
				{
					if(this._receive != null)
						throw new InvalidOperationException("Only one delegate can be assigned.");

					this._receive += value;
					
				}
			}

			public BroadcastServerCtrlErrorHandler ErrorHandler
			{
				set
				{
					if(this._errorHandler != null)
						throw new InvalidOperationException("Only one delegate can be assigned.");


					this._errorHandler += value;
				}
			}


			public BroadcastServerCtrl()
			{
				// This call is required by the Windows.Forms Form Designer.
				InitializeComponent();

				// TODO: Add any initialization after the InitializeComponent call

			}


			#region Component Designer generated code
			/// <summary> 
			/// Required method for Designer support - do not modify 
			/// the contents of this method with the code editor.
			/// </summary>
			private void InitializeComponent()
			{
				System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BroadcastServerCtrl));
				// 
				// BroadcastServerCtrl
				// 
				this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
				this.Name = "BroadcastServerCtrl";
				this.Size = new System.Drawing.Size(32, 32);
				this.Load += new System.EventHandler(this.BroadcastServerCtrl_Load);

			}
			#endregion
			
			private void BroadcastServer_Load(object sender, System.EventArgs e)
			{
							
			}
		
			private void BroadcastServerCtrl_AsyncCallback(System.IAsyncResult result)
			{
				object[] args = {result};
				((System.Runtime.Remoting.Messaging.AsyncResult)result).AsyncDelegate.GetType().GetMethod("EndInvoke").Invoke(((System.Runtime.Remoting.Messaging.AsyncResult)result).AsyncDelegate,args);
			}

			private void BroadcastServerCtrl_Error(AsyncNetworkAdapter state,System.Exception e)
			{
				if(this._errorHandler != null)
					this._errorHandler.BeginInvoke(state,e,new System.AsyncCallback(BroadcastServerCtrl_AsyncCallback),null);
				
			}

			
			public void Stop()
			{
				this._socket.Close();
				this._server.Dispose();
				this._socket = null;
				this._server = null;
			}

			public void Listen(Int32 Port)
			{
				if(_server!=null)
					throw new System.InvalidOperationException("Socket is already open");
		
				_socket = new Socket(
					AddressFamily.InterNetwork,
					SocketType.Dgram,
					ProtocolType.Udp);

				// Creates an IpEndPoint to capture the identity of the sending host.
				IPEndPoint IPlocal = new IPEndPoint(IPAddress.Any, Port);
				
				// Binding is required with ReceiveFrom calls.
				_socket.Bind(IPlocal);

				// Receive IP request messages.
				_server = new AsyncSocketAdapter(_socket, new AsyncNetworkErrorCallBack(BroadcastServerCtrl_Error));
				Listen((EndPoint)IPlocal);
			}

			private void Listen(EndPoint IPRemote)
			{			
				if(_server.Status == AsyncNetworkStatus.Read || _server.Status == AsyncNetworkStatus.ReadWrite )
					_server.Read(ref IPRemote,System.Threading.Timeout.Infinite); 
				else    
					_server.BeginRead(ref IPRemote,this._receive,System.Threading.Timeout.Infinite); 
			}

			private void Accept(AsyncNetworkAdapter state)
			{
				Listen((EndPoint)this._server.RemoteAddress );
			}

			public void Respond(AsyncNetworkAdapter state, byte[] data)
			{
				((AsyncSocketAdapter)state).BeginWrite(((AsyncSocketAdapter)state).RemoteAddress,data,new AsyncNetworkCallBack(Accept),(int)TimeOut.Default); 
			}

			
			
			//Finalize/Dispose
			protected override void Dispose( bool disposing )
			{
				if( disposing )
				{
					if(components != null)
					{
						components.Dispose();
						_server.Dispose();
					}
				}
				base.Dispose( disposing );
			}

			private void BroadcastServerCtrl_Load(object sender, System.EventArgs e)
			{
			
			}


		}
	}
}
