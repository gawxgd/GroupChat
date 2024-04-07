using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace GroupChat
{
    public class Connection
    {
        Form1 form;
        connectionDialogBox connectbox;
        CancellationToken token;
        public ConnectionClient cli;
        public string userName;
        public Connection(Form1 form, connectionDialogBox connectbox,CancellationToken token) 
        {
            this.form = form;
            this.connectbox = connectbox;
            this.token = token;
        }
        public bool ConnectionRun()
        {
            IPHostEntry entry;
            try
            {
                entry = Dns.GetHostEntry(connectbox.textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to resolve address");
                connectbox.DialogResult = DialogResult.No;
                return false;
            }
            Int32 port;
            try
            {
                port = Int32.Parse(connectbox.textBox2.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to parse port number");
                connectbox.DialogResult = DialogResult.No;
                return false;
            }
            IPAddress ip = entry.AddressList[0];
            TcpClient tcpClient = new TcpClient();
            ConnectionClient client;
            try
            {
                connectbox.ChangeProgressbar(25);
                Task ConnectionTask = tcpClient.ConnectAsync(ip, port);
                connectbox.DissableConnecrtionButton(true);
                client = new ConnectionClient(tcpClient);
            }
            catch(Exception ex)
            {
                connectbox.ChangeProgressbar(0);
                MessageBox.Show("server not started");
                connectbox.DialogResult = DialogResult.No;
                return false;
            }
            
            try
            {
                return AuthorizeClient(client);
            }
            catch(Exception ex) 
            {
                connectbox.DialogResult = DialogResult.No;
                return false;
            }

        }
        private bool AuthorizeClient(ConnectionClient client)
        {
            Messages.Authorization Auth = new Messages.Authorization(connectbox.textBox3.Text, connectbox.textBox4.Text);
            string authmes = SerialOps.SerializeToJson(Auth);
            ((TextWriter)client.Writer).WriteLine(authmes);
            connectbox.ChangeProgressbar(50);
            ((TextWriter)client.Writer).Flush();
            connectbox.ChangeProgressbar(75);
            string recived = ((TextReader)client.Reader).ReadLine();
            if (recived is null)
            {
                connectbox.ChangeProgressbar(0);
                MessageBox.Show("authorization failed");
                connectbox.DialogResult = DialogResult.No;
                connectbox.CloseBox(true);
                return false;
            }
            connectbox.ChangeProgressbar(100);
            MessageBox.Show("authorized");
            connectbox.DialogResult = DialogResult.Yes;
            //connectbox.CloseBox(true);
            cli = client;
            userName = connectbox.textBox3.Text;
            MessageOps(client);
            return true;
        }
        private void MessageOps(ConnectionClient client)
        {
            try
            {
                while (!token.IsCancellationRequested && client.Connected)
                {
                    string RecivedMessage = ((TextReader)client.Reader).ReadLine();
                    if(!(RecivedMessage is null)) 
                    {
                        Messages.Message recivedOG = SerialOps.DeserializeFromJsonMes(RecivedMessage);
                        form.Invoke((Action)(() => form.createBubble(recivedOG.Sender, recivedOG.Text, recivedOG.Time)));

                    }
                }
            }
            catch (Exception ex) 
            {

            }
        }
    }
}
