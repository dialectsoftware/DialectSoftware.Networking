using System;
using System.Runtime.InteropServices;

/// ******************************************************************************************************************
/// * Copyright (c) 2011 Dialect Software LLC                                                                        *
/// * This software is distributed under the terms of the Apache License http://www.apache.org/licenses/LICENSE-2.0  *
/// *                                                                                                                *
/// ******************************************************************************************************************

namespace DialectSoftware.Networking.Security
{
	/// <summary>
	/// Summary description for NetworkManagement.
	/// </summary>
	namespace Authorization
	{

		public enum NET_API_STATUS:uint
		{
			NERR_Success  = 0,
			NERR_BASE =   2100,
			NERR_UserNotFound  =    (NERR_BASE+121), 
			NERR_InvalidComputer =  (NERR_BASE+251),
			ERROR_ACCESS_DENIED = 5
			
		}

		public enum USER_INFO:uint
		{
			USER_INFO_0 = 0,
			USER_INFO_1 = 1,
			USER_INFO_2 = 2,
			USER_INFO_3 = 3,
			USER_INFO_4 = 4,
			USER_INFO_10 = 10
		}


		[StructLayout(LayoutKind.Explicit)]
		struct _USER_INFO_0
		{
			[FieldOffset(0)]public IntPtr usri0_name;

			public IntPtr ToPtr()
			{
				return (IntPtr)this;
			}

			public static _USER_INFO_0 ToStruct(IntPtr Ptr)
			{
				return (_USER_INFO_0)Ptr;
			}

			public static implicit operator _USER_INFO_0(IntPtr Ptr) 
			{
				_USER_INFO_0 value = (_USER_INFO_0)System.Runtime.InteropServices.Marshal.PtrToStructure(Ptr, 
					typeof(_USER_INFO_0));
				Marshal.DestroyStructure(Ptr,typeof(_USER_INFO_0));
				return value;
			}

			public static implicit operator IntPtr(_USER_INFO_0 value) 
			{			
				IntPtr Ptr =  System.Runtime.InteropServices.Marshal.AllocCoTaskMem(System.Runtime.InteropServices.Marshal.SizeOf(value));
				System.Runtime.InteropServices.Marshal.StructureToPtr(value,Ptr,false);
				return Ptr;
			}

		} 


		[StructLayout(LayoutKind.Explicit)]
		struct _USER_INFO_10 
		{
			[FieldOffset(0)]public IntPtr usri10_name;  
			[FieldOffset(4)]public IntPtr usri10_comment;
			[FieldOffset(8)]public IntPtr usri10_usr_comment;
			[FieldOffset(12)]public IntPtr usri10_full_name;


			public IntPtr ToPtr()
			{
				return (IntPtr)this;
			}

			public static _USER_INFO_10 ToStruct(IntPtr Ptr)
			{
				return (_USER_INFO_10)Ptr;
			}

			public static implicit operator _USER_INFO_10(IntPtr Ptr) 
			{
				_USER_INFO_10 value = (_USER_INFO_10)System.Runtime.InteropServices.Marshal.PtrToStructure(Ptr, 
					typeof(_USER_INFO_10));
				Marshal.DestroyStructure(Ptr,typeof(_USER_INFO_10));
				return value;
			}

			public static implicit operator IntPtr(_USER_INFO_10 value) 
			{			
				IntPtr Ptr =  System.Runtime.InteropServices.Marshal.AllocCoTaskMem(System.Runtime.InteropServices.Marshal.SizeOf(value));
				System.Runtime.InteropServices.Marshal.StructureToPtr(value,Ptr,false);
				return Ptr;
			}
		} 


		[StructLayout(LayoutKind.Explicit)]
		public struct USER_INFO_0
		{
			[FieldOffset(0)]public string user_name;

		} 


		[StructLayout(LayoutKind.Explicit)]
		public struct USER_INFO_10 
		{
			[FieldOffset(0)]public string user_name;  
			[FieldOffset(4)]public string user_comment;
			[FieldOffset(8)]public string user_usr_comment;
			[FieldOffset(12)]public string user_full_name;
		} 


		public class NETAPI
		{
			//
			// TODO: Add constructor logic here
			//
			[DllImport(@"C:\Winnt\System32\Netapi32.dll")]
			public static extern NET_API_STATUS NetApiBufferFree(
				IntPtr pBuffer
				);


			[DllImport(@"C:\Winnt\System32\Netapi32.dll")]
			public static extern NET_API_STATUS NetUserGetInfo(
			[In]IntPtr /*string*/ servername,
			[In]IntPtr /*string*/ username,
			[In]USER_INFO level,
			[Out]out IntPtr pBuffer
			);

			public static NET_API_STATUS NetUserGetInfo(
				[In]string servername,
				[In]string username,
				[In]USER_INFO level,
				[Out]out USER_INFO_0 Buffer
				)
			{

				IntPtr pbuffer; 
				Buffer = new USER_INFO_0();
				IntPtr pUserName = System.Runtime.InteropServices.Marshal.StringToHGlobalUni(username);	
				IntPtr pServerName = System.Runtime.InteropServices.Marshal.StringToHGlobalUni(servername);				
				
				Authorization.NET_API_STATUS status = Authorization.NETAPI.NetUserGetInfo(pServerName,pUserName,level,out pbuffer);  
				
				System.Runtime.InteropServices.Marshal.FreeHGlobal(pUserName);
				System.Runtime.InteropServices.Marshal.FreeHGlobal(pServerName);
			
								
				if(status == Authorization.NET_API_STATUS.NERR_Success)
				{
					Authorization._USER_INFO_0 _buffer = pbuffer;
					Buffer.user_name = System.Runtime.InteropServices.Marshal.PtrToStringUni(_buffer.usri0_name);
					status = NetApiBufferFree(_buffer.usri0_name);
				}

				return status;
			
			}

			public static NET_API_STATUS NetUserGetInfo(
				[In]string servername,
				[In]string username,
				[In]USER_INFO level,
				[Out]out USER_INFO_10 Buffer
				)
			{

				IntPtr pbuffer; 
				Buffer = new USER_INFO_10();
				IntPtr pUserName = System.Runtime.InteropServices.Marshal.StringToHGlobalUni(username);	
				IntPtr pServerName = System.Runtime.InteropServices.Marshal.StringToHGlobalUni(servername);	
			
				Authorization.NET_API_STATUS status = Authorization.NETAPI.NetUserGetInfo(pServerName,pUserName,level,out pbuffer);  

				System.Runtime.InteropServices.Marshal.FreeHGlobal(pUserName);
				System.Runtime.InteropServices.Marshal.FreeHGlobal(pServerName);
                
				if(status == Authorization.NET_API_STATUS.NERR_Success)
				{
					Authorization._USER_INFO_10 _buffer = pbuffer;

					Buffer.user_name = System.Runtime.InteropServices.Marshal.PtrToStringUni(_buffer.usri10_name);
					status = NetApiBufferFree(_buffer.usri10_name);
					
					Buffer.user_comment = System.Runtime.InteropServices.Marshal.PtrToStringUni(_buffer.usri10_comment);
					status = NetApiBufferFree(_buffer.usri10_comment);
					
					Buffer.user_usr_comment = System.Runtime.InteropServices.Marshal.PtrToStringUni(_buffer.usri10_usr_comment);
					status = NetApiBufferFree(_buffer.usri10_usr_comment);

					Buffer.user_full_name  = System.Runtime.InteropServices.Marshal.PtrToStringUni(_buffer.usri10_full_name);
					status = NetApiBufferFree(_buffer.usri10_full_name);

				}

				return status;
			
			}

			

		}
	}
}
