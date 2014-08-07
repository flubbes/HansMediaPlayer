using FakeItEasy;
using Hans.Components.General;
using Hans.Components.Library;
using Hans.Core.Audio;
using Hans.Core.Services;
using Hans.Core.Web;
using NUnit.Framework;

namespace Hans.Components.Tests.General
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
                A.Fake<Core.FileSystem.FileSystem>());

            hansAudioPlayer.Search(new SearchRequest
            {
                OnlineService = service,
                Query = string.Empty
            });
            A.CallTo(() => service.Search(string.Empty)).MustHaveHappened();
        }
    }
}