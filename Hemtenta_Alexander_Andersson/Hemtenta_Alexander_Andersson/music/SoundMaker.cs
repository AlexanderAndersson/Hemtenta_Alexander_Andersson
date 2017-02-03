using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017.music
{
    public class SoundMaker : ISoundMaker
    {
        string currentSong = "Tystnad råder";

        public string NowPlaying
        {
            get { return currentSong; }
        }

        public void Play(ISong song)
        {
            currentSong = song.Title;
        }

        public void Stop()
        {
            currentSong = "Tystnad råder";
        }
    }
}
