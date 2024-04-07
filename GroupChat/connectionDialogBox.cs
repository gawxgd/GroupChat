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
        Form1 mainForm;
        public connectionDialogBox(Form1 mainForm)
        {
            InitializeComponent();
            progressBar1.Value = 0;
            this.mainForm = mainForm;
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
            mainForm.CreateConnectionTask();
        }
        public void ChangeProgressbar(int value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<int>(ChangeProgressbar), new object[] { value });
                return;
            }
            progressBar1.Value = value;
        }
        public void DissableConnecrtionButton(bool value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<bool>(DissableConnecrtionButton), new object[] { value });
                return;
            }
            button1.Enabled = !value;
        }
        public void CloseBox(bool value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<bool>(CloseBox), new object[] { value });
                return;
            }
            if (value == true)
                Close();
        }
    }
}
