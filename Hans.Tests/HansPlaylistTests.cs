using System.Linq;
using FakeItEasy;
using FluentAssertions;
using Hans.Library;
using NUnit.Framework;

namespace Hans.Tests
{
    [TestFixture]
    public class HansPlaylistTests
    {
        private HansPlaylist _hansPlaylist;
        private HansSong _hansSong;

        [SetUp]
        public void SetUp()
        {
            _hansPlaylist = new HansPlaylist("test");
            _hansSong = A.Fake<HansSong>();
        }

        [Test]
        public void CanAddSongs()
        {
            _hansPlaylist.Add(_hansSong);
            _hansPlaylist.Songs.Any().Should().BeTrue();
        }

        [Test]
        public void CanRemoveSong()
        {
            _hansPlaylist.Add(_hansSong);
            _hansPlaylist.Remove(_hansSong);
            _hansPlaylist.Songs.Any().Should().BeFalse();
        }

        [Test]
        public void PlaylistNameIsSettedInConstructor()
        {
            var hansPlayList = new HansPlaylist("playlist");
            hansPlayList.Name.Should().Be("playlist");
        }
    }
}