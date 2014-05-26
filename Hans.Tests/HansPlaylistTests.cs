using FakeItEasy;
using NUnit.Framework;

namespace Hans.Tests
{
    [TestFixture]
    public class HansPlaylistTests
    {
        [Test]
        public void CanCreateHansPlaylist()
        {
            new HansPlaylist();
        }

        [Test]
        public void CanAddSongs()
        {
            var hansplaylist = new HansPlaylist();
            var hansSong = A.Fake<HansSong>();
            hansplaylist.Add(hansSong);
        }
    }

    public class HansPlaylist
    {
        public void Add(HansSong hansSong)
        {
            throw new System.NotImplementedException();
        }
    }
}