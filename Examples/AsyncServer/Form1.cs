using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Net.Sockets;
using DialectSoftware.Networking;

namespace AsyncServer
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
        private TextBox textBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(2, 5);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(285, 258);
            this.textBox1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			IPHostEntry hostEntry = Dns.Resolve(Dns.GetHostName());
			IPEndPoint endPoint = new IPEndPoint(hostEntry.AddressList[0], 1300);

			Socket s = new Socket(endPoint.Address.AddressFamily,
				SocketType.Dgram,
				ProtocolType.Udp);
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			// Creates an IpEndPoint to capture the identity of the sending host.
			IPEndPoint ipsender = new IPEndPoint(IPAddress.Any, 1300);
			//EndPoint senderRemote = (EndPoint)ipsender;
    
			// Binding is required with ReceiveFrom calls.
			s.Bind(ipsender);
			AsyncSocketAdapter mc = new AsyncSocketAdapter (s,new DialectSoftware.Networking.AsyncNetworkErrorCallBack(Error));
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ThreadProc_Accept),mc);
		}

        private  void ThreadProc_Accept(object stateInfo)
		{
			IPEndPoint sender = new IPEndPoint(IPAddress.Any, 1300);
			EndPoint senderRemote = (EndPoint)sender;
			((AsyncSocketAdapter)stateInfo).BeginRead(ref senderRemote,new AsyncNetworkCallBack(ThreadProc_ReadComplete),(int)TimeOut.Default); 
		}

		private  void ThreadProc_Accept(AsyncNetworkAdapter state)
		{
			EndPoint IPRemote = ((AsyncSocketAdapter)state).RemoteAddress;
			((AsyncSocketAdapter)state).Read(ref IPRemote,(int)TimeOut.Default); 
		}

		private  void ThreadProc_ReadComplete(AsyncNetworkAdapter state)
		{
            var text = System.Text.ASCIIEncoding.ASCII.GetString(state.Buffer);
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    this.textBox1.AppendText( text + "\r\n");

                }));
            }
			//IPHostEntry hostEntry = Dns.Resolve(Dns.GetHostName())[0];
			((AsyncSocketAdapter)state).BeginWrite(((AsyncSocketAdapter)state).RemoteAddress ,System.Text.ASCIIEncoding.ASCII.GetBytes(this.GetHashCode().ToString() + ":" + Dns.Resolve(Dns.GetHostName()).AddressList[0].ToString()),new AsyncNetworkCallBack(ThreadProc_Accept),(int)TimeOut.Default); 
		}

		private void Error(AsyncNetworkAdapter state,System.Exception e)
		{
		}


	}
}
