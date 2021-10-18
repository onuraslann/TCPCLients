using SimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPServers
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        SimpleTcpServer server;
        private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start();
            txtInfo.Text += $"Starting...{Environment.NewLine}";
            btnStart.Enabled = false;
            btnSend.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            server = new SimpleTcpServer(txtIP.Text);
            server.Events.ClientConnected += Events_ClientConnected;
            server.Events.ClientDisconnected += Events_ClientDisconnected;
            server.Events.DataReceived += Events_DataReceived;

        }


        private void Events_DataReceived(object sender, DataReceivedFromClientEventArgs e)
        {
           
            this.Invoke((MethodInvoker)delegate
               {
                   txtInfo.Text += $"{e.IpPort}: {Encoding.UTF8.GetString(e.Data)}{Environment.NewLine}";
               });
        }
        private void Events_ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort} disconnected.{Environment.NewLine}";
                IstClientIP.Items.Remove(e.IpPort);
            });
            
        }

        private void Events_ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort} connected.{Environment.NewLine}";
                IstClientIP.Items.Add(e.IpPort);
            });

           
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (server.IsListening)
            {
                if (!string.IsNullOrEmpty(txtMessage.Text) && IstClientIP.SelectedItem != null)
                {
                    server.Send(IstClientIP.SelectedItem.ToString(), txtMessage.Text);
                    txtInfo.Text += $"Server:{txtMessage}{Environment.NewLine}";
                    txtMessage.Text = string.Empty;
                }
            }
        }
    }
}
