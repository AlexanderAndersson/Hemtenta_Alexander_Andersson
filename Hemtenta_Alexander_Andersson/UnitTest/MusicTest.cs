using NUnit.Framework;
using HemtentaTdd2017.music;
using HemtentaTdd2017;
using System;
using System.Collections.Generic;
using System.Linq;
using Moq;

namespace UnitTest
{
    [TestFixture]
    public class MusicTest
    {
        private MusicPlayer mp;
        private Mock<IMediaDatabase> db;
        private SoundMaker sm;

        public static List<ISong> ListOfSongs(string search)
        {
            var songList = new List<ISong> {
            new Song { Title = "Nalle Puh Seglar" },
            new Song { Title = "Hits for kids" },
            new Song { Title = "Markoolio" },
            new Song { Title = "Perikles" },
            new Song { Title = "Dåliga låtar album" }};

            return songList.Where(x => x.Title.Contains(search)).ToList();
        }

        [SetUp]
        public void SetUp()
        {
            db = new Mock<IMediaDatabase>();
            sm = new SoundMaker();
            var fakeDb = db.Object;
            mp = new MusicPlayer(fakeDb, sm);

            db.Setup(x => x.IsConnected).Returns(true); //Låter kopplingen alltid börja med vara öppen
        }
        

        [Test]
        public void NowPlaying_NextSong_Success()
        {
            db.Setup(x => x.FetchSongs("Markoolio")).Returns(ListOfSongs("Markoolio"));
            mp.LoadSongs("Markoolio");
            mp.NextSong();

            Assert.AreEqual("Markoolio", mp.NowPlaying());
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void LoadSongs_IncorrectValues_Failed(string search)
        {
            db.Setup(x => x.IsConnected).Returns(true);
            mp.LoadSongs(search);

            Assert.AreEqual(0, mp.NumSongsInQueue);
        }

        [Test]
        public void OpenConnection_DatabaseAlreadyOpen_Throw()
        {
            db.Setup(x => x.IsConnected).Returns(true);

            if (db.Object.IsConnected == true)
            {
                db.Setup(x => x.OpenConnection()).Throws<DatabaseAlreadyOpenException>();
                Assert.Throws<DatabaseAlreadyOpenException>(() => db.Object.OpenConnection());
            }
        }

        [Test]
        public void LoadMultipleSongs_Success()
        {
            var numberOfSongs = mp.NumSongsInQueue;
            Assert.AreEqual(0, numberOfSongs);

            db.Setup(x => x.FetchSongs("Nalle Puh Seglar")).Returns(ListOfSongs("Nalle Puh Seglar"));
            mp.LoadSongs("Nalle Puh Seglar");
            numberOfSongs = mp.NumSongsInQueue;
            Assert.AreEqual(1, numberOfSongs);

            db.Setup(x => x.FetchSongs("Markoolio")).Returns(ListOfSongs("Markoolio"));
            mp.LoadSongs("Markoolio");
            numberOfSongs = mp.NumSongsInQueue;
            Assert.AreEqual(2, numberOfSongs);
        }

        [Test]
        public void LoadSongs_DatabaseClosed_Throw()
        {
            db.Setup(x => x.IsConnected).Returns(false);
            Assert.Throws<DatabaseClosedException>(() => mp.LoadSongs("Markoolio"));
        }

        [Test]
        public void NowPlaying_Play_Success()
        {
            db.Setup(x => x.FetchSongs("Markoolio")).Returns(ListOfSongs("Markoolio"));
            mp.LoadSongs("Markoolio");
            mp.Play();

            Assert.AreEqual("Markoolio", mp.NowPlaying());
        }

        [Test]
        public void NowPlaying_Stop_Success()
        {
            db.Setup(x => x.FetchSongs("Markoolio")).Returns(ListOfSongs("Markoolio"));
            mp.LoadSongs("Markoolio");
            mp.Play();
            mp.Stop();

            Assert.AreEqual("Tystnad råder", mp.NowPlaying());
        }
    }
}

