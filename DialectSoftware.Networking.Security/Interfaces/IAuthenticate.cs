/// ******************************************************************************************************************
/// * Copyright (c) 2011 Dialect Software LLC                                                                        *
/// * This software is distributed under the terms of the Apache License http://www.apache.org/licenses/LICENSE-2.0  *
/// *                                                                                                                *
/// ******************************************************************************************************************

namespace DialectSoftware.Networking.Security.Authentication
{
	public interface IAuthenticate
	{
		SSPI.SECURITY_STATUS Status
		{
			get;
		}

		byte[] Authenticate();
		byte[] Authenticate(byte[] Token);
			
	}
}