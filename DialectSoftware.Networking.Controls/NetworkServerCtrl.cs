using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DialectSoftware.Networking
{
	namespace Controls
	{
		
		/// <summary>
		/// Summary description for NetworkServerCtrl.
		/// </summary>
		public class NetworkServerCtrl : System.Windows.Forms.UserControl
		{
			/// <summary> 
			/// Required designer variable.
			/// </summary>
			private System.Boolean started = false;
			private System.ComponentModel.Container components = null;
			private static System.Net.Sockets.TcpListener _server = null;
			private Security.Authentication.SSPI.SSPIAuthenticate _securityServer = null;
			private readonly System.Int32 _size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(long));

			public delegate void ServerNetworkCallBack(AsyncNetworkAdapter Adapter, System.Byte[] Data);
			public event ServerNetworkCallBack _receive = null;

			public delegate void ServerNetworkErrorHandler(AsyncNetworkAdapter state, System.Exception e);
			public event ServerNetworkErrorHandler _errorHandler = null;

			
			public ServerNetworkCallBack Receive
			{
				set
				{
					if(this._receive != null)
						throw new InvalidOperationException("Only one delegate can be assigned.");

					this._receive += value;
					
				}
			}

			public ServerNetworkErrorHandler ErrorHandler
			{
				set
				{
					if(this._errorHandler != null)
						throw new InvalidOperationException("Only one delegate can be assigned.");

					this._errorHandler += value;
				}
			}


			public Security.Authentication.SSPI.SSPIAuthenticate  SecurityServer
			{
				get{return _securityServer;}
			}

			public NetworkServerCtrl()
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
				System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(NetworkServerCtrl));
				// 
				// BuildServerCtrl
				// 
				this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
				this.Name = "BuildServerCtrl";
				this.Size = new System.Drawing.Size(32, 32);
				this.Load += new System.EventHandler(this.NetworkServerCtrl_Load);

			}
			#endregion

			private void NetworkServerCtrl_Load(object sender, System.EventArgs e)
			{
				
			}

			private void NetworkServerCtrl_AsyncCallback(System.IAsyncResult result)
			{
				object[] args = {result};
				((System.Runtime.Remoting.Messaging.AsyncResult)result).AsyncDelegate.GetType().GetMethod("EndInvoke").Invoke(((System.Runtime.Remoting.Messaging.AsyncResult)result).AsyncDelegate,args);

			}

			private void NetworkServerCtrl_Error(AsyncNetworkAdapter state,System.Exception e)
			{
				if(this._errorHandler != null)
					_errorHandler.BeginInvoke(state,e,new System.AsyncCallback(NetworkServerCtrl_AsyncCallback),null);
					
			}


			public void Stop()
			{
				if(started)
				{
					_server.Stop();
					started = !started;
				}

			}
					
			public void Accept(Int32 Port)
			{
				_server = new System.Net.Sockets.TcpListener(Port);
				_server.Start();
				started = true;
				System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ThreadProc));
			
			}

			public void Send(AsyncNetworkAdapter state, byte[] Data, Int32 TimeOut,bool WriteRead)
			{
				System.IO.MemoryStream stream = new System.IO.MemoryStream(Data.Length + _size);
				stream.Write(System.BitConverter.GetBytes(Data.LongLength),0,_size);
				stream.Write(Data,0,Data.Length);
				stream.Flush();
				stream.Seek(0,System.IO.SeekOrigin.Begin);
				if(WriteRead)
					((AsyncTcpClientAdapter)state).BeginWrite((byte[])stream.GetBuffer().Clone(),new AsyncNetworkCallBack(Read),TimeOut);
				else
					((AsyncTcpClientAdapter)state).BeginWrite((byte[])stream.GetBuffer().Clone(),new AsyncNetworkCallBack(End),TimeOut);

				stream.Close();
			}


			
			private void ThreadProc(object stateInfo)
			{
				while(started)
				{
					try
					{
						System.Net.Sockets.TcpClient client = _server.AcceptTcpClient(); 
						_securityServer = new Security.Authentication.SSPI.NTLM.AuthenticationServer();
						AsyncTcpClientAdapter adapter = new AsyncTcpClientAdapter(client,new AsyncNetworkErrorCallBack(NetworkServerCtrl_Error));
						Read(adapter);
						adapter = null;
					}
					catch
					{
						started = false;
					}
					
				}
			
			}

			private void Read(AsyncNetworkAdapter state)
			{
				if(((AsyncTcpClientAdapter)state).Status == AsyncNetworkStatus.Read || ((AsyncTcpClientAdapter)state).Status == AsyncNetworkStatus.ReadWrite)
					((AsyncTcpClientAdapter)state).Read(_size,(int)TimeOut.Default);
				else
					((AsyncTcpClientAdapter)state).BeginRead(new AsyncNetworkCallBack(ReadComplete),(int)TimeOut.Default);
			}
			
			private void End(AsyncNetworkAdapter state)
			{
					_receive.BeginInvoke(state,null,new System.AsyncCallback(NetworkServerCtrl_AsyncCallback),null);
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
							_receive.BeginInvoke(state,(byte[])stream.GetBuffer().Clone(),new System.AsyncCallback(NetworkServerCtrl_AsyncCallback),null);
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
				}
				base.Dispose( disposing );
			}

		}
	}
}
