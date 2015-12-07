using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;

namespace DialectSoftware.Networking
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class AsyncTcpClientAdapter:AsyncNetworkAdapter
	{
		public TcpClient socket = null;
		private System.Threading.Timer monitor = null;
		private Int32 _timeout = System.Threading.Timeout.Infinite;

		
		public AsyncTcpClientAdapter(TcpClient socket, AsyncNetworkErrorCallBack Error):base(Error)
		{
			this.socket = socket; 
			this.monitor = new System.Threading.Timer(new System.Threading.TimerCallback(Expire),this,_timeout,_timeout);    
		}

		public bool DataAvailable
		{
			get{return this.socket.GetStream().DataAvailable;}
		}
		
		protected override System.AsyncCallback AsyncReadCallback
		{
			get{return new System.AsyncCallback(this.ReadCallBack);}
		}

		protected override System.AsyncCallback AsyncWriteCallback
		{
			get{return new System.AsyncCallback(this.WriteCallBack);}
		}


		//Monitor
		private static void Expire(object state)
		{
			((AsyncTcpClientAdapter)state).monitor.Change(System.Threading.Timeout.Infinite,System.Threading.Timeout.Infinite); 
 
			System.Net.Sockets.Socket socket = null;
			if(((AsyncTcpClientAdapter)state).socket != null)
				socket = ((AsyncTcpClientAdapter)state).socket.GetType().GetField("m_ClientSocket",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic).GetValue(((AsyncTcpClientAdapter)state).socket) as  System.Net.Sockets.Socket;
			
			if(socket == null)
				return;
			if(!socket.Connected)
				socket.Close();
			else if(socket.Available==0)
			{
				((AsyncTcpClientAdapter)state).socket.GetStream().Close();
				((AsyncTcpClientAdapter)state).socket.Close();
			}
			else //maybe use reflection or use Default
			{	
				Int32 TimeOut =(Int32)((AsyncTcpClientAdapter)state).GetType().GetField("_timeout",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic).GetValue(0);
				((AsyncTcpClientAdapter)state).monitor.Change(TimeOut ,System.Threading.Timeout.Infinite); 
			}
				
		}


		//Listener
		public /*override*/ void BeginRead(AsyncNetworkCallBack ReadComplete,Int32 TimeOut)
		{
			base.BeginRead(ReadComplete);
			this.socket.ReceiveTimeout = TimeOut;
			this.socket.GetStream().BeginRead(this._buffer,0,_bufferSize,this.AsyncReadCallback,null);  
			this._timeout = TimeOut;
			this.monitor.Change(TimeOut,System.Threading.Timeout.Infinite);
		}

		public void BeginRead(long Length,AsyncNetworkCallBack ReadComplete,Int32 TimeOut)
		{
			base.BeginRead(ReadComplete);
			this.socket.ReceiveTimeout = TimeOut;
			this.socket.GetStream().BeginRead(this._buffer,0,((Int64)this._bufferSize >= Length)? (Int32)Length:this._bufferSize,this.AsyncReadCallback,Length);  
			this._timeout = TimeOut;
			this.monitor.Change(TimeOut,System.Threading.Timeout.Infinite);
		}

		public /*override*/ void Read(Int32 TimeOut)
		{
			base.Read();
			this.socket.ReceiveTimeout = TimeOut;
			this.socket.GetStream().BeginRead(this._buffer,0,_bufferSize,this.AsyncReadCallback,null); 
			this._timeout = TimeOut;
			this.monitor.Change(TimeOut,System.Threading.Timeout.Infinite);
		}
		
		public void Read(long Length,Int32 TimeOut)
		{
			base.Read();
			this.socket.ReceiveTimeout = TimeOut;
			this.socket.GetStream().BeginRead(this._buffer,0,((Int64)this._bufferSize >= Length)? (Int32)Length:this._bufferSize,this.AsyncReadCallback,Length); 
			this._timeout = TimeOut;
			this.monitor.Change(TimeOut,System.Threading.Timeout.Infinite);
		}


		//Read Callback
		protected override void ReadCallBack(IAsyncResult result)
		{
			this.monitor.Change(System.Threading.Timeout.Infinite,System.Threading.Timeout.Infinite); 
			try
			{
				//if((int)((System.Net.Sockets.Socket)this.socket.GetType().GetField("m_ClientSocket",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic).GetValue(this.socket)).Handle==-1)
				//{
				//	throw new System.InvalidOperationException("Operation not allowed on non-connected sockets.");
				//}

                Int32 length = this.socket.GetStream().EndRead(result);
				this.buffer.Write(this._buffer,0,length);
				this.buffer.Flush(); 
				this.Clear(this._buffer); 
				
				if(result.AsyncState != null)
				{
					Int64 Length = (Int64)result.AsyncState;
					Length -= length;
					if(Length > 0 )
					{
						this.socket.GetStream().BeginRead(this._buffer,0,(this._bufferSize >= (Int32)Length)? (Int32)Length:this._bufferSize,this.AsyncReadCallback,Length);
						this.monitor.Change(this._timeout,System.Threading.Timeout.Infinite);
						return;
					}	
				}
				else
				{
					if(this.socket.GetStream().DataAvailable)
					{
						this.socket.GetStream().BeginRead(this._buffer,0,_bufferSize,this.AsyncReadCallback,null);
						this.monitor.Change(this._timeout,System.Threading.Timeout.Infinite);
						return;
					}
				}
				
				base.ReadCallBack (result);
			}
			catch(System.Exception e)
			{
				base.ErrorCallBack(result,e);
			}
							
		}


		//Sender
		public /*override*/ void BeginWrite(byte[] data, AsyncNetworkCallBack WriteComplete,Int32 TimeOut)
		{
			base.BeginWrite(data,WriteComplete);
			Int32 length = this.buffer.Read(this._buffer,0,_bufferSize);
			this.socket.SendTimeout = TimeOut;
			this.socket.GetStream().BeginWrite(this._buffer,0,length,this.AsyncWriteCallback,null);  
			this._timeout = TimeOut;
			this.monitor.Change(TimeOut,System.Threading.Timeout.Infinite);
		}


		//Write Callback
		protected override void WriteCallBack(IAsyncResult result)
		{
			this.monitor.Change(System.Threading.Timeout.Infinite,System.Threading.Timeout.Infinite); 
			try
			{
				//if((int)((System.Net.Sockets.Socket)this.socket.GetType().GetField("m_ClientSocket",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic).GetValue(this.socket)).Handle==-1)
				//{
				//	throw new System.InvalidOperationException("Operation not allowed on non-connected sockets.");
				//}

				this.socket.GetStream().EndWrite(result); 
				if(this.buffer.Position != this.buffer.Length)
				{
					this.Clear(this._buffer); 
					
					Int32 length = this.buffer.Read(this._buffer,0,_bufferSize); 
					this.socket.GetStream().BeginWrite(this._buffer,0,length,this.AsyncWriteCallback,null);
					this.monitor.Change(this._timeout,System.Threading.Timeout.Infinite);
				}
				else
				{	
					base.WriteCallBack (result);
				}
			}
			catch(System.Exception e)
			{
				base.ErrorCallBack(result,e);				
			}
		
		}


		//Finalize/Dispose
		public override void Dispose()
		{
			if(this.State != AsyncNetworkState.Disposed)
			{
				if(((System.Net.Sockets.Socket)((AsyncTcpClientAdapter)this).socket.GetType().GetField("m_ClientSocket",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic).GetValue(((AsyncTcpClientAdapter)this).socket)).Connected)
					this.socket.GetStream().Close(); 
				this.socket.Close();
				this.socket = null;
				base.Dispose ();
			}
		}

		~AsyncTcpClientAdapter()
		{
			this.Dispose();
		}
	}
}
