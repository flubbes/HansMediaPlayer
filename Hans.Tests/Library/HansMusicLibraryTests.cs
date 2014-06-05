using System;
using System.Configuration;
using System.Linq;
using System.Net.Mime;
using FakeItEasy;
using FluentAssertions;
using Hans.Database;
using Hans.General;
using Hans.Library;
using NUnit.Framework;

namespace Hans.Tests.Library
{
    [TestFixture]
    public class HansMusicLibraryTests
    {
        private HansMusicLibrary _musicLibrary;
        private IDatabaseSaver _databaseSaver;

        [SetUp]
        public void SetUp()
        {
            _databaseSaver = A.Fake<IDatabaseSaver>();
            var exitTrigger = A.Fake<ExitTrigger>();
            _musicLibrary = new HansMusicLibrary(_databaseSaver, exitTrigger);
        }

        [Test]
        public void CanCreatePlayList()
        {
            _musicLibrary.CreatePlayList("name");
            _musicLibrary.Playlists.Any().Should().BeTrue();
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

        [Test]
        public void CanSaveDatabase()
        {
            var databaseSaver = A.Fake<IDatabaseSaver>();
            _musicLibrary.SaveDatabase(databaseSaver);
            A.CallTo(() => databaseSaver.Save(_musicLibrary, string.Empty)).WithAnyArguments().MustHaveHappened();
        }
    }
}
