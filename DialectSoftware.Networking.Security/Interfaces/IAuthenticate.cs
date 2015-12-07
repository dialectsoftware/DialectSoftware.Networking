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