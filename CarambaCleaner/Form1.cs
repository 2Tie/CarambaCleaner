using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

            Draw.init(ClientSize.Width, ClientSize.Height);
            BackgroundImage = Draw.getBuffer();
        }

        public void CoreLoop()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            long startTime, interval;
            int cycle = 1;
            while (Created)
            {
                startTime = timer.ElapsedMilliseconds;

                gameLogic();
                gameRender();

                interval = 16 + (cycle % 1);
                cycle++;
                if (cycle == 4) cycle = 1;

                Application.DoEvents();
                while (timer.ElapsedMilliseconds - startTime < interval) ;
            }
        }

        public void gameRender()
        {
            Draw.context.FillRectangle(Draw.b, ClientRectangle);
            Draw.context.FillRectangle(Draw.w, 10, 10, 50, 60);
        }

        public void gameLogic()
        {

        }
    }
}
