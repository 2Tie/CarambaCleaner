using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CarambaCleaner
{
    public static class Inputs
    {
        public struct Input
        {
            public Button button; //what was pressed
            public long timestamp; //when it was pressed, in milliseconds since start of song
        }

        public enum Button { NONE = -1, A, B, X, Y, L1, R1}
        public static Keys[] keys = { Keys.L, Keys.P, Keys.K, Keys.O, Keys.Q, Keys.W }; //feel free to change defaults, should also be configurable in the future

        public static Input[] inputBuffer;

        public static void Init()
        {
            inputBuffer = new Input[8];
            for (int i = 0; i < inputBuffer.Length; i++)
                inputBuffer[i] = new Input { button = Button.NONE, timestamp = 0 };
        }

        public static void Add(Button b, long time)
        {
            for (int i = 0; i < inputBuffer.Length; i++)
            {
                if (inputBuffer[i].button == Button.NONE)
                {
                    inputBuffer[i] = new Input { button = b, timestamp = time };
                    break;
                }
            }
        }
    }
}
