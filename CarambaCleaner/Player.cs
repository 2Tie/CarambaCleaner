using System;
using System.Collections.Generic;
using System.Text;

namespace CarambaCleaner
{
    class Player
    {
        Track track;
        int points;
        Tiers tier;
        long startTime;
        long progress; //milliseconds since song start

        enum Tiers{ Awful, Bad, Okay, Cool }

        public Player()
        {
            track = new Track();
            track.Load("test");
            tier = Tiers.Okay;
            points = 0;
            startTime = GlobalTimer.timer.ElapsedMilliseconds;
        }

        public void Tick()
        {
            //process the past frame of input, play notes, give points, etc
        }

        public void Draw()
        {
            //draw note interface, points, etc
        }
    }
}
