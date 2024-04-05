using System.Runtime.InteropServices;
using System.Text;

namespace GroupChat
{
    public partial class Form1 : Form
    {
        private int messageCount = 0;
        public Form1()
        {
            InitializeComponent();
            tableLayoutPanel2.RowStyles.Clear();
            tableLayoutPanel2.Controls.Clear();
            tableLayoutPanel2.AutoScroll = true;
            tableLayoutPanel2.HorizontalScroll.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            createBubble();
        }
        private void createBubble()
        {
            string author = "not you";
            if (messageCount % 2 == 0)
            {
                author = "you";
            }
            bubble b = new bubble(author, textBox1.Text, DateTime.Now);
            if (messageCount % 2 == 0)
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
            textBox1.Clear();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                createBubble();
            }
        }
    }
}