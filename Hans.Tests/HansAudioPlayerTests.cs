using FakeItEasy;
using NUnit.Framework;

namespace Hans.Tests
{
    [TestFixture]
    public class HansAudioPlayerTests
    {
        [Test]
        public void CanSearchServices()
        {
            var service = A.Fake<IOnlineService>();
            var hansAudioPlayer = new HansAudioPlayer();
            hansAudioPlayer.Search(new SearchRequest
            {
                OnlineService = service,
                Query = string.Empty
            });
            A.CallTo(() => service.Search(string.Empty)).MustHaveHappened();
        }
    }
}
