using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017.music
{
    public class MusicPlayer : IMusicPlayer
    {
        private IList<ISong> listOfSongs;
        private IMediaDatabase md;
        private SoundMaker sm;

        public MusicPlayer(IMediaDatabase mediaDatabase, SoundMaker soundMaker)
        {
            this.listOfSongs = new List<ISong>();
            this.md = mediaDatabase;
            this.sm = soundMaker;
        }

        public int NumSongsInQueue
        {
            get
            {
                return listOfSongs.Count;
            }
        }

        public void LoadSongs(string search)
        {
            if (md.IsConnected == false)
                throw new DatabaseClosedException();

            if (!string.IsNullOrEmpty(search))
            {
                var newSongs = md.FetchSongs(search);
                foreach (var song in newSongs)
                {
                    listOfSongs.Add(song);
                }
            }
        }

        public void Play()
        {
            if (md.IsConnected == false)
                throw new DatabaseClosedException();

            if (sm.NowPlaying == "Tystnad råder")
            {
                if (listOfSongs.Count > 0)
                    sm.Play(listOfSongs.FirstOrDefault());
            }
        }

        public void NextSong()
        {
            if (listOfSongs.Count == 0)
                Stop();

            else if (sm.NowPlaying == "Tystnad råder")
                Play();

            else
            {
                listOfSongs.FirstOrDefault();
                Stop();
                Play();
            }
        }

        public string NowPlaying()
        {
            return sm.NowPlaying;
        }

        public void Stop()
        {
            if (sm.NowPlaying == "Tystnad råder") { /*Gör inget om ingen låt spelas*/ }

            else
                sm.Stop();
        }
    }
}
