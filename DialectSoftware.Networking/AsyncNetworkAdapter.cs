using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace DialectSoftware.Networking
{
	/// <summary>
	/// Summary description for AsyncNetWorkAdapter.
	/// </summary>
	public delegate void AsyncNetworkCallBack(AsyncNetworkAdapter adapter);
	public delegate void AsyncNetworkErrorCallBack(AsyncNetworkAdapter adapter,System.Exception e);

    public abstract class AsyncNetworkAdapter:System.IDisposable 
	{

		protected readonly Int32 _bufferSize = 1024;

		private event AsyncNetworkCallBack EndRead; 
		private event AsyncNetworkCallBack EndWrite; 
		private event AsyncNetworkErrorCallBack EndError; 

		protected byte[] _buffer = null;
		protected System.IO.MemoryStream buffer = null;
		protected AsyncNetworkState state = AsyncNetworkState.Idle;
		
		public AsyncNetworkAdapter(AsyncNetworkErrorCallBack Error)
		{
			//
			// TODO: Add constructor logic here
			//
			this.EndError = Error;
			this.buffer = new MemoryStream();
			this._buffer = new byte[_bufferSize];
		}
		
		public byte[] Buffer
		{
			get
			{
				return (byte[])this.buffer.ToArray();
			}
		
		}

		public AsyncNetworkState State
		{
			get{return this.state;}			
		}

		public AsyncNetworkStatus Status
		{
			get{
				return (AsyncNetworkStatus)(System.Convert.ToInt32(this.EndRead!=null))+((System.Convert.ToInt32(this.EndWrite!=null))<<1); 
			}			
		}

		protected void Clear(System.IO.MemoryStream Stream)
		{
			if(Stream!=null)
			{
				Stream.Flush();
				System.Array.Clear(Stream.GetBuffer(),0,Stream.GetBuffer().Length);
				Stream.Seek(0,System.IO.SeekOrigin.Begin) ; 
				Stream.GetType().GetField("_length",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic).SetValue(Stream,0);
			}
		}

		protected void Clear(byte[] buffer)
		{
			System.Array.Clear(buffer,0,buffer.Length);
		}


		//returns AsyncCallback objects of Callback Functions
		protected abstract System.AsyncCallback AsyncReadCallback
		{
			get;
		}

		protected abstract System.AsyncCallback AsyncWriteCallback
		{
			get;
		}

		
		//returns AsyncCallback objects of End Async Event Invocation Functions
		protected System.AsyncCallback EndReadInvoke
		{
			get{return new System.AsyncCallback(EndAsyncCallBack);}
		}

		protected System.AsyncCallback EndWriteInvoke
		{
			get{return new System.AsyncCallback(EndAsyncCallBack);}
		}

		protected System.AsyncCallback EndErrorInvoke
		{
			get{return new System.AsyncCallback(EndAsyncErrorCallBack);}
		}


		//Stream Read Functions
		protected virtual void BeginRead(AsyncNetworkCallBack ReadComplete)
		{
			if(this.state != AsyncNetworkState.Idle)
				throw new System.InvalidOperationException(this.state.ToString());

			if(this.EndRead != null)
				throw new InvalidOperationException("BeginRead can be called only once for each AsyncNetworkAdapter object.");

			if(ReadComplete == null)
				throw new NullReferenceException("ReadComplete parameter cannot be null.");

			this.EndRead = ReadComplete;
			this.Clear(this.buffer);
			this.Clear(this._buffer);
 
			this.state = AsyncNetworkState.Reading; 
		}

		protected virtual void Read()
		{
			if(this.state != AsyncNetworkState.Idle)
				throw new System.InvalidOperationException(this.state.ToString());

			if(this.EndRead == null)
				throw new InvalidOperationException("BeginRead must be called first for each AsyncNetworkAdapter object.");
			
			this.Clear(this._buffer); 
			this.state = AsyncNetworkState.Reading; 
		}


		//Stream Write Functions
		protected virtual void BeginWrite(byte[] data, AsyncNetworkCallBack WriteComplete)
		{
			if(this.state != AsyncNetworkState.Idle)
				throw new System.InvalidOperationException(this.state.ToString());

			if(data == null)
				throw new NullReferenceException("byte[] data parameter cannot be null.");

			if(WriteComplete == null)
				throw new NullReferenceException("WriteComplete parameter cannot be null.");

			
			this.EndWrite = WriteComplete;
			this.Clear(this.buffer);
			this.Clear(this._buffer); 

			this.buffer.SetLength(data.Length);  
			data.CopyTo(this.buffer.GetBuffer(),0);  
			this.buffer.Seek(0,System.IO.SeekOrigin.Begin);
			this.state = AsyncNetworkState.Writing; 
		}


		//Callback Functions
		protected virtual void ReadCallBack(System.IAsyncResult result)
		{
			this.state = AsyncNetworkState.Idle;
			this.EndRead.BeginInvoke(this,this.EndReadInvoke,null);
		}

		protected virtual void WriteCallBack(System.IAsyncResult result)
		{
			this.Clear(this._buffer); 
			this.Clear(this.buffer); 
			this.state = AsyncNetworkState.Idle;
			this.EndWrite.BeginInvoke(this,this.EndWriteInvoke,null); 
		}

		protected virtual void ErrorCallBack(System.IAsyncResult result,System.Exception e)
		{
			this.Clear(this._buffer); 
			this.Clear(this.buffer); 
			this.state = AsyncNetworkState.Error;
			this.EndError.BeginInvoke(this,e,this.EndErrorInvoke,this);
		}


		//End Async Event Invocation Functions
		private static void EndAsyncCallBack(System.IAsyncResult result)
		{
            try 
	        {	        
		        if(!((AsyncResult)result).EndInvokeCalled )
			            ((AsyncNetworkCallBack)((AsyncResult)result).AsyncDelegate).EndInvoke(result); 
	        }
	        catch
	        {
                throw;
	        }
            
		}

		private static void EndAsyncErrorCallBack(System.IAsyncResult result)
		{
            try
            {
                if (!((AsyncResult)result).EndInvokeCalled)
                {
                    ((AsyncNetworkErrorCallBack)((AsyncResult)result).AsyncDelegate).EndInvoke(result);
                    ((AsyncNetworkAdapter)result.AsyncState).Dispose();
                }
            }
	        catch //(Exception e)
	        {
                throw;
                //endless loop((AsyncSocketAdapter)result).ErrorCallBack(result, e);
	        }
		}


		//Finalize/Dispose
		public virtual void Dispose()
		{			
			if(this.State != AsyncNetworkState.Disposed)
			{
                this.buffer.Close();
				this.buffer = null;
				GC.SuppressFinalize(this);
				this.state = AsyncNetworkState.Disposed;
			}

		}

    	~AsyncNetworkAdapter()
		{
			this.Dispose();
		}
	}
}
