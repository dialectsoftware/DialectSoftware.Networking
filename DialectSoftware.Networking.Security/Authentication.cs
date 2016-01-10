using System;
using DialectSoftware.Networking; 
using System.Runtime.InteropServices;

/// ******************************************************************************************************************
/// * Copyright (c) 2011 Dialect Software LLC                                                                        *
/// * This software is distributed under the terms of the Apache License http://www.apache.org/licenses/LICENSE-2.0  *
/// *                                                                                                                *
/// ******************************************************************************************************************

namespace DialectSoftware.Networking.Security	
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	namespace Authentication
	{
		

		namespace SSPI
		{

			/// <summary>
			/// /////////////////////INTREFACES////////////////////////////
			/// </summary>
	
			public interface SSPIAuthenticate:IAuthenticate
			{
				/*SECURITY_STATUS State
				{
					get;
				}

				byte[] Authenticate();
				byte[] Authenticate(byte[] Token);*/
		
			}

			/// <summary>
			/// /////////////////////STRUCTS////////////////////////////
			/// </summary>
	

			/// <summary>
			/// /////////////////////SecPkgInfo////////////////////////////
			/// </summary>
			[StructLayout(LayoutKind.Explicit)]
			public struct SecPkgInfo
			{
				[FieldOffset(0)]public int Capabilities;       // Capability bitmask
				[FieldOffset(4)]public ushort wVersion;         // Version of driver
				[FieldOffset(6)]public ushort wRPCID;           // ID for RPC Runtime
				[FieldOffset(8)]public int cbMaxToken;          // Size of authentication token (max)
				[FieldOffset(12)]public String  Name;			// Text name
				[FieldOffset(16)]public String Comment;			//Comment


				public IntPtr ToPtr()
				{
					return (IntPtr)this;
				}

				public static SecPkgInfo ToStruct(IntPtr Ptr)
				{
					return (SecPkgInfo)Ptr;
				}

				public static implicit operator SecPkgInfo(IntPtr Ptr) 
				{
					SecPkgInfo value = (SSPI.SecPkgInfo)System.Runtime.InteropServices.Marshal.PtrToStructure(Ptr, 
						typeof(Authentication.SSPI.SecPkgInfo));
					Marshal.DestroyStructure(Ptr,typeof(Authentication.SSPI.SecPkgInfo));
					return value;
				}

				public static implicit operator IntPtr(SecPkgInfo value) 
				{			
					IntPtr Ptr =  System.Runtime.InteropServices.Marshal.AllocCoTaskMem(System.Runtime.InteropServices.Marshal.SizeOf(value));
					System.Runtime.InteropServices.Marshal.StructureToPtr(value,Ptr,false);
					return Ptr;
				}

			}


			/// ///////////////////////////////LUID////////////////////////
			[StructLayout(LayoutKind.Explicit)]
			public struct LUID 
			{
				[FieldOffset(0)]uint LowPart;
				[FieldOffset(4)]int HighPart;

				
				public IntPtr ToPtr()
				{
					return (IntPtr)this;
				}

				public static LUID ToStruct(IntPtr Ptr)
				{
					return (LUID)Ptr;
				}

				public static implicit operator LUID(IntPtr Ptr) 
				{
					LUID value = (Authentication.SSPI.LUID)System.Runtime.InteropServices.Marshal.PtrToStructure(Ptr, 
						typeof(Authentication.SSPI.LUID));
					Marshal.DestroyStructure(Ptr,typeof(Authentication.SSPI.LUID));
					return value;
				}

				public static implicit operator IntPtr(LUID value) 
				{			
					IntPtr Ptr =  System.Runtime.InteropServices.Marshal.AllocCoTaskMem( System.Runtime.InteropServices.Marshal.SizeOf(value));
					System.Runtime.InteropServices.Marshal.StructureToPtr(value,Ptr,false);
					return Ptr;
				}
			} 


			/// ///////////////////////////////SEC_WINNT_AUTH_IDENTITY////////////////////////
			[StructLayout(LayoutKind.Explicit)]
			public struct Identity /*SEC_WINNT_AUTH_IDENTITY */
			{
		
				[FieldOffset(0)] public  String User;
				[FieldOffset(4)] public  uint UserLength;
				[FieldOffset(8)] public  String Domain;
				[FieldOffset(12)] public uint DomainLength;
				[FieldOffset(16)]public  String Password;
				[FieldOffset(20)]public  uint PasswordLength;
				[FieldOffset(24)]public  AuthIdentityFlags Flags;


				public Identity (string domain,string user,string password,AuthIdentityFlags flags)
				{
					this.User = user;
					this.UserLength = (uint)user.Length;
					this.Domain = domain;
					this.DomainLength = (uint)domain.Length;
					this.Password = password;
					this.PasswordLength = (uint)password.Length;
					this.Flags = flags; 
				}

				public IntPtr ToPtr()
				{
					return (IntPtr)this;
				}

				public static SSPI.Identity ToStruct(IntPtr Ptr)
				{
					return (SSPI.Identity)Ptr;
				}

				public static implicit operator SSPI.Identity(IntPtr Ptr) 
				{
					SSPI.Identity value = (Authentication.SSPI.Identity)System.Runtime.InteropServices.Marshal.PtrToStructure(Ptr, 
						typeof(Authentication.SSPI.Identity));
					Marshal.DestroyStructure(Ptr,typeof(Authentication.SSPI.Identity));
					return value;
				}

				public static implicit operator IntPtr(SSPI.Identity value) 
				{			
					IntPtr Ptr =  System.Runtime.InteropServices.Marshal.AllocCoTaskMem(System.Runtime.InteropServices.Marshal.SizeOf(value));
					System.Runtime.InteropServices.Marshal.StructureToPtr(value,Ptr,false);
					return Ptr;
				}
			} 


			///////////////////////////////SecHandle////////////////////////
			[StructLayout(LayoutKind.Explicit)]
			public struct SecHandle
			{
				[FieldOffset(0)]uint  dwLower;
				[FieldOffset(4)]uint  dwUpper;

					
				public IntPtr ToPtr()
				{
					return (IntPtr)this;
				}

				public static SecHandle ToStruct(IntPtr Ptr)
				{
					return (SecHandle)Ptr;
				}

				public static implicit operator SecHandle(IntPtr Ptr) 
				{
					SecHandle value = (Authentication.SSPI.SecHandle)System.Runtime.InteropServices.Marshal.PtrToStructure(Ptr, 
						typeof(Authentication.SSPI.SecHandle));
					Marshal.DestroyStructure(Ptr,typeof(Authentication.SSPI.SecHandle));
					return value;
				}

				public static implicit operator IntPtr(SecHandle value) 
				{			
					IntPtr Ptr =  System.Runtime.InteropServices.Marshal.AllocCoTaskMem( System.Runtime.InteropServices.Marshal.SizeOf(value));
					System.Runtime.InteropServices.Marshal.StructureToPtr(value,Ptr,false);
					return Ptr;
				}

			}


			/// <summary>
			/// ///////////////////////////////TimeStamp////////////////////////
			/// </summary>
			[StructLayout(LayoutKind.Explicit)]
			public struct _Part
			{	
				[FieldOffset(0)]uint  dwLower;
				[FieldOffset(4)]int  dwUpper;
			
			}


			[StructLayout(LayoutKind.Explicit)]
			public struct TimeStamp 
			{
				[FieldOffset(0)]_Part LowPart;
				[FieldOffset(8)]_Part HightPart;
				[FieldOffset(16)]Int64 QuadPart;

				public IntPtr ToPtr()
				{
					return (IntPtr)this;
				}

				public static TimeStamp ToStruct(IntPtr Ptr)
				{
					return (TimeStamp)Ptr;
				}

				public static implicit operator TimeStamp(IntPtr Ptr) 
				{
					TimeStamp value = (Authentication.SSPI.TimeStamp)System.Runtime.InteropServices.Marshal.PtrToStructure(Ptr, 
						typeof( Authentication.SSPI.TimeStamp));
					Marshal.DestroyStructure(Ptr,typeof(Authentication.SSPI.TimeStamp));
					return value;
				}

				public static implicit operator IntPtr(TimeStamp value) 
				{			
					IntPtr Ptr =  System.Runtime.InteropServices.Marshal.AllocCoTaskMem( System.Runtime.InteropServices.Marshal.SizeOf(value));
					System.Runtime.InteropServices.Marshal.StructureToPtr(value,Ptr,false);
					return Ptr;
				}

			}


			/// ///////////////////////////////SecPkgContext_Names////////////////////////
			[StructLayout(LayoutKind.Explicit)]
			public struct SecPkgContext_Names 
			{
				[FieldOffset(0)]public string UserName;

				public IntPtr ToPtr()
				{
					return (IntPtr)this;
				}

				public static SecPkgContext_Names ToStruct(IntPtr Ptr)
				{
					return (SecPkgContext_Names)Ptr;
				}

				public static implicit operator SecPkgContext_Names(IntPtr Ptr) 
				{
					SecPkgContext_Names value = (Authentication.SSPI.SecPkgContext_Names)System.Runtime.InteropServices.Marshal.PtrToStructure(Ptr, 
						typeof( Authentication.SSPI.SecPkgContext_Names));
					Marshal.DestroyStructure(Ptr,typeof(Authentication.SSPI.SecPkgContext_Names));
					return value;
				}

				public static implicit operator IntPtr(SecPkgContext_Names value) 
				{			
					IntPtr Ptr =  System.Runtime.InteropServices.Marshal.AllocCoTaskMem( System.Runtime.InteropServices.Marshal.SizeOf(value));
					System.Runtime.InteropServices.Marshal.StructureToPtr(value,Ptr,false);
					return Ptr;
				}

			} 


			/// ///////////////////////////////SecBuffer ////////////////////////
			[StructLayout(LayoutKind.Explicit)]
			public struct SecBuffer 
			{
				[FieldOffset(0)]public uint cbBuffer;    // Size of the buffer, in bytes
				[FieldOffset(4)]public SecBufferFlags BufferType; // Type of the buffer (below)
				[FieldOffset(8)]public /*byte[]*/ IntPtr Buffer; // Pointer to the buffer

				public IntPtr ToPtr()
				{
					return (IntPtr)this;
				}

				public static SecBuffer ToStruct(IntPtr Ptr)
				{
					return (SecBuffer)Ptr;
				}

				public static implicit operator SecBuffer(IntPtr Ptr) 
				{
					SecBuffer value = (Authentication.SSPI.SecBuffer)System.Runtime.InteropServices.Marshal.PtrToStructure(Ptr, 
						typeof(Authentication.SSPI.SecBuffer));
					Marshal.DestroyStructure(Ptr,typeof(Authentication.SSPI.SecBuffer));
					return value;
				}

				public static implicit operator IntPtr(SecBuffer value) 
				{			
					IntPtr Ptr =  System.Runtime.InteropServices.Marshal.AllocCoTaskMem(System.Runtime.InteropServices.Marshal.SizeOf(value));
					System.Runtime.InteropServices.Marshal.StructureToPtr(value,Ptr,false);
					return Ptr;
				}

			}	


			/// ///////////////////////////////SecBufferDesc ////////////////////////
			[StructLayout(LayoutKind.Explicit)]
			public struct SecBufferDesc 
			{
				[FieldOffset(0)]public SecBufferFlags ulVersion;// Version number
				[FieldOffset(4)]public uint cBuffers;// Number of buffers
				[FieldOffset(8)]public /*SecBuffer*/ IntPtr pSecBuffers;// Pointer to array of buffers
					
				public IntPtr ToPtr()
				{
					return (IntPtr)this;
				}

				public static SecBufferDesc ToStruct(IntPtr Ptr)
				{
					return (SecBufferDesc)Ptr;
				}

				public static implicit operator SecBufferDesc(IntPtr Ptr) 
				{
					SecBufferDesc value = (Authentication.SSPI.SecBufferDesc)System.Runtime.InteropServices.Marshal.PtrToStructure(Ptr, 
						typeof( Authentication.SSPI.SecBufferDesc));
					Marshal.DestroyStructure(Ptr,typeof(Authentication.SSPI.SecBufferDesc));
					return value;
				}

				public static implicit operator IntPtr(SecBufferDesc value) 
				{			
					IntPtr Ptr =  System.Runtime.InteropServices.Marshal.AllocCoTaskMem( System.Runtime.InteropServices.Marshal.SizeOf(value));
					System.Runtime.InteropServices.Marshal.StructureToPtr(value,Ptr,false);
					return Ptr;
				}
			} 


			/// ///////////////////////////////ENUMS////////////////////////
			
			public enum AuthIdentityFlags:uint
			{
				SEC_WINNT_AUTH_IDENTITY_ANSI =    0x00000001,
				SEC_WINNT_AUTH_IDENTITY_UNICODE = 0x00000002
			}

			public enum CredentialUseFlags:uint
			{
				//
				SECPKG_CRED_INBOUND =  0x00000001,
				SECPKG_CRED_OUTBOUND = 0x00000002,
				SECPKG_CRED_BOTH =     0x00000003,
				SECPKG_CRED_DEFAULT =  0x00000004,
				SECPKG_CRED_RESERVED = 0xF0000000	
			}

			public enum SecurityCredentialsAttribute:int
			{
				SECPKG_CRED_ATTR_NAMES = 1
			}

			public enum SecurityContextRequirement:uint
			{
				
				ISC_REQ_DELEGATE                =0x00000001,
				ISC_REQ_MUTUAL_AUTH             =0x00000002,
				ISC_REQ_REPLAY_DETECT           =0x00000004,
				ISC_REQ_SEQUENCE_DETECT         =0x00000008,
				ISC_REQ_CONFIDENTIALITY         =0x00000010,
				ISC_REQ_USE_SESSION_KEY         =0x00000020,
				ISC_REQ_PROMPT_FOR_CREDS        =0x00000040,
				ISC_REQ_USE_SUPPLIED_CREDS      =0x00000080,
				ISC_REQ_ALLOCATE_MEMORY         =0x00000100,
				ISC_REQ_USE_DCE_STYLE           =0x00000200,
				ISC_REQ_DATAGRAM                =0x00000400,
				ISC_REQ_CONNECTION              =0x00000800,
				ISC_REQ_CALL_LEVEL              =0x00001000,
				ISC_REQ_FRAGMENT_SUPPLIED       =0x00002000,
				ISC_REQ_EXTENDED_ERROR          =0x00004000,
				ISC_REQ_STREAM                  =0x00008000,
				ISC_REQ_INTEGRITY               =0x00010000,
				ISC_REQ_IDENTIFY                =0x00020000,
				ISC_REQ_NULL_SESSION            =0x00040000,
				ISC_REQ_MANUAL_CRED_VALIDATION  =0x00080000,
				ISC_REQ_RESERVED1               =0x00100000,
				ISC_REQ_FRAGMENT_TO_FIT         =0x00200000,
						
				ISC_RET_DELEGATE                =0x00000001,
				ISC_RET_MUTUAL_AUTH             =0x00000002,
				ISC_RET_REPLAY_DETECT           =0x00000004,
				ISC_RET_SEQUENCE_DETECT         =0x00000008,
				ISC_RET_CONFIDENTIALITY         =0x00000010,
				ISC_RET_USE_SESSION_KEY         =0x00000020,
				ISC_RET_USED_COLLECTED_CREDS    =0x00000040,
				ISC_RET_USED_SUPPLIED_CREDS     =0x00000080,
				ISC_RET_ALLOCATED_MEMORY        =0x00000100,
				ISC_RET_USED_DCE_STYLE          =0x00000200,
				ISC_RET_DATAGRAM                =0x00000400,
				ISC_RET_CONNECTION              =0x00000800,
				ISC_RET_INTERMEDIATE_RETURN     =0x00001000,
				ISC_RET_CALL_LEVEL              =0x00002000,
				ISC_RET_EXTENDED_ERROR          =0x00004000,
				ISC_RET_STREAM                  =0x00008000,
				ISC_RET_INTEGRITY               =0x00010000,
				ISC_RET_IDENTIFY                =0x00020000,
				ISC_RET_NULL_SESSION            =0x00040000,
				ISC_RET_MANUAL_CRED_VALIDATION  =0x00080000,
				ISC_RET_RESERVED1               =0x00100000,
				ISC_RET_FRAGMENT_ONLY           =0x00200000,

				ASC_REQ_DELEGATE                =0x00000001,
				ASC_REQ_MUTUAL_AUTH             =0x00000002,
				ASC_REQ_REPLAY_DETECT           =0x00000004,
				ASC_REQ_SEQUENCE_DETECT         =0x00000008,
				ASC_REQ_CONFIDENTIALITY         =0x00000010,
				ASC_REQ_USE_SESSION_KEY         =0x00000020,
				ASC_REQ_ALLOCATE_MEMORY         =0x00000100,
				ASC_REQ_USE_DCE_STYLE           =0x00000200,
				ASC_REQ_DATAGRAM                =0x00000400,
				ASC_REQ_CONNECTION              =0x00000800,
				ASC_REQ_CALL_LEVEL              =0x00001000,
				ASC_REQ_EXTENDED_ERROR          =0x00008000,
				ASC_REQ_STREAM                  =0x00010000,
				ASC_REQ_INTEGRITY               =0x00020000,
				ASC_REQ_LICENSING               =0x00040000,
				ASC_REQ_IDENTIFY                =0x00080000,
				ASC_REQ_ALLOW_NULL_SESSION      =0x00100000,
				ASC_REQ_ALLOW_NON_USER_LOGONS   =0x00200000,
				ASC_REQ_ALLOW_CONTEXT_REPLAY    =0x00400000,
				ASC_REQ_FRAGMENT_TO_FIT         =0x00800000,
				ASC_REQ_FRAGMENT_SUPPLIED       =0x00002000,
				ASC_REQ_NO_TOKEN                =0x01000000,

				ASC_RET_DELEGATE                =0x00000001,
				ASC_RET_MUTUAL_AUTH             =0x00000002,
				ASC_RET_REPLAY_DETECT           =0x00000004,
				ASC_RET_SEQUENCE_DETECT         =0x00000008,
				ASC_RET_CONFIDENTIALITY         =0x00000010,
				ASC_RET_USE_SESSION_KEY         =0x00000020,
				ASC_RET_ALLOCATED_MEMORY        =0x00000100,
				ASC_RET_USED_DCE_STYLE          =0x00000200,
				ASC_RET_DATAGRAM                =0x00000400,
				ASC_RET_CONNECTION              =0x00000800,
				ASC_RET_CALL_LEVEL              =0x00002000,// skipped 1000 to be like ISC_
				ASC_RET_THIRD_LEG_FAILED        =0x00004000,
				ASC_RET_EXTENDED_ERROR          =0x00008000,
				ASC_RET_STREAM                  =0x00010000,
				ASC_RET_INTEGRITY               =0x00020000,
				ASC_RET_LICENSING               =0x00040000,
				ASC_RET_IDENTIFY                =0x00080000,
				ASC_RET_NULL_SESSION            =0x00100000,
				ASC_RET_ALLOW_NON_USER_LOGONS   =0x00200000,
				ASC_RET_ALLOW_CONTEXT_REPLAY    =0x00400000,
				ASC_RET_FRAGMENT_ONLY           =0x00800000,
				ASC_RET_NO_TOKEN                =0x01000000


			}

			public enum SECURITY_STATUS:uint
			{
				SEC_E_OK = 0x00000000,//The security context was successfully established.  
				SEC_I_CONTINUE_NEEDED = 0x00090312, //The client must send the output token to the server and then pass the token returned by the server in a second call to InitializeSecurityContext.  
				SEC_I_COMPLETE_NEEDED = 0x00090313, //The client must finish building the message and then call the CompleteAuthToken function. 
				SEC_I_COMPLETE_AND_CONTINUE = 0x00090314,//The client must call CompleteAuthToken, then pass the output to the server, and 
				//finally make a second call to InitializeSecurityContext.  
				SEC_E_INVALID_TOKEN   = 0x80090308,//The token passed to the function is invalid. 
				SEC_E_CONTEXT_EXPIRED = 0x80090317,//The context has expired and can no longer be used.
				SEC_E_INVALID_HANDLE = 0x80090301,//The handle passed to the function is invalid. 
				SEC_E_LOGON_DENIED   = 0x8009030C,  //The logon failed. 
				SEC_E_INTERNAL_ERROR = 0x80090304,//The Local Security Authority cannot be contacted. 
				SEC_E_NO_AUTHENTICATING_AUTHORITY = 0x80090311, //No authority could be contacted for authentication.
				SEC_E_TARGET_UNKNOWN = 0x80090303,//The target was not recognized. 
				SEC_E_NO_CREDENTIALS = 0x8009030E,//No credentials are available in the security package. 
				SEC_E_UNKNOWN_CREDENTIALS = 0x8009030D,//The credentials supplied to the package were not recognized. 
				SEC_E_NOT_OWNER = 0x80090306,// The caller of the function does not own the necessary credentials. 
				SEC_E_INSUFFICIENT_MEMORY = 0x80090300,// Not enough memory is available to complete this request. 
				SEC_E_SECPKG_NOT_FOUND = 0x80090305 // The requested security package does not exist. 
			}

			public enum SecBufferFlags:uint
			{
				SECBUFFER_VERSION			  =0,
				SECBUFFER_EMPTY              =0,   // Undefined, replaced by provider
				SECBUFFER_DATA               =1,   // Packet data
				SECBUFFER_TOKEN              =2,   // Security token
				SECBUFFER_PKG_PARAMS         =3,   // Package specific parameters
				SECBUFFER_MISSING            =4,   // Missing Data indicator
				SECBUFFER_EXTRA              =5,   // Extra data
				SECBUFFER_STREAM_TRAILER     =6,   // Security Trailer
				SECBUFFER_STREAM_HEADER      =7,   // Security Header
				SECBUFFER_NEGOTIATION_INFO   =8,   // Hints from the negotiation pkg
				SECBUFFER_PADDING            =9,   // non-data padding
				SECBUFFER_STREAM             =10,  // whole encrypted message
				SECBUFFER_MECHLIST           =11,  
				SECBUFFER_MECHLIST_SIGNATURE =12, 
				SECBUFFER_TARGET             =13,
				SECBUFFER_CHANNEL_BINDINGS   =14,

				SECBUFFER_ATTRMASK          		=0xF0000000,
				SECBUFFER_READONLY          		=0x80000000, // Buffer is read-only, no checksum
				SECBUFFER_READONLY_WITH_CHECKSUM	=0x10000000,  // Buffer is read-only, and checksummed
				SECBUFFER_RESERVED          		=0x60000000  // Flags reserved to security system
			}

			public enum DataRepresentationConstant:uint
			{
			
				SECURITY_NETWORK_DREP        = 0x00000000,
				SECURITY_NATIVE_DREP         = 0x00000010
			}


			/// ///////////////////////////////CLASS////////////////////////
			public abstract class SSPAPI
			{
				public  SSPAPI()
				{
				}

				[DllImport(@"C:\Winnt\System32\Secur32.dll")]
				public static extern SECURITY_STATUS CompleteAuthToken(ref SecHandle phContext,ref SecBufferDesc pToken);
			
				[DllImport(@"C:\Winnt\System32\Secur32.dll")]
					public static extern SECURITY_STATUS DeleteSecurityContext(ref SecHandle phContext);
				
				/// <summary>
				///http://msdn.microsoft.com/library/default.asp?url=/library/en-us/wcesecurity5/html/wce50lrfacquirecredentialshandle.asp
				/*
					This function retrieves information about a specified security package. This information includes the bounds on sizes of authentication information, credentials, and contexts.
					This function retrieves information about a specified security package. This information includes the bounds on sizes of authentication information, credentials, and contexts.

					SECURITY_STATUS QuerySecurityPackageInfo(
						SEC_CHAR* pszPackageName, 
						PSecPkgInfo* ppPackageInfo 
						);
					Parameters
						pszPackageName 
						[in] Pointer to a null-terminated string that specifies the name of the security package. 
						ppPackageInfo 
						[out] Pointer to a variable that receives a pointer to a SecPkgInfo structure containing information about the specified security package. 
						
					Return Values
						SEC_E_OK indicates success. A nonzero error value indicates failure.

					Remarks
						The caller must call the FreeContextBuffer function to free the buffer returned in ppPackageInfo.
				*/
				/// </summary>
				/// <param name="pPackageName"></param>
				/// <param name="ppPackageInfo"></param>
				/// <returns></returns>
				[DllImport(@"C:\Winnt\System32\Secur32.dll")]
				private static extern SECURITY_STATUS  QuerySecurityPackageInfo(
					[In]string pPackageName,
					[In,Out] ref IntPtr /* **ppPackageInfo */ ppPackageInfo);

				/// <summary>
				/// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/wcesecurity5/html/wce50lrfacquirecredentialshandle.asp
				/*
					This function returns an array of SecPkgInfo structures that describe the security packages available to the client.

					SECURITY_STATUS EnumerateSecurityPackages( 
						PULONG pcPackages,
						PSecPkgInfo* ppPackageInfo 
						);
					Parameters
						pcPackages 
						[out] Pointer to a ULONG variable that receives the number of packages returned. 
					ppPackageInfo 
						[out] Pointer to a variable that receives a pointer to an array of SecPkgInfo structures. Each structure contains data that describes a security package available from the security provider. 
					Return Values
						SEC_E_OK indicates success. A nonzero error value indicates failure.

					Remarks
						The SecPkgInfo structures provide data that the caller can use to determine which security package best satisfies the caller's requirements.

					The caller can use the Name member of a SecPkgInfo structure to specify a security package in a call to the AcquireCredentialsHandle function.

					The caller must assume that the security provider includes all available security packages in the single call to EnumerateSecurityPackages.
					The caller must call the FreeContextBuffer function to free the buffer returned in ppPackageInfo.
				*/
				/// </summary>
				/// <param name="pszPackages"></param>
				/// <param name="ppPackageInfo"></param>
				/// <returns></returns>
				[DllImport(@"C:\Winnt\System32\Secur32.dll")]
				private static extern SECURITY_STATUS  EnumerateSecurityPackages(
					out int pszPackages,
					[In, Out]ref IntPtr ppPackageInfo
					);

				/// <summary>
				///http://msdn.microsoft.com/library/default.asp?url=/library/en-us/wcesecurity5/html/wce50lrfacquirecredentialshandle.asp
				/*
				This function retrieves the attributes of a credential, such as the name associated with the credential.

				SECURITY_STATUS QueryCredentialsAttributes( 
					PCredHandle phCredential, 
					ULONG ulAttribute, 
					PVOID pBuffer 
					);
				Parameters
					phCredential 
					[in] Pointer to the handle to the credentials to be queried. 
				ulAttribute 
					[in] Specifies the attribute to query. This parameter must be SECPKG_CRED_ATTR_NAMES. 
					pBuffer 
					[out] Pointer to a buffer that receives the requested attribute. For the SECPKG_CRED_ATTR_NAMES attribute,
					pBuffer must point to a SecPkgCredentials_Names structure. 
				Return Values
					SEC_E_OK indicates success. 
					The following table shows the possible error values.

				Value Description 
					SEC_E_INVALID_HANDLE The handle passed to the function is invalid. 
					SEC_E_INSUFFICIENT_MEMORY Insufficient memory. 

				Remarks
					This function allows a customer of the security services to determine the name associated with the specified
					credentials. The caller must allocate the structure pointed to by the pBuffer parameter. The security provider
					allocates the buffer for any pointer returned in the pBuffer structure. The caller can call the FreeContextBuffer
					function to free any pointers allocated by the security provider. 
				*/
				/// </summary>
				/// <param name="phCredential"></param>
				/// <param name="ulAttribute"></param>
				/// <param name="pBuffer"></param>
				/// <returns></returns>
				[DllImport(@"C:\Winnt\System32\Secur32.dll")]
				public static extern SECURITY_STATUS  QueryCredentialsAttributes(
					[In,Out]ref SecHandle phCredential,
					[In]SecurityCredentialsAttribute ulAttribute,
					[In,Out]ref SecPkgContext_Names pBuffer
					);

				/// <summary>
				/// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/wcesecurity5/html/wce50lrfacquirecredentialshandle.asp
				/* This function allows applications to acquire a handle to preexisting credentials associated with the user on whose behalf the call is made. These preexisting credentials are established through a system logon not described here. However, this is different from login to the network and does not imply gathering of credentials.

					SECURITY_STATUS AcquireCredentialsHandle( 
					SEC_CHAR* pszPrincipal, 
					SEC_CHAR* pszPackage,
					ULONG fCredentialUse, 
					PLUID pvLogonId, 
					PVOID pAuthData,
					PVOID pGetKeyFn, 
					VOID pvGetKeyArgument, 
					PCredHandle phCredential,
					PTimeStamp ptsExpiry 
					);
					
				Parameters
				pszPrincipal 
					[in] Pointer to a null-terminated string that specifies the name of the principal whose credentials the
					handle will reference. Note that if the process requesting the handle does not have access to the credentials,
					the function returns an error. A null string indicates that the process requires a handle to the credentials
					of the user under whose security context it is executing. 
				pszPackage 
					[in] Pointer to a null-terminated string that specifies the name of the security package with which these
					credentials will be used. This is a security package name returned in the Name member of a SecPkgInfo structure
					returned by the EnumerateSecurityPackages function. 
				fCredentialUse 
					[in] Flag that indicates how these credentials will be used. The following list shows the possible values.
					This parameter can be one of these values: 
					SECPKG_CRED_INBOUND 
					SECPKG_CRED_OUTBOUND 
					SECPKG_CRED_BOTH 
					The credentials created with the CRED_INBOUND option can be used only for validating incoming calls.
					They cannot be used for accessing objects. 

				pvLogonId 
					[in] Void pointer to a Microsoft® Windows® NT–style logon identifier, an LUID. This parameter
					is provided for file-system processes such as network redirectors. This parameter can be NULL. 
					pAuthData 
						[in] Pointer to package-specific data. This parameter can be NULL, indicating that the default
						credentials for that package should be used. The NTLM Security Support Provider (SSP) accepts
						a pointer to a SEC_WINNT_AUTH_IDENTITY structure containing the user name, domain name, and
						password. 
					pGetKeyFn 
						[in] Not supported. Set to NULL. 
					pvGetKeyArgument 
						[in] Not supported. Set to NULL. 
					phCredential 
						[out] Pointer to CredHandle structure that receives the credential handle. 
						See SSPI Handles for information on CredHandle. 
				Return Values
					SEC_E_OK indicates success. 

					The following table shows the possible error values.

					Value  Description 
					SEC_E_UNKNOWN_CREDENTIALS The credentials supplied to the package were not recognized. 
					SEC_E_NO_CREDENTIALS No credentials are available in the security package. 
					SEC_E_NOT_OWNER The caller of the function does not own the necessary credentials. 
					SEC_E_INSUFFICIENT_MEMORY Not enough memory is available to complete this request. 
					SEC_E_INTERNAL_ERROR The Local Security Authority cannot be contacted. 
					SEC_E_SECPKG_NOT_FOUND The requested security package does not exist. 

				Remarks
					This function returns a handle to the credentials of a principal (user, client) as used by 
					a specific security package. The handle returned can be used in subsequent calls to the 
					AcceptSecurityContext and InitializeSecurityContext functions. 
					The AcquireCredentialsHandle function will not let a process obtain a handle to credentials
					that are not related to the process; in other words, a process cannot get the credentials of
					another user who is logged on to the same computer. There is no way to determine whether a
					process is a Trojan horse if it is executed by the user.
					This function uses the following algorithm to determine whether to grant the request for a
					handle to the credentials. 
					If the caller is a system process with the SE_TCB_NAME privilege (for example, an FSP) and the
					caller provides both the name and logon identifier, the function verifies that they match before
					returning the credentials. If only one is provided, the function returns a handle to that identifier.
					A caller that is not a system process can only obtain a handle to the credentials under which it is
					running. The caller can provide the name or the logon identifier, but it must be for the current session
					or the request fails.
					A package may call the function in pGetKeyFn provided by the RPC run-time transport. If the transport does
					not support the notion of callback to retrieve credentials, this parameter must be NULL.
					For kernel mode callers, the following differences must be noted: The two string parameters are Unicode
					strings and the buffer values must be allocated in process virtual memory, not from the pool.
					When you no longer need the returned credentials, call the FreeCredentialsHandle function. 
				*/
				/// </summary>
				/// <param name="pszPrincipal"></param>
				/// <param name="pszPackage"></param>
				/// <param name="fCredentialUse"></param>
				/// <param name="pvLogonId"></param>
				/// <param name="pAuthData"></param>
				/// <param name="pGetKeyFn"></param>
				/// <param name="pvGetKeyArgument"></param>
				/// <param name="phCredential"></param>
				/// <param name="ptsExpiry"></param>
				/// <returns></returns>
				[DllImport(@"C:\Winnt\System32\Secur32.dll")]
				private static extern SECURITY_STATUS AcquireCredentialsHandle(
					[In]string pszPrincipal,
					[In]string pszPackage,
					[In]CredentialUseFlags CredentialUse,
					[In]IntPtr/*LPUID*/pvLogonId,
					[In]IntPtr/*SEC_WINNT_AUTH_IDENTITY*/pAuthData,
					[In]IntPtr/*NULL*/ pGetKeyFn,
					[In]IntPtr/*NULL*/ pvGetKeyArgument, 
					[In,Out]ref SecHandle phCredential,
					[In,Out]ref TimeStamp ptsExpiry 
					);

				/// <summary>
				/// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/wcesecurity5/html/wce50lrfacquirecredentialshandle.asp?frame=true
				/*This function initiates the outbound security context from a credential handle. The function establishes a security context between the client application and a remote peer. The function returns a token that the client must pass to the remote peer, which in turn submits it to the local security implementation through the AcceptSecurityContext call. The token generated should be considered opaque by all callers.
				SECURITY_STATUS InitializeSecurityContext( 
					PCredHandle phCredential,
					PCtxtHandle phContext, 
					SEC_CHAR* pszTargetName, 
					ULONG fContextReq,
					ULONG Reserved1, 
					ULONG TargetDataRep, 
					PSecBufferDesc pInput, 
					ULONG Reserved2, 
					PCtxtHandle phNewContext, 
					PSecBufferDesc pOutput,
					PULONG pfContextAttr, 
					PTimeStamp ptsExpiry 
					);
				Parameters
					phCredential 
					[in] Pointer to the handle to the credentials to use to create the context. The client retrieves this handle
					by calling the AcquireCredentialsHandle function. 
				phContext 
					[in, out] Pointer to the handle of a CtxtHandle structure. For information on CtxtHandle, see SSPI Handles.
					On the first call to InitializeSecurityContext, this pointer is NULL. On the second call, this parameter is
					the handle to the partially formed context returned in the phNewContext parameter by the first call. 
				pszTargetName 
					[in] Pointer to a null-terminated string that indicates the target of the context. 
					The name is security-package specific. 
				fContextReq 
					[in] Set of bit flags that indicate the requirements of the context. Not all packages can support all requirements.
					For more information about context requirements, see Cryptography. The following list shows the values that this 
					parameter can include: 
					ISC_REQ_ALLOCATE_MEMORY 
					ISC_REQ_CALL_LEVEL 
					ISC_REQ_CONFIDENTIALITY 
					ISC_REQ_CONNECTION 
					ISC_REQ_DATAGRAM 
					ISC_REQ_DELEGATE 
					ISC_REQ_EXTENDED_ERROR 
					ISC_REQ_INTEGRITY 
					ISC_REQ_MUTUAL_AUTH 
					ISC_REQ_PROMPT_FOR_CREDS 
					ISC_REQ_REPLAY_DETECT 
					ISC_REQ_SEQUENCE_DETECT 
					ISC_REQ_STREAM 
					ISC_REQ_USE_DCE_STYLE 
					ISC_REQ_USE_SESSION_KEY 
					ISC_REQ_USE_SUPPLIED_CREDS 
				Reserved1 
					Reserved; set to zero. 
				TargetDataRep 
					[in] Indicates the data representation (byte ordering and so on) on the target. 
					The constant SECURITY_NATIVE_DREP may be supplied by the transport application,
					indicating that the native format is in use. 
				pInput 
					[in, out] Pointer to a SecBufferDesc structure that contains pointers to the buffers supplied
					as input to the package. The transport application should provide as much input as possible,
					although some packages may not be interested in the nonsecurity portions. This parameter is
					optional, particularly on the first call to InitializeSecurityContext. 
				Reserved2 
					Reserved; set to zero. 
				phNewContext 
					[in, out] Pointer to the handle of a CtxtHandle structure. For information on CtxtHandle, see
					SSPI Handles. On the first call to InitializeSecurityContext, this pointer receives the new
					context handle. On the second call, this can be the same as the handle specified in the 
					phContext parameter. 
				pOutput 
					[out] Pointer to a SecBufferDesc structure that contains pointers to the buffers that receive
					the output data. If a buffer was typed as READWRITE in the input, it will be there on output.
					The system will allocate a buffer for the security token if requested (through ISC_REQ_ALLOCATE_MEMORY)
					 and fill in the address in the buffer descriptor for the security token. 
				pfContextAttr 
					[out] Pointer to a variable that receives a set of bit flags indicating the attributes of the
					established context. For more information about context requirements, see Cryptography. 
					The following list shows the flags that this value can include: 
					ISC_RET_ALLOCATED_MEMORY 
					ISC_RET_CALL_LEVEL 
					ISC_RET_CONFIDENTIALITY 
					ISC_RET_CONNECTION 
					ISC_RET_DATAGRAM 
					ISC_RET_DELEGATE 
					ISC_RET_EXTENDED_ERROR 
					ISC_RET_INTEGRITY 
					ISC_RET_INTERMEDIATE_RETURN 
					ISC_RET_MUTUAL_AUTH 
					ISC_RET_REPLAY_DETECT 
					ISC_RET_SEQUENCE_DETECT 
					ISC_RET_STREAM 
					ISC_RET_USED_COLLECTED_CREDS 
					ISC_RET_USED_DCE_STYLE 
					ISC_RET_USE_SESSION_KEY 
					ISC_RET_USED_SUPPLIED_CREDS 
				ptsExpiry 
					[out] Pointer to a TimeStamp structure that receives the expiration time of the context.
					The security provider should always return this value in local time. 
				Return Values
					The following table shows the possible return values.

				Value Description 
				SEC_E_OK The security context was successfully initialized. There is no need for another InitializeSecurityContext
				call and no response from the server is expected.  
				SEC_I_CONTINUE_NEEDED The client must send the output token to the server and then pass the token returned by the
				server in a second call to InitializeSecurityContext.  
				SEC_I_COMPLETE_NEEDED The client must finish building the message and then call the CompleteAuthToken function. 
				SEC_I_COMPLETE_AND_CONTINUE The client must call CompleteAuthToken, then pass the output to the server, and 
				finally make a second call to InitializeSecurityContext.  
				The following table shows the possible error values.
				Value Description 
				SEC_E_INVALID_HANDLE The handle passed to the function is invalid. 
				SEC_E_TARGET_UNKNOWN The target was not recognized. 
				SEC_E_LOGON_DENIED The logon failed. 
				SEC_E_INTERNAL_ERROR The Local Security Authority cannot be contacted. 
				SEC_E_NO_CREDENTIALS No credentials are available in the security package. 
				SEC_E_NO_AUTHENTICATING_AUTHORITY No authority could be contacted for authentication. 

				Remarks
				This function is used by a client to initialize an outbound context. 

				For a two-leg security package, the calling sequence is as follows: 

				The client calls this function with phContext set to NULL and fills in the buffer descriptor with the input message. 
				The security package examines the parameters and constructs an opaque token, placing it in the TOKEN element in the 
				buffer array. If the fContextReq parameter includes the ISC_REQ_ALLOCATE_MEMORY flag, the security package allocates
				the memory and returns the pointer in the TOKEN element. 
				The client sends the token returned in the pOutput buffer to the target server. The server then passes the token as
				an input argument in a call to the AcceptSecurityContext function. 
				AcceptSecurityContext may return a token, which the server sends to the client for a second call to InitializeSecurityContext. 
				For a three-leg (mutual authentication) security package, the calling sequence is as follows: 

				The client calls the function as described earlier, but the package returns the SEC_I_CONTINUE_NEEDED success code. 
				The client then sends the output token to the server and waits for the server's reply. 
				Upon receipt of the server's response, the client calls InitializeSecurityContext again, with phContext set to the handle that
				was returned from the first call. The token received from the server is supplied in the pInput parameter. If the server has
				successfully responded, the security package will respond with success; otherwise, it will invalidate the context. 
				To initialize a security context, more than one call to this function may be required, depending on the underlying authentication
				mechanism as well as the choices specified in the fContextReq parameter. 

				The fContextReq and pfContextAttributes parameters are bit masks representing various context attributes. 
				For more information about context requirements, see Cryptography. The pfContextAttributes parameter is 
				valid on any successful return, but only on the final successful return should you examine the flags 
				pertaining to security aspects of the context. Intermediate returns can set, for example, the 
				ISC_RET_ALLOCATED_MEMORY flag. 

				The caller is responsible for determining whether the final context attributes are sufficient.
				For example, if confidentiality was requested but could not be established, some applications
				may choose to shut down the connection immediately.

				When the ISC_REQ_PROMPT_FOR_CREDS flag is set, the security package attempts to prompt the user
				for the credentials to use for the connection. If the caller is not an interactive user (for
				example, a noninteractive service), this flag is ignored. During the prompt, the package may
				inquire if the supplied credentials should be retained. If so, the package can store them away
				for future use, relieving the user of having to enter credentials later. This behavior, if
				supported, should be configurable for environments in which the credentials cannot or should
				not be stored away.

				If the ISC_REQ_USE_SUPPLIED_CREDS flag is set, the security package should look for a SECBUFFER_PKG_PARAMS
				buffer type in the pInput input buffer. This is not a generic solution, but it allows for a strong pairing
				of security package and application when appropriate.

				You can specify either ISC_REQ_CONNECTION or ISC_REQ_DATAGRAM flags for the fContextReq parameter, but not both.
				Note that Datagram support is available only if NTLM v2 can be negotiated.

				If the ISC_REQ_ALLOCATE_MEMORY flag was specified, the caller must free the memory by calling the FreeContextBuffer
				function.

				For example, the input token could be the challenge from a LAN Manager or Windows NT® file server. In this case, the
				output token would be the NTLM encrypted response to the challenge. 

				The action the client takes depends on the return code from this function. If the return code is SEC_E_OK, there
				will be no second InitializeSecurityContext call and no response from the server is expected. If the return code
				is SEC_I_CONTINUE_NEEDED, the client expects a token in response from the server and passes it in a second call
				to InitializeSecurityContext. The SEC_I_COMPLETE_NEEDED return code indicates that the client must finish building
				the message and call the CompleteAuthToken function. The SEC_I_COMPLETE_AND_CONTINUE code, of course, incorporates
				both of these actions.

				If the connection is rejected by the server, the client must call the DeleteSecurityContext
				function at that time to free any resources.
				*/
				/// </summary>
				/// <param name="phCredential"></param>
				/// <param name="phContext"></param>
				/// <param name="pszTargetName"></param>
				/// <param name="fContextReq"></param>
				/// <param name="Reserved1"></param>
				/// <param name="TargetDataRep"></param>
				/// <param name="pInput"></param>
				/// <param name="Reserved2"></param>
				/// <param name="phNewContext"></param>
				/// <param name="pOutput"></param>
				/// <param name="pfContextAttr"></param>
				/// <param name="ptsExpiry"></param>
				/// <returns></returns>
				[DllImport(@"C:\Winnt\System32\Secur32.dll")]
				public static extern SECURITY_STATUS InitializeSecurityContext( 
					[In,Out]ref SecHandle hCredential,
					[In]IntPtr /*SecHandle*/ hContext,  
					[In]string szTargetName, 
					[In]SecurityContextRequirement ContextReq,
					[In]uint Reserved1, 
					[In]DataRepresentationConstant TargetDataRep, 
					[In]IntPtr /*SecBufferDesc*/ Input, 
					[In]uint Reserved2, 
					[In,Out]ref SecHandle hNewContext, 
					[In,Out]ref SecBufferDesc Output,
					[In,Out]ref uint ContextAttr, 
					[In,Out]ref TimeStamp tsExpiry 
					);


				[DllImport(@"C:\Winnt\System32\Secur32.dll")]
				public static extern SECURITY_STATUS InitializeSecurityContext( 
					[In,Out]ref SecHandle hCredential,
					[In]ref SecHandle hContext,  
					[In]string szTargetName, 
					[In]SecurityContextRequirement ContextReq,
					[In]uint Reserved1, 
					[In]DataRepresentationConstant TargetDataRep, 
					[In]IntPtr /*SecBufferDesc*/ Input, 
					[In]uint Reserved2, 
					[In,Out]ref SecHandle hNewContext, 
					[In,Out]ref SecBufferDesc Output,
					[In,Out]ref uint ContextAttr, 
					[In,Out]ref TimeStamp tsExpiry 
					);


				/// <summary>
				/// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/wcesecurity5/html/wce50lrfacquirecredentialshandle.asp
				/*	This function enables the server part of a transport application to establish a security context between the server and a remote client. The remote client uses the InitializeSecurityContext function to start the process of establishing a security context. The server may need one or more reply tokens from the remote client to complete the establishment of the security context.
					SECURITY_STATUS AcceptSecurityContext( 
					PCredHandle phCredential, 
					PCtxtHandle phContext, 
					PSecBufferDesc pInput, 
					ULONG pfContextReq, 
					ULONG TargetDataRep, 
					PCtxtHandle phNewContext, 
					PSecBufferDesc pOutput, 
					PULONG pfContextAttr, 
					PTimeStamp ptsExpiry 
					);
				Parameters
				phCredential 
					[in] Pointer to the handle to the server's credentials. The server calls the AcquireCredentialsHandle
					function to retrieve this handle. 
				phContext 
					[in] Pointer to the handle of a CtxtHandle structure. For information on CtxtHandle, see SSPI Handles.
					On the first call to AcceptSecurityContext, this pointer is NULL. On the second call, this is the handle
					to the partially formed context that was returned in the phNewContext parameter by the first call. 
				pInput 
					[in] Pointer to a SecBufferDesc structure that contains the input buffer descriptor.
					Depending on the security package, this parameter may be NULL if no initial token is ready. 
				pfContextReq 
					[in] Set of bit flags that specify the attributes that the server requires for the context to
					be established. The following list shows the flags that can be used in any combination: 
					ASC_REQ_ALLOCATE_MEMORY 
					ASC_REQ_CALL_LEVEL 
					ASC_REQ_CONFIDENTIALITY 
					ASC_REQ_CONNECTION 
					ASC_REQ_DATAGRAM 
					ASC_REQ_DELEGATE 
					ASC_REQ_EXTENDED_ERROR 
					ASC_REQ_INTEGRITY 
					ASC_REQ_MUTUAL_AUTH 
					ASC_REQ_REPLAY_DETECT 
					ASC_REQ_STREAM 
					ASC_REQ_SEQUENCE_DETECT 
					ASC_REQ_USE_DCE_STYLE 
					ASC_REQ_USE_SESSION_KEY 
				TargetDataRep 
					[in] Indicates the data representation (byte ordering and so on) on the target.
					You can specify SECURITY_NATIVE_DREP to indicate that the native format is in use. 
				phNewContext 
					[out] Pointer to a CtxtHandle structure. For information on CtxtHandle, see SSPI Handles.
					On the first call to AcceptSecurityContext, this pointer receives the new context handle.
					On the second call, this parameter can be the same as the handle specified in the phContext
					parameter. 
				pOutput 
					[in] Pointer to a SecBufferDesc structure that contains the output buffer descriptor. 
				pfContextAttr 
					[out] Pointer to a variable that receives a set of bit flags indicating the attributes of the
					established context. For more information about context requirements, see Cryptography. The 
					following list shows the possible flags: 
					ASC_RET_ALLOCATED_MEMORY 
					ASC_RET_CALL_LEVEL 
					ASC_RET_CONFIDENTIALITY 
					ASC_RET_CONNECTION 
					ASC_RET_DATAGRAM 
					ASC_RET_DELEGATE 
					ASC_RET_EXTENDED_ERROR 
					ASC_RET_INTEGRITY 
					ASC_RET_MUTUAL_AUTH 
					ASC_RET_REPLAY_DETECT 
					ASC_RET_SEQUENCE_DETECT 
					ASC_RET_STREAM 
					ASC_RET_USED_DCE_STYLE 
					ASC_RET_USE_SESSION_KEY 
					ASC_RET_THIRD_LEG_FAILED 
				ptsExpiry 
					[out] Pointer to a PTimeStamp variable that receives the expiration time of the context. 
					The security provider should always return this value in local time. 
				Return Values
					The following table shows the possible return values.

				Value Description 
					SEC_E_OK The security context was successfully established.  
					SEC_I_CONTINUE_NEEDED The client must send the output token to the server and then pass the token returned by the server in a second call to InitializeSecurityContext.  
					SEC_I_COMPLETE_NEEDED The client must finish building the message and then call the CompleteAuthToken function. 
					SEC_I_COMPLETE_AND_CONTINUE The client must call CompleteAuthToken, then pass the output to the server, and 
				
				finally make a second call to InitializeSecurityContext.  
				The following table shows the possible error values.

				Value Description 
					SEC_E_INVALID_TOKEN The token passed to the function is invalid. 
					SEC_E_INVALID_HANDLE The handle passed to the function is invalid. 
					SEC_E_LOGON_DENIED The logon failed. 
					SEC_E_INTERNAL_ERROR The Local Security Authority cannot be contacted. 
					SEC_E_NO_AUTHENTICATING_AUTHORITY No authority could be contacted for authentication. 

				Remarks
					This function is the server counterpart to the InitializeSecurityContext function.

					When a request comes in, the server uses the fContextReq parameter to specify what it requires of the session. 
					In this fashion, a server can specify that clients must be capable of using a confidential or integrity-checked 
					session, and it can fail clients that cannot meet that demand. As an alternative, a server can require nothing, 
					and whatever the client can provide or requires is returned in the pfContextAttr parameter.

					For a package that supports three-leg mutual authentication, the calling sequence is as follows: 

					The client transmits a token to the server. 
					The server calls AcceptSecurityContext the first time, generating a reply token. 
					The client passes this token in a second call to InitializeSecurityContext, which generates a final token. 
					The server uses this token in the final call to AcceptSecurityContext to complete the session. 
					
					LAN Manager and Windows NT use the following authentication style: 

					The client connects to negotiate a protocol. 
					The server calls AcceptSecurityContext to set up a context and generate a challenge to the client. 
					The client calls InitializeSecurityContext and creates the response. 
					The server calls AcceptSecurityContext the final time to allow the security package to verify that the response
					is appropriate for the challenge. 
				Requirements
					OS Versions: Windows CE 2.10 and later.
					Header: Security.h, Sspi.h.
					Link Library: Secur32.lib.
				*/
				/// </summary>
				/// <param name="phCredential"></param>
				/// <param name="phContext"></param>
				/// <param name="pInput"></param>
				/// <param name="pfContextReq"></param>
				/// <param name="TargetDataRep"></param>
				/// <param name="phNewContext"></param>
				/// <param name="pOutput"></param>
				/// <param name="pfContextAttr"></param>
				/// <param name="ptsExpiry"></param>
				/// <returns></returns>

				[DllImport(@"C:\Winnt\System32\Secur32.dll")]
				public static extern SECURITY_STATUS AcceptSecurityContext( 
					[In,Out]ref SecHandle hCredential, 
					[In]IntPtr /*SecHandle*/ hContext, 
					[In]IntPtr /*SecBufferDesc*/ Input, 
					[In]SecurityContextRequirement ContextReq, 
					[In]DataRepresentationConstant TargetDataRep, 
					[In,Out]ref SecHandle hNewContext, 
					[In,Out]ref SecBufferDesc Output, 
					[In,Out]ref uint ContextAttr, 
					[In,Out]ref TimeStamp tsExpiry 
					);

				[DllImport(@"C:\Winnt\System32\Secur32.dll")]
				public static extern SECURITY_STATUS AcceptSecurityContext( 
					[In,Out]ref SecHandle hCredential, 
					[In]ref SecHandle hContext, 
					[In]IntPtr /*SecBufferDesc*/ Input, 
					[In]SecurityContextRequirement ContextReq, 
					[In]DataRepresentationConstant TargetDataRep, 
					[In,Out]ref SecHandle hNewContext, 
					[In,Out]ref SecBufferDesc Output, 
					[In,Out]ref uint ContextAttr, 
					[In,Out]ref TimeStamp tsExpiry 
					);

				public static SecPkgInfo QuerySecurityPackageInfo(string PackageName)
				{
					
					Authentication.SSPI.SecPkgInfo Info = new Authentication.SSPI.SecPkgInfo(); 
					IntPtr pInfo = Info;
									
					if(Authentication.SSPI.SSPAPI.QuerySecurityPackageInfo(PackageName, ref pInfo) == SECURITY_STATUS.SEC_E_OK)
					{
						Info = pInfo;
					}
					return Info;
				}
	       

				public static int EnumerateSecurityPackages(ref SecPkgInfo[] Packages)
				{
					int szPackages = 0;
					System.IntPtr pPackageInfo = IntPtr.Zero;
					SECURITY_STATUS status = EnumerateSecurityPackages(out szPackages,ref pPackageInfo);
					Packages = new SecPkgInfo[szPackages];
					for(int x = 0; x < szPackages;x++)
					{
						Packages[x] = (SecPkgInfo)((IntPtr)((int)pPackageInfo+(x*Marshal.SizeOf(Packages[x]))));
					}
					return szPackages;
				}


				public static SecHandle AcquireCredentialsHandle(
					string Principal, 
					string Package,
					CredentialUseFlags CredentialUse,
					ref LUID LogonId,
					ref SSPI.Identity AuthData,
					out TimeStamp tsExpiry 
					)
				{
					
					IntPtr pLUID = LogonId;
					IntPtr pSEC_WINNT_AUTH_IDENTITY = AuthData;
		
					SecHandle hCredential = new SecHandle();
					tsExpiry = new TimeStamp();
	 
					SECURITY_STATUS status = Authentication.SSPI.SSPAPI.AcquireCredentialsHandle(null,Package,CredentialUse,pLUID, pSEC_WINNT_AUTH_IDENTITY,IntPtr.Zero,IntPtr.Zero,ref hCredential,ref tsExpiry);
					LogonId  = pLUID;
					AuthData = pSEC_WINNT_AUTH_IDENTITY ;

					return hCredential;
				}


				public static SecHandle AcquireCredentialsHandle(
					string Principal, 
					string Package,
					CredentialUseFlags CredentialUse,
					ref SSPI.Identity AuthData,
					out TimeStamp tsExpiry 
					)
				{
					
					IntPtr pSEC_WINNT_AUTH_IDENTITY =AuthData;
		
					SecHandle hCredential = new SecHandle();
					tsExpiry = new TimeStamp();
	 
					SECURITY_STATUS status = Authentication.SSPI.SSPAPI.AcquireCredentialsHandle(null,Package,CredentialUse,IntPtr.Zero, pSEC_WINNT_AUTH_IDENTITY,IntPtr.Zero,IntPtr.Zero,ref hCredential,ref tsExpiry);
					AuthData = pSEC_WINNT_AUTH_IDENTITY ;

					return hCredential;
				}


				public static SecHandle AcquireCredentialsHandle(
					string Principal, 
					string Package,
					CredentialUseFlags CredentialUse,
					out TimeStamp tsExpiry 
					)
				{
					
					SecHandle hCredential = new SecHandle();
					tsExpiry = new TimeStamp();
	 
					SECURITY_STATUS status = Authentication.SSPI.SSPAPI.AcquireCredentialsHandle(null,Package,CredentialUse,IntPtr.Zero, IntPtr.Zero,IntPtr.Zero,IntPtr.Zero,ref hCredential,ref tsExpiry);
					return hCredential;
				}


				public static SecHandle AcquireCredentialsHandle(
					string Principal, 
					string Package,
					CredentialUseFlags CredentialUse
					)
				{

					SecHandle hCredential = new SecHandle();
					TimeStamp tsExpiry = new TimeStamp();
	 
					SECURITY_STATUS status = Authentication.SSPI.SSPAPI.AcquireCredentialsHandle(null,Package,CredentialUse,IntPtr.Zero, IntPtr.Zero,IntPtr.Zero,IntPtr.Zero,ref hCredential,ref tsExpiry);
					return hCredential;
				}


				public SECURITY_STATUS InitializeSecurityContext( 
					ref SecHandle hCredential,
					ref SecHandle hContext,  
					string szTargetName, 
					SecurityContextRequirement ContextReq,
					DataRepresentationConstant TargetDataRep, 
					ref  SecBufferDesc Input, 
					//ref  SecHandle hNewContext, 
					ref  SecBufferDesc Output,
					out uint ContextAttr, 
					out  TimeStamp tsExpiry 
					)
				{

					ContextAttr = new uint(); 
					tsExpiry = new TimeStamp();
                    
					IntPtr pInput = Input;
					
					SECURITY_STATUS status = InitializeSecurityContext(
						ref hCredential,ref hContext,szTargetName,ContextReq,0,TargetDataRep,pInput,0,ref hContext,ref Output,ref ContextAttr,ref tsExpiry);

					Input = pInput;
					return status;
					
				}
				

				public SECURITY_STATUS InitializeSecurityContext( 
					ref SecHandle hCredential,
					//ref SecHandle hContext, 
					string szTargetName, 
					SecurityContextRequirement ContextReq,
					DataRepresentationConstant TargetDataRep, 
					ref  SecBufferDesc Input,
					ref  SecHandle hNewContext, 
					ref  SecBufferDesc Output,
					out uint ContextAttr, 
					out  TimeStamp tsExpiry 
					)
				{

					ContextAttr = new uint(); 
					tsExpiry = new TimeStamp();

					IntPtr pInput = Input;
					
					SECURITY_STATUS status = InitializeSecurityContext(
						ref hCredential,IntPtr.Zero,szTargetName,ContextReq,0,TargetDataRep,pInput,0,ref hNewContext,ref Output,ref ContextAttr,ref tsExpiry);

					Input = pInput;
					return status;
					
				}


				public SECURITY_STATUS InitializeSecurityContext( 
					ref SecHandle hCredential,
					ref SecHandle hContext,  
					string szTargetName, 
					SecurityContextRequirement ContextReq,
					DataRepresentationConstant TargetDataRep, 
					//ref  SecBufferDesc Input, 
					//ref  SecHandle hNewContext, 
					ref  SecBufferDesc Output,
					out uint ContextAttr, 
					out  TimeStamp tsExpiry 
					)
				{

					ContextAttr = new uint(); 
					tsExpiry = new TimeStamp();
					
					SECURITY_STATUS status = InitializeSecurityContext(
						ref hCredential,ref hContext,szTargetName,ContextReq,0,TargetDataRep,IntPtr.Zero,0,ref hContext,ref Output,ref ContextAttr,ref tsExpiry);

					return status;
					
				}


				public SECURITY_STATUS InitializeSecurityContext( 
					ref SecHandle hCredential,
					//ref SecHandle hContext,
					string szTargetName, 
					SecurityContextRequirement ContextReq,
					DataRepresentationConstant TargetDataRep, 
					//ref  SecBufferDesc Input, 
					ref  SecHandle hNewContext, 
					ref  SecBufferDesc Output,
					out uint ContextAttr, 
					out  TimeStamp tsExpiry 
					)
				{
					
					ContextAttr = new uint(); 
					tsExpiry = new TimeStamp();

					SECURITY_STATUS status = InitializeSecurityContext(
						ref hCredential,IntPtr.Zero,szTargetName,ContextReq,0,TargetDataRep,IntPtr.Zero,0,ref hNewContext,ref Output,ref ContextAttr,ref tsExpiry);

					return status;
					
				}
				

				public SECURITY_STATUS InitializeSecurityContext( 
					ref SecHandle hCredential,
					ref SecHandle hContext,  
					string szTargetName, 
					SecurityContextRequirement ContextReq,
					DataRepresentationConstant TargetDataRep, 
					ref  SecBufferDesc Input, 
					//ref  SecHandle hNewContext, 
					ref  SecBufferDesc Output
					//ref uint ContextAttr, 
					//ref  TimeStamp tsExpiry 
					)
				{
					uint ContextAttr = new uint(); 
					TimeStamp tsExpiry = new TimeStamp();

					IntPtr pInput = Input;
									
					SECURITY_STATUS status = InitializeSecurityContext(
						ref hCredential,ref hContext,szTargetName,ContextReq,0,TargetDataRep,pInput,0,ref hContext,ref Output,ref ContextAttr,ref tsExpiry);

					Input = pInput;

					return status;
					
				}

				
				public SECURITY_STATUS InitializeSecurityContext( 
					ref SecHandle hCredential,
					//ref SecHandle hContext, 
					string szTargetName, 
					SecurityContextRequirement ContextReq,
					DataRepresentationConstant TargetDataRep, 
					ref  SecBufferDesc Input,
					ref  SecHandle hNewContext, 
					ref  SecBufferDesc Output
					//ref uint ContextAttr, 
					//ref  TimeStamp tsExpiry 
					)
				{
					uint ContextAttr = new uint(); 
					TimeStamp tsExpiry = new TimeStamp();

					IntPtr pInput = Input;
					
					SECURITY_STATUS status = InitializeSecurityContext(
						ref hCredential,IntPtr.Zero,szTargetName,ContextReq,0,TargetDataRep,pInput,0,ref hNewContext,ref Output,ref ContextAttr,ref tsExpiry);

					Input = pInput;
					return status;
					
				}


				public SECURITY_STATUS InitializeSecurityContext( 
					ref SecHandle hCredential,
					ref SecHandle hContext,  
					string szTargetName, 
					SecurityContextRequirement ContextReq,
					DataRepresentationConstant TargetDataRep, 
					//ref  SecBufferDesc Input, 
					//ref  SecHandle hNewContext, 
					ref  SecBufferDesc Output
					//ref uint fContextAttr, 
					//ref  TimeStamp tsExpiry 
					)
				{

					uint ContextAttr = new uint(); 
					TimeStamp tsExpiry = new TimeStamp();

									
					SECURITY_STATUS status = InitializeSecurityContext(
						ref hCredential,ref hContext,szTargetName,ContextReq,0,TargetDataRep,IntPtr.Zero,0,ref hContext,ref Output,ref ContextAttr,ref tsExpiry);

					return status;
					
				}


				public SECURITY_STATUS InitializeSecurityContext( 
					ref SecHandle hCredential,
					//ref SecHandle hContext,
					string szTargetName, 
					SecurityContextRequirement ContextReq,
					DataRepresentationConstant TargetDataRep, 
					//ref  SecBufferDesc Input, 
					ref  SecHandle hNewContext, 
					ref  SecBufferDesc Output
					//ref uint ContextAttr, 
					//ref  TimeStamp tsExpiry 
					)
				{
					
					uint ContextAttr = new uint(); 
					TimeStamp tsExpiry = new TimeStamp();

					SECURITY_STATUS status = InitializeSecurityContext(
						ref hCredential,IntPtr.Zero,szTargetName,ContextReq,0,TargetDataRep,IntPtr.Zero,0,ref hNewContext,ref Output,ref ContextAttr,ref tsExpiry);

					return status;
					
				}
				

				public SECURITY_STATUS AcceptSecurityContext( 
					ref SecHandle hCredential, 
					ref SecBufferDesc Input, 
					SecurityContextRequirement ContextReq, 
					DataRepresentationConstant TargetDataRep, 
					ref SecHandle hNewContext, 
					ref SecBufferDesc Output, 
					out uint ContextAttr, 
					out TimeStamp tsExpiry 
					)
				{
				
					ContextAttr = new uint(); 
					tsExpiry = new TimeStamp();
					IntPtr pInput = Input;
					
					SECURITY_STATUS status = AcceptSecurityContext(
						ref hCredential,IntPtr.Zero,pInput,ContextReq,TargetDataRep,ref hNewContext,ref Output,ref ContextAttr,ref tsExpiry);

					Input = pInput;
					return status;
				}


				public SECURITY_STATUS AcceptSecurityContext( 
					ref SecHandle hCredential, 
					ref SecBufferDesc Input, 
					SecurityContextRequirement ContextReq, 
					DataRepresentationConstant TargetDataRep, 
					ref SecHandle hNewContext, 
					ref SecBufferDesc Output 
					)
				{
				
					uint ContextAttr = new uint(); 
					TimeStamp tsExpiry = new TimeStamp();

					IntPtr pInput = Input;
					
					SECURITY_STATUS status = AcceptSecurityContext(
						ref hCredential,IntPtr.Zero,pInput,ContextReq,TargetDataRep,ref hNewContext,ref Output,ref ContextAttr,ref tsExpiry);

					Input = pInput;
					return status;
				}


				public SECURITY_STATUS AcceptSecurityContext( 
					ref SecHandle hCredential, 
					ref SecHandle hContext, 
					ref SecBufferDesc Input, 
					SecurityContextRequirement ContextReq, 
					DataRepresentationConstant TargetDataRep, 
					ref SecBufferDesc Output, 
					out uint ContextAttr, 
					out TimeStamp tsExpiry 
					)
				{
				
					ContextAttr = new uint(); 
					tsExpiry = new TimeStamp();

					IntPtr pInput = Input;
					
					SECURITY_STATUS status = AcceptSecurityContext(
						ref hCredential,ref hContext,pInput,ContextReq,TargetDataRep,ref hContext,ref Output,ref ContextAttr,ref tsExpiry);
					
					Input = pInput;
					return status;
				}


				public SECURITY_STATUS AcceptSecurityContext( 
					ref SecHandle hCredential, 
					ref SecHandle hContext, 
					ref SecBufferDesc Input, 
					SecurityContextRequirement ContextReq, 
					DataRepresentationConstant TargetDataRep, 
					ref SecBufferDesc Output 
					)
				{
				
					uint ContextAttr = new uint(); 
					TimeStamp tsExpiry = new TimeStamp();

					IntPtr pInput = Input;
				
					SECURITY_STATUS status = AcceptSecurityContext(
						ref hCredential,ref hContext,pInput,ContextReq,TargetDataRep,ref hContext,ref Output,ref ContextAttr,ref tsExpiry);

					Input = pInput;
					return status;
				}
			}
			namespace NTLM
			{
				
				public class AuthenticationClient:SSPAPI,SSPIAuthenticate
				{
					protected SECURITY_STATUS _status;
					protected string _target;
					protected SecHandle _context;
					protected SecPkgInfo _package;
					protected SecHandle _credentials;
					protected bool hasContext = false;
					protected static readonly string defaultPackage = "NTLM";
	
					//CONSTRUCTORS CHAINED TO A SINGLE SIGNATURE

					public  AuthenticationClient():this(AcquireCredentialsHandle(null,defaultPackage,Authentication.SSPI.CredentialUseFlags.SECPKG_CRED_BOTH))
					{
					
					}

					public  AuthenticationClient(string domain, string user, string password):this(new Identity(domain,user,password,AuthIdentityFlags.SEC_WINNT_AUTH_IDENTITY_ANSI))
					{
						
								
					}

					public  AuthenticationClient(string domain, string user, string password,string targetName):this(new Identity(domain,user,password,AuthIdentityFlags.SEC_WINNT_AUTH_IDENTITY_ANSI),targetName)
					{
								
					}

					public  AuthenticationClient(string domain, string user, string password,SecPkgInfo package):this(new Identity(domain,user,password,AuthIdentityFlags.SEC_WINNT_AUTH_IDENTITY_ANSI),package)
					{
								
					}

					public  AuthenticationClient(string domain, string user, string password,SecPkgInfo package,string targetName):this(new Identity(domain,user,password,AuthIdentityFlags.SEC_WINNT_AUTH_IDENTITY_ANSI),package,targetName)
					{
								
					}

					public  AuthenticationClient(Identity identity):this(identity,QuerySecurityPackageInfo(defaultPackage),null)
					{

					}

					public  AuthenticationClient(Identity identity,string targetName):this(identity,QuerySecurityPackageInfo(defaultPackage),targetName)
					{
						
					}

					public  AuthenticationClient(Identity identity,SecPkgInfo package):this(identity,package,null)
					{
			
					}

					public  AuthenticationClient(Identity identity,SecPkgInfo package,string targetName):this(AcquireCredentialsHandle(null,package.Name,Authentication.SSPI.CredentialUseFlags.SECPKG_CRED_BOTH),package,targetName)
					{
						
					}

					public  AuthenticationClient(SecHandle credentials):this(credentials,QuerySecurityPackageInfo(defaultPackage),null)
					{
						
										 
								
					}

					public  AuthenticationClient(SecHandle credentials,string targetName):this(credentials,QuerySecurityPackageInfo(defaultPackage),targetName)
					{
								
					}

					public  AuthenticationClient(SecHandle credentials,SecPkgInfo package):this(credentials,package,null)
					{
								
					}

					public  AuthenticationClient(SecHandle credentials,SecPkgInfo package,string targetName)
					{
						this._package = package;
						this._target = targetName;
						this._credentials = credentials;	
							
					}

					~AuthenticationClient()
					{
						this._status = DeleteSecurityContext(ref this._context);
					}

					public SECURITY_STATUS Status
					{
						get{return this._status;}
					}

					//Authentication Method
					public byte[] Authenticate(byte[] Inbound)
					{
						SecBuffer       ib   = new SecBuffer();
						SecBufferDesc   ibd  = new SecBufferDesc();

						SecBuffer       ob = new SecBuffer();
						SecBufferDesc   obd = new SecBufferDesc();
								
						ob.BufferType = SecBufferFlags.SECBUFFER_TOKEN;
						ob.cbBuffer   = (uint)_package.cbMaxToken;
						ob.Buffer  = System.Runtime.InteropServices.Marshal.AllocHGlobal((int)ob.cbBuffer);
						
						// prepare buffer description
						obd.cBuffers  = 1;
						obd.ulVersion = SecBufferFlags.SECBUFFER_VERSION;
						obd.pSecBuffers  = ob;


						// prepare inbound buffer
						ib.BufferType = SecBufferFlags.SECBUFFER_TOKEN;
						ib.cbBuffer   = (uint)Inbound.Length;
						ib.Buffer     = System.Runtime.InteropServices.Marshal.AllocHGlobal((int)ib.cbBuffer);
					
						System.Runtime.InteropServices.Marshal.Copy(Inbound,0,ib.Buffer,Inbound.Length);
					
						// prepare buffer description
						ibd.cBuffers  = 1;
						ibd.ulVersion = SecBufferFlags.SECBUFFER_VERSION;
						ibd.pSecBuffers  = ib;

						uint CtxtAttr;
						TimeStamp  Expiration;

						this._status = InitializeSecurityContext( 
							ref _credentials,
							ref _context,
							_target,
							SecurityContextRequirement.ISC_REQ_REPLAY_DETECT | SecurityContextRequirement.ISC_REQ_SEQUENCE_DETECT |
							SecurityContextRequirement.ISC_REQ_CONFIDENTIALITY | SecurityContextRequirement.ISC_REQ_DELEGATE, 
							DataRepresentationConstant.SECURITY_NATIVE_DREP,
							ref ibd,
							ref obd,
							out CtxtAttr,
							out Expiration 
							);
						
						if((_status == SECURITY_STATUS.SEC_I_COMPLETE_NEEDED)||(_status == SECURITY_STATUS.SEC_I_COMPLETE_AND_CONTINUE) )
						{
							_status = CompleteAuthToken (ref _context, ref obd );
						}

						ob = obd.pSecBuffers;
						ib = ibd.pSecBuffers;

						byte[] Outbound  = new byte[ob.cbBuffer];
						System.Runtime.InteropServices.Marshal.Copy(ob.Buffer,Outbound,0,(int)ob.cbBuffer);
						System.Runtime.InteropServices.Marshal.FreeHGlobal(ob.Buffer);
						System.Runtime.InteropServices.Marshal.FreeHGlobal(ib.Buffer);
						return Outbound ;
					}

					public byte[] Authenticate()
					{
					
						SecBuffer       ob = new SecBuffer();
						SecBufferDesc   obd = new SecBufferDesc();
								
						ob.BufferType = SecBufferFlags.SECBUFFER_TOKEN;
						ob.cbBuffer   = (uint)_package.cbMaxToken;
						ob.Buffer  = System.Runtime.InteropServices.Marshal.AllocHGlobal((int)ob.cbBuffer);
						// prepare buffer description
						obd.cBuffers  = 1;
						obd.ulVersion = SecBufferFlags.SECBUFFER_VERSION;
						obd.pSecBuffers  = ob;

						uint CtxtAttr;
						TimeStamp  Expiration;

						this._context = new SecHandle();
						this._status = InitializeSecurityContext( 
							ref _credentials,
							_target,
							SecurityContextRequirement.ISC_REQ_REPLAY_DETECT | SecurityContextRequirement.ISC_REQ_SEQUENCE_DETECT |
							SecurityContextRequirement.ISC_REQ_CONFIDENTIALITY | SecurityContextRequirement.ISC_REQ_DELEGATE, 
							DataRepresentationConstant.SECURITY_NATIVE_DREP,
							ref _context,
							ref obd,
							out CtxtAttr,
							out Expiration 
							);
						
						if ((_status == SECURITY_STATUS.SEC_I_COMPLETE_NEEDED)||(_status == SECURITY_STATUS.SEC_I_COMPLETE_AND_CONTINUE) )
						{
							_status = CompleteAuthToken (ref _context, ref obd );
						}

						ob = obd.pSecBuffers;
						byte[] Outbound  = new byte[ob.cbBuffer];
						System.Runtime.InteropServices.Marshal.Copy(ob.Buffer,Outbound,0,(int)ob.cbBuffer);
						System.Runtime.InteropServices.Marshal.FreeHGlobal(ob.Buffer);
						return Outbound ;
					}
				}

				public class AuthenticationServer:SSPAPI,SSPIAuthenticate
				{
			
					protected SECURITY_STATUS _status;
					protected string _target;
					protected SecHandle _context;
					protected SecPkgInfo _package;
					protected SecHandle _credentials;
					protected bool hasContext = false;
					protected static readonly string defaultPackage = "NTLM";
					
					//CONSTRUCTORS CHAINED TO A SINGLE SIGNATURE

					public  AuthenticationServer():this(AcquireCredentialsHandle(null,defaultPackage,Authentication.SSPI.CredentialUseFlags.SECPKG_CRED_INBOUND))
					{

					}

					public  AuthenticationServer(string domain, string user, string password):this(new Identity(domain,user,password,AuthIdentityFlags.SEC_WINNT_AUTH_IDENTITY_ANSI))
					{
						
								
					}

					public  AuthenticationServer(string domain, string user, string password,string targetName):this(new Identity(domain,user,password,AuthIdentityFlags.SEC_WINNT_AUTH_IDENTITY_ANSI),targetName)
					{
								
					}

					public  AuthenticationServer(string domain, string user, string password,SecPkgInfo package):this(new Identity(domain,user,password,AuthIdentityFlags.SEC_WINNT_AUTH_IDENTITY_ANSI),package)
					{
								
					}

					public  AuthenticationServer(string domain, string user, string password,SecPkgInfo package,string targetName):this(new Identity(domain,user,password,AuthIdentityFlags.SEC_WINNT_AUTH_IDENTITY_ANSI),package,targetName)
					{
								
					}

					public  AuthenticationServer(Identity identity):this(identity,QuerySecurityPackageInfo(defaultPackage),null)
					{

					}

					public  AuthenticationServer(Identity identity,string targetName):this(identity,QuerySecurityPackageInfo(defaultPackage),targetName)
					{
						
					}

					public  AuthenticationServer(Identity identity,SecPkgInfo package):this(identity,package,null)
					{

					}

					public  AuthenticationServer(Identity identity,SecPkgInfo package,string targetName):this(AcquireCredentialsHandle(null,package.Name,Authentication.SSPI.CredentialUseFlags.SECPKG_CRED_INBOUND),package,targetName)
					{
						
					}

					public  AuthenticationServer(SecHandle credentials):this(credentials,QuerySecurityPackageInfo(defaultPackage),null)
					{
						
											
								
					}

					public  AuthenticationServer(SecHandle credentials,string targetName):this(credentials,QuerySecurityPackageInfo(defaultPackage),targetName)
					{
								
					}

					public  AuthenticationServer(SecHandle credentials,SecPkgInfo package):this(credentials,package,null)
					{
								
					}

					public  AuthenticationServer(SecHandle credentials,SecPkgInfo package,string targetName)
					{
						this._package = package;
						this._target = targetName;
						this._credentials = credentials;	
							
					}

					~AuthenticationServer()
					{
						this._status = DeleteSecurityContext(ref this._context); 
					}

					public SECURITY_STATUS Status
					{
						get{return this._status;}
					}
				
					//Authentication Methods
					public byte[] Authenticate()
					{
						throw new System.NotImplementedException(); 
					}

					public byte[] Authenticate(byte[] Inbound)
					{
						SecBuffer       ib   = new SecBuffer();
						SecBufferDesc   ibd  = new SecBufferDesc();

						SecBuffer       ob = new SecBuffer();
						SecBufferDesc   obd = new SecBufferDesc();
								
						ob.BufferType = SecBufferFlags.SECBUFFER_TOKEN;
						ob.cbBuffer   = (uint)_package.cbMaxToken;
						ob.Buffer  = System.Runtime.InteropServices.Marshal.AllocHGlobal((int)ob.cbBuffer);
						
						// prepare buffer description
						obd.cBuffers  = 1;
						obd.ulVersion = SecBufferFlags.SECBUFFER_VERSION;
						obd.pSecBuffers  = ob;


						// prepare inbound buffer
						ib.BufferType = SecBufferFlags.SECBUFFER_TOKEN;
						ib.cbBuffer   = (uint)Inbound.Length;
						ib.Buffer     = System.Runtime.InteropServices.Marshal.AllocHGlobal((int)ib.cbBuffer);

						System.Runtime.InteropServices.Marshal.Copy(Inbound,0,ib.Buffer,Inbound.Length);

					
						// prepare buffer description
						ibd.cBuffers  = 1;
						ibd.ulVersion = SecBufferFlags.SECBUFFER_VERSION;
						ibd.pSecBuffers  = ib;

						uint CtxtAttr;
						TimeStamp  Expiration;

						
						if(!hasContext)
						{
							this._context = new SecHandle();
							this._status = AcceptSecurityContext(
								ref _credentials,
								ref ibd,
								SecurityContextRequirement.ISC_REQ_REPLAY_DETECT | SecurityContextRequirement.ISC_REQ_SEQUENCE_DETECT |
								SecurityContextRequirement.ISC_REQ_CONFIDENTIALITY | SecurityContextRequirement.ISC_REQ_DELEGATE, 
								DataRepresentationConstant.SECURITY_NATIVE_DREP,
								ref _context,
								ref obd,
								out CtxtAttr,
								out Expiration 
								);
						}
						else
						{
							this._status = AcceptSecurityContext(
								ref _credentials,
								ref _context,
								ref ibd,
								SecurityContextRequirement.ISC_REQ_REPLAY_DETECT | SecurityContextRequirement.ISC_REQ_SEQUENCE_DETECT |
								SecurityContextRequirement.ISC_REQ_CONFIDENTIALITY | SecurityContextRequirement.ISC_REQ_DELEGATE, 
								DataRepresentationConstant.SECURITY_NATIVE_DREP,
								ref obd,
								out CtxtAttr,
								out Expiration 
								);
						}	
						if((_status == SECURITY_STATUS.SEC_I_COMPLETE_NEEDED)||(_status == SECURITY_STATUS.SEC_I_COMPLETE_AND_CONTINUE) )
						{
							_status = CompleteAuthToken (ref _context, ref obd );
						}

						ob = obd.pSecBuffers;
						ib = ibd.pSecBuffers;

						byte[] Outbound  = new byte[ob.cbBuffer];
						System.Runtime.InteropServices.Marshal.Copy(ob.Buffer,Outbound,0,(int)ob.cbBuffer);
						System.Runtime.InteropServices.Marshal.FreeHGlobal(ob.Buffer);
						System.Runtime.InteropServices.Marshal.FreeHGlobal(ib.Buffer);
						

						if((_status == SECURITY_STATUS.SEC_I_COMPLETE_NEEDED)
							||(_status == SECURITY_STATUS.SEC_I_COMPLETE_AND_CONTINUE)
							||(_status == SECURITY_STATUS.SEC_E_OK)
							||(_status == SECURITY_STATUS.SEC_I_CONTINUE_NEEDED)
							)
						{
							hasContext = true;
						}

						return Outbound ;
					}


				}

			}
		}
	}
}


//[StructLayout(LayoutKind.Explicit)]
/*struct _SecPkgInfo
{
[FieldOffset(0)]public int fCapabilities;        // Capability bitmask
[FieldOffset(4)]public ushort wVersion;            // Version of driver
[FieldOffset(6)]public ushort wRPCID;              // ID for RPC Runtime
[FieldOffset(8)]public int cbMaxToken;           // Size of authentication token (max)
[FieldOffset(12)]public IntPtr  Name;					// Text name
[FieldOffset(16)]public IntPtr Comment;				//Comment
}
//info.Name = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(Info.Name);
//info.Comment = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(Info.Comment);
////Replace these with a delete function//
//System.Runtime.InteropServices.Marshal.FreeCoTaskMem(Info.Name);
//System.Runtime.InteropServices.Marshal.FreeCoTaskMem(Info.Comment);
//////////////////////////////
///
*/