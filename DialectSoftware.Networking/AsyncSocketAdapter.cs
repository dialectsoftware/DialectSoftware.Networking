using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;

namespace DialectSoftware.Networking
{
	/// <summary>
	/// Summary description for DynamicDiscovery.
	/// </summary>
	public class AsyncSocketAdapter:AsyncNetworkAdapter
	{

		private Socket socket = null;
	    private EndPoint remoteEP = null;
		
		private System.Threading.Timer monitor = null;
		private Int32 _timeout = System.Threading.Timeout.Infinite;

		public AsyncSocketAdapter(Socket socket,AsyncNetworkErrorCallBack Error):base(Error)
		{
			//
			// TODO: Add constructor logic here
			//
			this.socket = socket;
			this.monitor = new System.Threading.Timer(new System.Threading.TimerCallback(Expire),this,_timeout,_timeout);    
			
		}

        public int TimeOut
        {
            get { return _timeout;  }
        }

		public EndPoint RemoteAddress
		{
			get{return this.remoteEP;}
		}

		public int DataAvailable
		{
			get{return this.socket.Available;}
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
			((AsyncSocketAdapter)state).monitor.Change(System.Threading.Timeout.Infinite,System.Threading.Timeout.Infinite); 
			
			if(((AsyncSocketAdapter)state).socket == null)
				return;
			else if(((AsyncSocketAdapter)state).socket.Available == 0)
				((AsyncSocketAdapter)state).socket.Close();
			else
			{
				Int32 TimeOut =(Int32)((AsyncSocketAdapter)state).GetType().GetField("_timeout",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic).GetValue(state);
				((AsyncSocketAdapter)state).monitor.Change(TimeOut,System.Threading.Timeout.Infinite); 
			}
				
		}


		//Listener
		public /*override*/ void BeginRead(AsyncNetworkCallBack ReadComplete,Int32 TimeOut)
		{
			base.BeginRead(ReadComplete);
			this.socket.BeginReceive(this._buffer,0,_bufferSize,System.Net.Sockets.SocketFlags.None,this.AsyncReadCallback,null);
			this._timeout = TimeOut;
			this.monitor.Change(TimeOut,System.Threading.Timeout.Infinite);
		}

		public void BeginRead(long Length,AsyncNetworkCallBack ReadComplete, Int32 TimeOut)
		{
			base.BeginRead(ReadComplete);
			this.socket.BeginReceive(this._buffer,0,((Int64)this._bufferSize >= Length)? (Int32)Length:this._bufferSize,System.Net.Sockets.SocketFlags.None,this.AsyncReadCallback,Length);
			this._timeout = TimeOut;
			this.monitor.Change(TimeOut,System.Threading.Timeout.Infinite);
		}

		public void BeginRead(ref EndPoint remoteEP,AsyncNetworkCallBack ReadComplete, Int32 TimeOut)
		{
			base.BeginRead(ReadComplete);
			this.remoteEP = remoteEP;
            this.socket.BeginReceiveFrom(this._buffer,0,_bufferSize,System.Net.Sockets.SocketFlags.None,ref remoteEP,this.AsyncReadCallback,null);
			this._timeout = TimeOut;
			this.monitor.Change(TimeOut,System.Threading.Timeout.Infinite);
		}

		public void BeginRead(long Length,ref EndPoint remoteEP,AsyncNetworkCallBack ReadComplete, Int32 TimeOut)
		{
			base.BeginRead(ReadComplete);
			this.remoteEP = remoteEP;
			this.socket.BeginReceiveFrom(this._buffer,0,((Int64)this._bufferSize >= Length)? (Int32)Length:this._bufferSize,System.Net.Sockets.SocketFlags.None,ref remoteEP,this.AsyncReadCallback,Length);
			this._timeout = TimeOut;
			this.monitor.Change(TimeOut,System.Threading.Timeout.Infinite);
		}

		public /*override*/  void Read(Int32 TimeOut)
		{
			base.Read();
			this.socket.BeginReceive(this._buffer,0,_bufferSize,System.Net.Sockets.SocketFlags.None,this.AsyncReadCallback,null);
			this._timeout = TimeOut;
			this.monitor.Change(TimeOut,System.Threading.Timeout.Infinite);
		}

		public void Read(long Length,Int32 TimeOut)
		{
			base.Read();
			this.socket.BeginReceive(this._buffer,0,((Int64)this._bufferSize >= Length)? (Int32)Length:this._bufferSize,System.Net.Sockets.SocketFlags.None,this.AsyncReadCallback,Length);
			this._timeout = TimeOut;
			this.monitor.Change(TimeOut,System.Threading.Timeout.Infinite);
		}

		public void Read(ref EndPoint remoteEP,Int32 TimeOut)
		{
			base.Read();
			this.remoteEP = remoteEP;
			this.socket.BeginReceiveFrom(this._buffer,0,_bufferSize,System.Net.Sockets.SocketFlags.None,ref remoteEP,this.AsyncReadCallback,null);
			this._timeout = TimeOut;
			this.monitor.Change(TimeOut,System.Threading.Timeout.Infinite);
		}

		public void Read(long Length,ref EndPoint remoteEP, Int32 TimeOut)
		{
			base.Read();
			this.remoteEP = remoteEP;
			this.socket.BeginReceiveFrom(this._buffer,0,((Int64)this._bufferSize >= Length)? (Int32)Length:this._bufferSize,System.Net.Sockets.SocketFlags.None,ref remoteEP,this.AsyncReadCallback,Length);
			this._timeout = TimeOut;
			this.monitor.Change(TimeOut,System.Threading.Timeout.Infinite);
		}


		//Read Callback
		protected override void ReadCallBack(IAsyncResult result)
		{
			this.monitor.Change(System.Threading.Timeout.Infinite,System.Threading.Timeout.Infinite);    
			try
			{
				//if((int)this.socket.Handle ==-1)
				//{
				//	throw new System.InvalidOperationException("Operation not allowed on non-connected sockets.");
				//}

				Int32 length = 0;
				if(this.remoteEP == null)
					length = this.socket.EndReceive(result);
				else
					length = this.socket.EndReceiveFrom(result,ref this.remoteEP);
				
				this.buffer.Write(this._buffer,0,length);
				this.buffer.Flush(); 
				this.Clear(this._buffer); 
		
				if(result.AsyncState != null)
				{
					Int64 Length = (Int64)result.AsyncState;
					Length -= length;
					if(Length > 0 )
					{
						if(this.remoteEP == null)
							this.socket.BeginReceive(this._buffer,0,((Int64)this._bufferSize >= Length)? (Int32)Length:this._bufferSize,System.Net.Sockets.SocketFlags.None,this.AsyncReadCallback,Length);
						else
							this.socket.BeginReceiveFrom(this._buffer,0,((Int64)this._bufferSize >= Length)? (Int32)Length:this._bufferSize,System.Net.Sockets.SocketFlags.None,ref this.remoteEP,this.AsyncReadCallback,Length);

						this.monitor.Change(this._timeout,System.Threading.Timeout.Infinite);
						return;
					}	
				}
				else
				{
					if(this.socket.Available > 0)
					{
						if(this.remoteEP == null)
							this.socket.BeginReceive(this._buffer,0,_bufferSize,System.Net.Sockets.SocketFlags.None,this.AsyncReadCallback,null);
						else
							this.socket.BeginReceiveFrom(this._buffer,0,_bufferSize,System.Net.Sockets.SocketFlags.None,ref this.remoteEP,this.AsyncReadCallback,null);
						
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
		public /*override*/  void BeginWrite(byte[] data, AsyncNetworkCallBack WriteComplete, Int32 TimeOut)
		{
			base.BeginWrite(data,WriteComplete);

			Int32 length = this.buffer.Read(this._buffer,0,_bufferSize);
			this.socket.BeginSend(this._buffer,0,length,System.Net.Sockets.SocketFlags.None,this.AsyncWriteCallback,null);  
			this._timeout = TimeOut;
			this.monitor.Change(TimeOut,System.Threading.Timeout.Infinite);
		}

		public void BeginWrite(EndPoint remoteEP,byte[] data, AsyncNetworkCallBack WriteComplete, Int32 TimeOut)
		{
			base.BeginWrite(data,WriteComplete);

			this.remoteEP = remoteEP;
			Int32 length = this.buffer.Read(this._buffer,0,_bufferSize);
			this.socket.BeginSendTo(this._buffer,0,length,System.Net.Sockets.SocketFlags.None,remoteEP,this.AsyncWriteCallback,null);  
			this._timeout = TimeOut;
			this.monitor.Change(TimeOut,System.Threading.Timeout.Infinite);
		}


		//Write Callback
		protected override void WriteCallBack(IAsyncResult result)
		{
			this.monitor.Change(System.Threading.Timeout.Infinite,System.Threading.Timeout.Infinite); 
			try
			{
				//if((int)this.socket.Handle ==-1)
				//{
				//	throw new System.InvalidOperationException("Operation not allowed on non-connected sockets.");
				//}
				
				if(this.remoteEP == null)
					this.socket.EndSend(result);
				else
					this.socket.EndSendTo(result);  

				if(this.buffer.Position != this.buffer.Length)
				{
					this.Clear(this._buffer); 
					Int32 length = this.buffer.Read(this._buffer,0,_bufferSize); 
					if(this.remoteEP == null)
						this.socket.BeginSend(this._buffer,0,length,System.Net.Sockets.SocketFlags.None,this.AsyncWriteCallback,null);  
					else
						this.socket.BeginSendTo(this._buffer,0,length,System.Net.Sockets.SocketFlags.None,this.remoteEP,this.AsyncWriteCallback,null);  
					
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
				if((int)this.socket.Handle!= -1)
					this.socket.Close();
				this.socket = null;
				this.remoteEP = null;
				base.Dispose ();
			}
		}

		~AsyncSocketAdapter()
		{
			this.Dispose();
		}
	}
}
