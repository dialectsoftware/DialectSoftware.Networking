using System;

/// ******************************************************************************************************************
/// * Copyright (c) 2011 Dialect Software LLC                                                                        *
/// * This software is distributed under the terms of the Apache License http://www.apache.org/licenses/LICENSE-2.0  *
/// *                                                                                                                *
/// ******************************************************************************************************************

namespace DialectSoftware.Networking.Controls
{
	
	public enum Status:int
	{
		Idle = 0,
		Connecting  = 1,
		Authenticating = 2,
		Negotiating = 3,
		Sending = 4,
		Receiving = 5,
		Terminating = 6
	}
}