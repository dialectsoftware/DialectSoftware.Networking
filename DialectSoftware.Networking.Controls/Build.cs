using System;
using System.IO;
using System.Collections;
using System.Security;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

/// ******************************************************************************************************************
/// * Copyright (c) 2011 Dialect Software LLC                                                                        *
/// * This software is distributed under the terms of the Apache License http://www.apache.org/licenses/LICENSE-2.0  *
/// *                                                                                                                *
/// ******************************************************************************************************************

namespace DialectSoftware.Networking
{
	namespace Controls
	{
		public enum TimeOut:int
		{
			Default = 30000
		}

		public enum BuildAdpaterState:int
		{
			Idle = 0,
			Connecting  = 1,
			Authenticating = 2,
			Negotiating = 3,
			Scheduling  = 4,
			Processing  = 5,
			Terminating = 6
		}

		public class AsyncBuildAdapter:AsyncTcpClientAdapter
		{
			private Build _command = null;
			private IBuildFactory _factory = null;
			private BuildAdpaterState _buildState = BuildAdpaterState.Idle;
			private Security.Authentication.SSPI.SSPIAuthenticate _securityServer = null;

			public Build Command
			{
				get{return _command;}
				set{_command = value;}
			}

			public BuildAdpaterState BuildState
			{
				set{_buildState = value;}
				get{return _buildState;}
			}

			public Security.Authentication.SSPI.SSPIAuthenticate SecurityServer
			{
				get{return _securityServer;}
			}

			public IBuildFactory Factory
			{
				set{_factory = value;}
				get{return _factory;}
			}

			public AsyncBuildAdapter(Security.Authentication.SSPI.SSPIAuthenticate SecurityServer,TcpClient socket, AsyncNetworkErrorCallBack Error):base(socket,Error)
			{
				this._securityServer = SecurityServer;
			}
		}

	}

	public struct SpecialFolders
	{
		public static string BuildFolder = "{32fbb9e3-09e0-4c41-ac1d-b69bd248c7d0}";
	}


	public interface IBuildFactory
	{
		void GetBuild(object Data,Build build);
	}

	public interface IBuildPattern
	{   
		string Execute();
	}


	[Serializable()]
	public abstract class BuildObject
	{
		protected object _id = null;
		public BuildObject()
		{	
		}

		public object ID
		{
			set{this._id = value;}
			get{return this._id;}
		}

		~BuildObject()
		{
			if(_id != null && _id.GetType().GetInterface(typeof(System.IDisposable).ToString())!=null)
				_id.GetType().GetInterface(typeof(System.IDisposable).ToString()).GetMethod("Dispose").Invoke(_id,null);
		}

	}


	[Serializable()]
	public class Project:BuildObject
	{
		private Project _parent = null;
		protected internal FileArrayList _files = null;
		protected internal ProjectArrayList _subprojects = null;

		//constructor logic here
		public Project():base()
		{
			_files = new FileArrayList();
			_subprojects = new ProjectArrayList();
			
		}

		public void Add(Project project)
		{
			project._parent = this;
			_subprojects.Add(project);
		}

	
		public void Add(System.Windows.Forms.ListViewItem Files)
		{
			_files.Add(Files);

		}

		public FileArrayList Files
		{
			get{return _files;}		
		}


		public ProjectArrayList SubProjects
		{
			get{return _subprojects;}		
		}

		~Project()
		{
			_files.Clear();
		}
	}

	
	[Serializable()]
	public class ProjectArrayList:System.Collections.ArrayList
	{
		//constructor logic here
		public ProjectArrayList():base()
		{
		}

		public int Add(Project project)
		{
			return base.Add(project);
		}
	
		public override object this[int index]
		{
			get{return base[index];}
		}

		~ProjectArrayList()
		{
			this.Clear();
		}
	}


	[Serializable()]
	public class FileArrayList:System.Collections.ArrayList
	{
		//constructor logic here
		public FileArrayList():base()
		{
		}

		public int Add(System.Windows.Forms.ListViewItem file)
		{
			return base.Add(file);
		}
	
		public override object this[int index]
		{
			get{return base[index];}
		}

		~FileArrayList()
		{
			this.Clear();
		}
	}


	[Serializable()]
	public abstract class Build:BuildObject,IBuildPattern
	{
		private ProjectArrayList _projects = null;
	
		//constructor logic here
		public Build():base()
		{
			_projects = new ProjectArrayList();
		}

		public void Add(Project project)
		{
			_projects.Add(project);	
		}

		public ProjectArrayList Projects
		{
			get{return _projects;}		
		}

		public abstract string Execute();

		~Build()
		{
		
		}

	}


	public abstract class BuildFactory:IBuildFactory
	{
		public BuildFactory()
		{
		}

		public abstract void GetBuild(object Data,Build build);

	}
}
