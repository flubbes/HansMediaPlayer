using System.Linq;
using FakeItEasy;
using FluentAssertions;
using Hans.Database.Playlists;
using Hans.Database.Songs;
using Hans.Library;
using Hans.SongData;
using NUnit.Framework;

namespace Hans.Tests.Library
{
    [TestFixture]
    public class HansMusicLibraryTests
    {
        private HansMusicLibrary _musicLibrary;
        private IPlaylistStore _playListStore;

        [SetUp]
        public void SetUp()
        {
            _playListStore = A.Fake<IPlaylistStore>();
            _musicLibrary = new HansMusicLibrary(_playListStore);
        }

        [Test]
        public void CanCreatePlayList()
        {
            _musicLibrary.CreatePlayList("name");
            A.CallTo(() => _playListStore.Add(default(HansPlaylist))).WithAnyArguments().MustHaveHappened();
        }

        [Test]
        public void CanAddSongsToTheLibrary()
        {
            _musicLibrary.AddSong(new HansSong(""));
            _musicLibrary.Songs.Any().Should().BeTrue();
        }

        [Test]
        public void CanRemoveSong()
        {
            var hansSong = new HansSong("");
            _musicLibrary.AddSong(hansSong);
            _musicLibrary.RemoveSong(hansSong);
            _musicLibrary.Songs.Any().Should().BeFalse();
        }
    }
}
