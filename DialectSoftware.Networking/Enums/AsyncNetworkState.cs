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
	public enum AsyncNetworkState
	{
		Idle = 0,
		Reading = 1,
		Writing = 2,
		Disposed = 3,
		Error = 4
	}

}
