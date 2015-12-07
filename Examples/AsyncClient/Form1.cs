using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Net.Sockets;
using DialectSoftware.Networking;

namespace DialectSoftware
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
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
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(92, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "send";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 41);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(268, 208);
            this.textBox1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Client";
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
		
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			// Create a multicast socket.
			Socket mcastSocket = new Socket(AddressFamily.InterNetwork, 
				SocketType.Dgram, 
				ProtocolType.Udp);
              
			mcastSocket.SetSocketOption(SocketOptionLevel.Socket, 
				SocketOptionName.Broadcast,1);

			// Get the local IP address used by the listener and the sender to
			// exchange multicast messages. 
			IPEndPoint IPlocal = new IPEndPoint(IPAddress.Any , 0);
			//EndPoint senderRemote = (EndPoint)IPlocal;
			IPEndPoint IPRemote = new IPEndPoint(IPAddress.Broadcast , 1300);

			// Bind this endpoint to the multicast socket.
			mcastSocket.Bind(IPlocal);

		    			
			//mcastSocket.Connect(IPRemote);
			AsyncSocketAdapter mc = new AsyncSocketAdapter(mcastSocket,new DialectSoftware.Networking.AsyncNetworkErrorCallBack(Error));
	
			
			// Receive brodcast messages.
			mc.BeginWrite(IPRemote,System.Text.ASCIIEncoding.ASCII.GetBytes("Hello World"),new AsyncNetworkCallBack(ThreadProc_WriteComplete),(int)TimeOut.Default);
			
		
		}

		private void ThreadProc_WriteComplete(AsyncNetworkAdapter state)
		{
			EndPoint IPRemote = ((AsyncSocketAdapter)state).RemoteAddress;
			((AsyncSocketAdapter)state).BeginRead(ref IPRemote,new AsyncNetworkCallBack(ThreadProc_ReadComplete),(int)TimeOut.Default);

		}
		private void ThreadProc_ReadComplete(AsyncNetworkAdapter state)
		{
            if (InvokeRequired)
            {
                string text = System.Text.ASCIIEncoding.ASCII.GetString(state.Buffer);
                Invoke(new Action(()=> { 
                    this.textBox1.AppendText(text + "\r\n");
                }));
                
            }
		}

		private void Error(AsyncNetworkAdapter state,System.Exception e)
		{
		}

	}
}
