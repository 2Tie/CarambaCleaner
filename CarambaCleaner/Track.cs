using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CarambaCleaner
{
    class Track
    {
        /*
         * Tracks start with a lesson intro, which consist of four teacher measures; all notes from teacher and player will still be processed (and all button notes hit) within them.
         * Teacher measures are played before the corresponding Player measures; these could be abstracted further to support 2P play and alternate tracks
         * ChangeNoteSet should be used whenever the samples used change, ideally at the start of the measure.
         * MeasureNext will end the track's measure on the beat set and immediately start the next measure; this is a manual override for if/when you need it, as measures should(?) automatically end after eight beats
         * LessonEnd will trigger the next Lesson Intro regardless of where and on which track it's placed; in normal cases this will be on the end of a Player measure
         */


        public string Name;

        public double BPM;

        public double StartWait; //the length of time to wait before the first lesson is triggered, in milliseconds

        struct Measure
        {
            public TrackNote[] notes;
        }

        struct TrackNote
        {
            public NoteType type;
            public double offset; //length in quarter notes from the start of the measure
            public int data; //button type, ID of note to tie, ETC
        }

        enum NoteType
        {
            Button, //buttons
            MeasureNext, //this loads the next measure wherever it's placed
            LessonEnd, //this ends the measure and lesson wherever it's placed
            ChangeNoteSet, //changes the samples linked to buttons
            TiedNote //plays note if last note was hit
        }

        struct ButtonNoteSet
        {
            public int[] samples; //keeps track of one per button
        }

        ButtonNoteSet[] playerNoteSets; //all of the note sets used in the song by the player
        ButtonNoteSet[] teacherNoteSets; //all of the note sets used in the song by the teacher

        Measure[] playerTrack;
        Measure[] teacherTrack;

        //private variables to keep track of sample loading
        int samplecounter;


        public bool Load(string trackname)
        {
            string dir = "songs/" + trackname;
            if(!Directory.Exists(dir))
            {
                Debug.WriteLine("Directory " + dir + " does not exist!");
                return false;
            }
            if(!File.Exists(dir + "/track.dat"))
            {
                Debug.WriteLine("Track file missing in folder " + dir + "!");
                return false;
            }
            bool success = false;
            int samplesCount1 = 0;
            int samplesCount2 = 0;
            using(BinaryReader track = new BinaryReader(File.OpenRead(dir+"/track.dat")))
            {
                Debug.WriteLine("Loading track.dat info");
                //file is opened
                //first byte is version number, verify it's the current one (in the future we can support older formats by adding blocks for different values)
                if (track.ReadByte() == 0)
                {
                    //basic info
                    Name = track.ReadString();
                    BPM = track.ReadDouble();
                    StartWait = track.ReadDouble();
                    //get sample counts
                    samplesCount1 = track.ReadInt32();
                    samplesCount2 = track.ReadInt32();
                    //populate note sets
                    playerNoteSets = new ButtonNoteSet[track.ReadInt32()];
                    for(int i = 0; i < playerNoteSets.Length; i++)
                    {
                        playerNoteSets[i] = new ButtonNoteSet
                        {
                            samples = new int[] {
                                track.ReadInt32(), //O
                                track.ReadInt32(), //X
                                track.ReadInt32(), //T
                                track.ReadInt32(), //S
                                track.ReadInt32(), //L
                                track.ReadInt32() //R
                            }
                        };
                    }
                    teacherNoteSets = new ButtonNoteSet[track.ReadInt32()];
                    for (int i = 0; i < playerNoteSets.Length; i++)
                    {
                        teacherNoteSets[i] = new ButtonNoteSet
                        {
                            samples = new int[] {
                                track.ReadInt32(), //O
                                track.ReadInt32(), //X
                                track.ReadInt32(), //T
                                track.ReadInt32(), //S
                                track.ReadInt32(), //L
                                track.ReadInt32() //R
                            }
                        };
                    }
                    //now populate the track notes
                    playerTrack = new Measure[track.ReadInt32()];
                    for (int i = 0; i < playerTrack.Length; i++)
                    {
                        playerTrack[i] = new Measure { notes = new TrackNote[track.ReadInt32()] };
                        for (int n = 0; n < playerTrack[i].notes.Length; n++)
                        {
                            playerTrack[i].notes[n] = new TrackNote { 
                                type = (NoteType)track.ReadByte(), 
                                offset = track.ReadDouble(), 
                                data = track.ReadInt32() 
                            };
                        }
                    }
                    teacherTrack = new Measure[track.ReadInt32()];
                    for (int i = 0; i < teacherTrack.Length; i++)
                    {
                        teacherTrack[i] = new Measure { notes = new TrackNote[track.ReadInt32()] };
                        for (int n = 0; n < teacherTrack[i].notes.Length; n++)
                        {
                            teacherTrack[i].notes[n] = new TrackNote
                            {
                                type = (NoteType)track.ReadByte(),
                                offset = track.ReadDouble(),
                                data = track.ReadInt32()
                            };
                        }
                    }
                    //now that those are set, we can fall out
                    Debug.WriteLine("track.dat loaded");
                    success = true;
                }
            }

            //now load up the samples!
            Audio.track = new System.Media.SoundPlayer(dir + "/track.wav");
            Audio.track.Load();
            //allocate sample space
            Audio.sampleCollection1 = new System.Media.SoundPlayer[samplesCount1];
            Audio.sampleCollection2 = new System.Media.SoundPlayer[samplesCount2];
            samplecounter = 0;
            for (int i = 0; i < samplesCount1; i++)
            {
                Audio.sampleCollection1[i].SoundLocation = dir + "t" + i;
                Audio.sampleCollection1[i].LoadCompleted += SampleLoaded;
                Audio.sampleCollection1[i].LoadAsync();
            }
            while (samplecounter < samplesCount1)
                System.Threading.Thread.Sleep(50);//give up some time??
            //now the first batch of samples is loaded, load second now
            samplecounter = 0;
            for (int i = 0; i < samplesCount2; i++)
            {
                Audio.sampleCollection2[i].SoundLocation = dir + "p" + i;
                Audio.sampleCollection2[i].LoadCompleted += SampleLoaded;
                Audio.sampleCollection2[i].LoadAsync();
            }
            while (samplecounter < samplesCount2)
                System.Threading.Thread.Sleep(50);//give up some time??

            return success;
        }

        private void SampleLoaded(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            samplecounter++;
        }
    }
}
