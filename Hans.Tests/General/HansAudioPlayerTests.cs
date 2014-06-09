using FakeItEasy;
using Hans.General;
using Hans.Library;
using Hans.Services;
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
            var hansAudioPlayer = new HansAudioPlayer(hansMusicLibrary, A.Fake<IAudioLoader>(), A.Fake<IAudioPlayer>());
            hansAudioPlayer.Search(new SearchRequest
            {
                OnlineService = service,
                Query = string.Empty
            });
            A.CallTo(() => service.Search(string.Empty)).MustHaveHappened();
        }
    }
}