using FakeItEasy;
using FluentAssertions;
using Hans.Components.Library;
using Hans.Core.Database.Playlists;
using Hans.Core.Database.Songs;
using Hans.Core.SongData;
using NUnit.Framework;
using System.Linq;

namespace Hans.Tests.Library
{
    [TestFixture]
    public class HansMusicLibraryTests
    {
        private HansMusicLibrary _musicLibrary;
        private IPlaylistStore _playListStore;
        private ISongStore _songStore;

        [Test]
        public void CanAddSongsToTheLibrary()
        {
            _musicLibrary.AddSong(new HansSong(""));
            A.CallTo(() => _songStore.Add(default(HansSong))).WithAnyArguments().MustHaveHappened();
        }

        [Test]
        public void CanCreatePlayList()
        {
            _musicLibrary.CreatePlayList("name");
            A.CallTo(() => _playListStore.Add(default(HansPlaylist))).WithAnyArguments().MustHaveHappened();
        }

        [Test]
        public void CanRemoveSong()
        {
            var hansSong = new HansSong("");
            _musicLibrary.AddSong(hansSong);
            _musicLibrary.RemoveSong(hansSong);
            _musicLibrary.Songs.Any().Should().BeFalse();
        }

        [SetUp]
        public void SetUp()
        {
            _playListStore = A.Fake<IPlaylistStore>();
            _songStore = A.Fake<ISongStore>();
            _musicLibrary = new HansMusicLibrary(_playListStore, _songStore, A.Fake<ISongDataFinder>());
        }
    }
}