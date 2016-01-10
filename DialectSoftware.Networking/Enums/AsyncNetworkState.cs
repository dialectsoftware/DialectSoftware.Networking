using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

/// ******************************************************************************************************************
/// * Copyright (c) 2011 Dialect Software LLC                                                                        *
/// * This software is distributed under the terms of the Apache License http://www.apache.org/licenses/LICENSE-2.0  *
/// *                                                                                                                *
/// ******************************************************************************************************************

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
