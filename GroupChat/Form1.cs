using System.Net.Quic;
using System.Runtime.InteropServices;
using System.Text;

namespace GroupChat
{
    public partial class Form1 : Form
    {
        Task OlgierdTask;

        private int messageCount = 0;
        bool connection = false;
        Task ConnectionTask;
        Connection ConnectionObject;
        Label connectLabel;
        CancellationTokenSource cancellationTokenSource;
        CancellationToken cancellationToken;
        string nick = "You";
        public Form1()
        {
            InitializeComponent();
            tableLayoutPanel2.RowStyles.Clear();
            tableLayoutPanel2.Controls.Clear();
            tableLayoutPanel2.AutoScroll = true;
            tableLayoutPanel2.HorizontalScroll.Visible = false;
            disconnectToolStripMenuItem.Enabled = false;
          
            OlgierdTask = Task.Factory.StartNew(() => { });
        }
        public void CreateConnectionTask()
        {
            ConnectionTask = new Task(() => ConnectionObject.ConnectionRun());
            ConnectionTask.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            createBubble(nick,textBox1.Text,DateTime.Now);
            if (connection)
            {
                Messages.Message mes = new Messages.Message(ConnectionObject.userName, textBox1.Text, DateTime.Now);
                AsyncOlgierd(mes, ConnectionObject.cli);
            }
            textBox1.Clear();
        }
        public void createBubble(string author,string text, DateTime time)
        {
            bubble b = new bubble(author,text, time);
            if (author == nick)
            {
                b.Margin = new Padding(50, 0, 0, 0);
                b.Dock = DockStyle.Right;
            }
            else
            {
                b.Margin = new Padding(0, 0, 50, 0);
                b.Dock = DockStyle.Left;
            }
            b.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
            tableLayoutPanel2.Controls.Add(b, 0, messageCount);
            tableLayoutPanel2.ScrollControlIntoView(b);
            messageCount++;
        }
        public void createPiwo()
        {
            piwo PIWO = new piwo(this);
            tableLayoutPanel2.Controls.Add(PIWO, 0, messageCount++);
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);

            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //disconnect

            Application.Exit();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {

            connectionDialogBox connectionBox = new connectionDialogBox(this);
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            ConnectionObject = new Connection(this, connectionBox,cancellationToken);
            DialogResult res = connectionBox.ShowDialog();
            if (res == DialogResult.Yes)
            {
                disconnectToolStripMenuItem.Enabled = true;
                connectToolStripMenuItem.Enabled = false;
                connectLabel = new Label();
                connectLabel.Text = "Connected";
                connectLabel.ForeColor = System.Drawing.Color.DarkGray;
                connectLabel.TextAlign = ContentAlignment.MiddleCenter;
                connectLabel.Dock = DockStyle.Top;
                tableLayoutPanel2.Controls.Add(connectLabel, 0, 0);
                connection = true;


            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AsyncOlgierd(new Messages.Message(ConnectionObject.userName, "Disconnected", DateTime.Now),ConnectionObject.cli);
            cancellationTokenSource.Cancel();
            connectLabel.Text = "Disconnected";
            connectToolStripMenuItem.Enabled = true;
            disconnectToolStripMenuItem.Enabled = false;
            connection = false;


            // stop thread
        }
        private void SendMesagePackage(Messages.Message message, ConnectionClient client)
        {
            try
            {
                string og_olgierd = SerialOps.SerializeToJson(message);
                ((TextWriter)client.Writer).WriteLine(og_olgierd);
                ((TextWriter)client.Writer).Flush();
            }
            catch (Exception ex)
            {


                //logBox.AppendText(ex.Message);

            }
        }
        public async Task AsyncOlgierd(Messages.Message message, ConnectionClient client)
        {
            await OlgierdTask.ContinueWith((Action<Task>)(task => SendMesagePackage(message, client)));
        }
        public void IdziemyNaPiwo()
        {
            AsyncOlgierd(new Messages.Message("PIWO", "dobra idziemy", DateTime.Now),ConnectionObject.cli);
            createBubble(nick,"dobra idziemy",DateTime.Now);
        }
    }
}