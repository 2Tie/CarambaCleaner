using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CarambaCleaner
{
    public static class Draw
    {
        static Image buffer;
        public static Graphics context;

        public static SolidBrush b = new SolidBrush(Color.Black);
        public static SolidBrush w = new SolidBrush(Color.White);

        public static void init(int width, int height)
        {
            buffer = new Bitmap(width, height);
            context = Graphics.FromImage(buffer);
        }

        public static Image getBuffer()
        {
            return buffer;
        }
    }
}
