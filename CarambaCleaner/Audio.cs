using System;
using System.Collections.Generic;
using System.Media;
using System.Text;

namespace CarambaCleaner
{
    public static class Audio
    {
        public static SoundPlayer sysBeep;

        //track-specific collections, to be populated and played by the track
        static SoundPlayer track; //the background song
        static SoundPlayer[] sampleCollection1; //the guide's samples
        static SoundPlayer[] sampleCollection2; //the player's samples

        public static void Init()
        {
            sysBeep = new SoundPlayer("corebeep.wav");
            sysBeep.Load();
        }
    }
}
