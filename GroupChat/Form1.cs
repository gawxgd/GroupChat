using System.Net.Quic;
using System.Runtime.InteropServices;
using System.Text;

namespace GroupChat
{
    public partial class Form1 : Form
    {
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
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
        }
        public void CreateConnectionTask()
        {
            ConnectionTask = new Task(() => ConnectionObject.ConnectionRun());
            ConnectionTask.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            createBubble(nick,textBox1.Text,DateTime.Now);
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

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                createBubble(nick, textBox1.Text, DateTime.Now);
                textBox1.Clear();

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


            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
            connectLabel.Text = "Disconnected";
            // stop thread
        }
    }
}