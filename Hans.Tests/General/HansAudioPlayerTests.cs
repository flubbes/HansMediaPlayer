using FakeItEasy;
using Hans.Audio;
using Hans.FileSystem;
using Hans.General;
using Hans.Library;
using Hans.Services;
using Hans.Web;
using NUnit.Framework;

namespace Hans.Tests.General
{
    [TestFixture]
    public class HansAudioPlayerTests
    {
        [Test]
        public void CanSearchServices()
        {
            var service = A.Fake<IOnlineService>();
            var hansMusicLibrary = A.Fake<HansMusicLibrary>();
            var hansAudioPlayer = new HansAudioPlayer(
                hansMusicLibrary,
                A.Fake<IAudioPlayer>(),
                A.Fake<SongDownloads>(),
                A.Fake<FileSystem.FileSystem>());

            hansAudioPlayer.Search(new SearchRequest
            {
                OnlineService = service,
                Query = string.Empty
            });
            A.CallTo(() => service.Search(string.Empty)).MustHaveHappened();
        }
    }
}