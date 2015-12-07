using System;

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