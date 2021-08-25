using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarambaCleaner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            Text = "Game Window";

            ClientSize = new Size(800, 600);

            Image imgBuf = new Bitmap(ClientSize.Width, ClientSize.Height);
            Graphics g = Graphics.FromImage(imgBuf);

            SolidBrush b = new SolidBrush(Color.Black);

            g.FillRectangle(b, ClientRectangle);

            BackgroundImage = imgBuf;
            Invalidate();
        }

    }
}
