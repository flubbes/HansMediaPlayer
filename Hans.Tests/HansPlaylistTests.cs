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
            hansplaylist.Add();
        }
    }

    public class HansPlaylist
    {

    }
}