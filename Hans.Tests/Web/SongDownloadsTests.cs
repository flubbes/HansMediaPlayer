using System;
using System.Linq;
using System.Threading;
using FakeItEasy;
using FluentAssertions;
using Hans.General;
using Hans.Web;
using NUnit.Framework;

namespace Hans.Tests.Web
{
    [TestFixture]
    public class SongDownloadsTests
    {
        private SongDownloads _songDownloads;
        private IDownloader _downloaderFake;

        [SetUp]
        public void SetUp()
        {
            _songDownloads = new SongDownloads(A.Fake<ExitAppTrigger>());
            _downloaderFake = A.Fake<IDownloader>();
        }

        [Test]
        public void CanQueueItem()
        {
            _songDownloads.Start(new DownloadRequest
            {
                Downloader = _downloaderFake
            });
            _songDownloads.ActiveDownloads.Any().Should().BeTrue();
        }

        [Test]
        public void StartsTheDownload()
        {
            var downloadRequest = new DownloadRequest
            {
                Downloader = _downloaderFake
            };
            _songDownloads.Start(downloadRequest);
            A.CallTo(() => downloadRequest.Downloader.Start(downloadRequest)).MustHaveHappened();
        }

        [Test]
        public void WhenDownloadIsFinished_TriggersEvent()
        {
            var downloader = A.Fake<IDownloader>();
            var downloadRequest = new DownloadRequest
            {
                Downloader = downloader
            };
            var checker = A.Fake<Action>();
            
            
            var songDownloads = new SongDownloads(A.Fake<ExitAppTrigger>());
            songDownloads.DownloadFinished += (sender, args) => checker.Invoke();
            A.CallTo(() => downloadRequest.Downloader.Progress).Returns(100);
            songDownloads.Start(downloadRequest);
            Thread.Sleep(400); //wait for the thread -.-' ugly shit
            A.CallTo(() => checker.Invoke()).MustHaveHappened();
        }
    }
}
