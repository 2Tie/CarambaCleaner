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
        Player player;

        public Form1()
        {
            InitializeComponent();

            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            Text = "Game Window";

            ClientSize = new Size(800, 600);

            Draw.init(ClientSize.Width, ClientSize.Height);
            BackgroundImage = Draw.getBuffer();

            KeyDown += new KeyEventHandler(keyHandler);
        }

        public void CoreLoop()
        {
            Inputs.Init();
            Audio.Init();
            GlobalTimer.timer = new Stopwatch();
            GlobalTimer.timer.Start();
            long startTime, interval;
            int cycle = 1;
            while (Created)
            {
                startTime = GlobalTimer.timer.ElapsedMilliseconds;

                gameLogic();
                gameRender();

                interval = 16 + (cycle % 1);
                cycle++;
                if (cycle == 4) cycle = 1;

                Application.DoEvents();
                while (GlobalTimer.timer.ElapsedMilliseconds - startTime < interval) ;
            }
        }

        public void gameRender()
        {
            Draw.context.FillRectangle(Draw.b, ClientRectangle);
            Draw.context.FillRectangle(Draw.w, 10, 10, 50, 60);

            if (player != null)
                player.Draw();
        }

        public void gameLogic()
        {
            //handle inputs?
            if (Inputs.inputBuffer[0].button != Inputs.Button.NONE)
            {
                if(Inputs.inputBuffer[0].button == Inputs.Button.A && player == null)
                {
                    player = new Player();
                }
                else
                Audio.sysBeep.Play();
            }
            Inputs.inputBuffer[0].button = Inputs.Button.NONE;

            if (player != null)
                player.Tick();
        }

        void keyHandler(object sender, KeyEventArgs e)
        {
            Keys k = e.KeyCode;
            for (int i = 0; i < Inputs.keys.Length; i++)
            {
                if (k == Inputs.keys[i])
                {
                    Inputs.Add((Inputs.Button)i, GlobalTimer.timer.ElapsedMilliseconds);
                    break;
                }
            }
            e.Handled = true;
        }
    }
}
