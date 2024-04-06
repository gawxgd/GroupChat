using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupChat
{
    public partial class connectionDialogBox : Form
    {
        public connectionDialogBox()
        {
            InitializeComponent();
            progressBar1.Value = 0;
        }
        private bool Connection()
        {
            IPHostEntry entry;
            try
            {
                entry = Dns.GetHostEntry(textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to resolve address");
                return false;
            }
            Int32 port;
            try
            {
                port = Int32.Parse(textBox2.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to parse port number");
                return false;
            }
            IPAddress ip = entry.AddressList[0];
            TcpClient tcpClient = new TcpClient();
            progressBar1.Value = 25;
            Task ConnectionTask = tcpClient.ConnectAsync(ip, port);
            button1.Enabled = false;
            ConnectionClient client = new ConnectionClient(tcpClient);
            return AuthorizeClient(client);


        }
        private bool AuthorizeClient(ConnectionClient client)
        {
            Messages.Authorization Auth = new Messages.Authorization(textBox3.Text, textBox4.Text);
            string authmes = SerialOps.SerializeToJson(Auth);
            ((TextWriter)client.Writer).WriteLine(authmes);
            progressBar1.Value = 50;
            ((TextWriter)client.Writer).Flush();
            progressBar1.Value = 75;
            string recived = ((TextReader)client.Reader).ReadLine();
            if (recived is null)
            {
                progressBar1.Value = 0;
                MessageBox.Show("authorization failed");
                DialogResult = DialogResult.No;
                Close();
                return false;
            }
            progressBar1.Value = 100;
            MessageBox.Show("authorized");
            DialogResult = DialogResult.Yes;
            Close();
            return true;
        }
        private void connectionDialogBox_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connection();
        }
    }
}
