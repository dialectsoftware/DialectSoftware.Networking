using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Net.Sockets;

namespace DialectSoftware.Networking
{
	namespace Controls
	{
		
		/// <summary>
		/// Summary description for NetworkClientCtrl.
		/// </summary>
		public class NetworkClientCtrl : System.Windows.Forms.UserControl
		{
			/// <summary> 
			/// Required designer variable.
			/// </summary>
			
			private AsyncTcpClientAdapter _client = null;
			private System.ComponentModel.Container components = null;
			private Security.Authentication.SSPI.SSPIAuthenticate _securityServer = null;
			private readonly System.Int32 _size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(long));
	        
			public delegate void NetworkClientCallBack(System.Byte[] Data);
			public event NetworkClientCallBack _receive = null;

			public delegate void NetworkClientCtrlErrorHandler(System.Exception e);
			public event NetworkClientCtrlErrorHandler _errorHandler = null;
		
			public AsyncTcpClientAdapter  Client
			{
				get{return this._client;}
			}

			public Security.Authentication.SSPI.SSPIAuthenticate SecurityServer
			{
				get{return _securityServer;}
			}

			public NetworkClientCallBack Receive
			{
				set
				{
					if(this._receive != null)
						throw new InvalidOperationException("Only one delegate can be assigned.");

					this._receive += value;
					
				}
			}

			public NetworkClientCtrlErrorHandler ErrorHandler
			{
				set
				{
					if(this._errorHandler != null)
						throw new InvalidOperationException("Only one delegate can be assigned.");

					this._errorHandler += value;
				}
			}


			public NetworkClientCtrl()
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
				System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(NetworkClientCtrl));
				// 
				// NetworkClientCtrl
				// 
				this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
				this.Name = "NetworkClientCtrl";
				this.Size = new System.Drawing.Size(24, 32);
				this.Load += new System.EventHandler(this.NetworkClientCtrl_Load_1);

			}
			#endregion

			private void NetworkClientCtrl_Load(object sender, System.EventArgs e)
			{
			 			
			}

			private void NetworkClientCtrl_AsyncCallback(System.IAsyncResult result)
			{
				object[] args = {result};
				((System.Runtime.Remoting.Messaging.AsyncResult)result).AsyncDelegate.GetType().GetMethod("EndInvoke").Invoke(((System.Runtime.Remoting.Messaging.AsyncResult)result).AsyncDelegate,args);
			}

			private void NetworkClientCtrl_Error(AsyncNetworkAdapter state,System.Exception e)
			{
				if(this._errorHandler  != null)
					_errorHandler.BeginInvoke(e,new System.AsyncCallback(NetworkClientCtrl_AsyncCallback),null);
			}


			public bool Connect(String IP,Int32 Port)
			{
				try
				{
					if(_client!=null)
						throw new System.InvalidOperationException("Socket is currently connected.");
					System.Net.IPEndPoint IPRemote = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(IP),Port);
					System.Net.Sockets.TcpClient socket = new TcpClient();
					socket.Connect(IPRemote);
					_securityServer = new Security.Authentication.SSPI.NTLM.AuthenticationClient();
					_client = new AsyncTcpClientAdapter(socket,new AsyncNetworkErrorCallBack(NetworkClientCtrl_Error)); 
					socket = null;
					return true;
				}
				catch
				{
					return false;
				}
			}

			public void Send(byte[] Data,Int32 TimeOut,bool WriteRead)
			{
				System.IO.MemoryStream stream = new System.IO.MemoryStream(Data.Length + _size);
				stream.Write(System.BitConverter.GetBytes(Data.LongLength),0,_size);
				stream.Write(Data,0,Data.Length);
				stream.Flush();
				stream.Seek(0,System.IO.SeekOrigin.Begin);
				if(WriteRead)
					_client.BeginWrite((byte[])stream.GetBuffer().Clone(),new AsyncNetworkCallBack(WriteComplete),TimeOut);
				else
					_client.BeginWrite((byte[])stream.GetBuffer().Clone(),new AsyncNetworkCallBack(End),TimeOut);
				stream.Close();
			}

			public void End()
			{
				if(_client!=null)
				{
					_client.Dispose();
					_client = null;
				}
			}

			private void WriteComplete(AsyncNetworkAdapter state)
			{
				if(((AsyncTcpClientAdapter)state).Status == AsyncNetworkStatus.Read || ((AsyncTcpClientAdapter)state).Status == AsyncNetworkStatus.ReadWrite)
					((AsyncTcpClientAdapter)state).Read(_size,(int)TimeOut.Default);
				else
					((AsyncTcpClientAdapter)state).BeginRead(new AsyncNetworkCallBack(ReadComplete),(int)TimeOut.Default);
			}


			private void End(AsyncNetworkAdapter state)
			{
				_receive.BeginInvoke(null,new System.AsyncCallback(NetworkClientCtrl_AsyncCallback),null);
			}


			private void ReadComplete(AsyncNetworkAdapter state)
			{
				if(state.Buffer.Length >= _size)
				{
					long length = System.BitConverter.ToInt64(state.Buffer,0);
					if(state.Buffer.LongLength < (length + _size))
					{
						((AsyncTcpClientAdapter)state).Read((length + _size) - state.Buffer.LongLength,(int)TimeOut.Default);
					}
					else
					{
						System.IO.MemoryStream stream = new System.IO.MemoryStream((int)length); 
						stream.Write(state.Buffer,_size,(int)length);
						stream.Seek(0,System.IO.SeekOrigin.Begin);  
						if(this._receive != null)
							_receive.BeginInvoke((byte[])stream.GetBuffer().Clone(),new System.AsyncCallback(NetworkClientCtrl_AsyncCallback),null);
						stream.Close();
					}
				}
			}


			protected override void Dispose( bool disposing )
			{
				if( disposing )
				{
					if(components != null)
					{
						components.Dispose();
					}
					if(_client!=null)
					{
						_client.Dispose();
					}
				}
				base.Dispose( disposing );
			}

			private void NetworkClientCtrl_Load_1(object sender, System.EventArgs e)
			{
			
			}

		}
}
}
