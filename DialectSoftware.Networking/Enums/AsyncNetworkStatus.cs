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
	public enum AsyncNetworkStatus
	{
		None = 0,
		Read = 1,
		Write = 2,
		ReadWrite = 3
	}

}
