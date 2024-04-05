using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupChat
{
    public partial class bubble : UserControl
    {
        public bubble(string author, string text, DateTime time)
        {
            InitializeComponent();
            authorLabel.Text = author;
            textBox1.Text = text;
            timeLabel.Text = time.ToString("HH:mm");
            //this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void bubble_Resize(object sender, EventArgs e)
        {
            int textHeight = TextRenderer.MeasureText(textBox1.Text, textBox1.Font, textBox1.ClientSize, TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl).Height;
            this.Height = textHeight + authorLabel.Height + timeLabel.Height + 20;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
         int nLeftRect,     // x-coordinate of upper-left corner
         int nTopRect,      // y-coordinate of upper-left corner
         int nRightRect,    // x-coordinate of lower-right corner
         int nBottomRect,   // y-coordinate of lower-right corner
         int nWidthEllipse, // width of ellipse
         int nHeightEllipse // height of ellipse
        );
    }
}
