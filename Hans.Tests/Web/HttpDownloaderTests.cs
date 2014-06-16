using FakeItEasy;
using FluentAssertions;
using Hans.FileSystem;
using Hans.Services;
using Hans.Web;
using NUnit.Framework;
using System.IO;
using System.Net;

namespace Hans.Tests.Web
{
    [TestFixture]
    public class HttpDownloaderTests
    {
        [Test]
        public void WhenTheFileCantBeAccessed_TriggersFailEvent()
        {
            //create test class and fakes
            var fileSystem = A.Fake<FileSystem.FileSystem>();
            var httpDownloader = new HttpDownloader();

            //hook event
            var gotCalled = false;
            httpDownloader.Failed += (sender, args) => gotCalled = true;

            //create faked methods
            A.CallTo(() => fileSystem.Exists.File(string.Empty)).WithAnyArguments().Returns(true);
            A.CallTo(() => fileSystem.Can.ReadWrite.File(string.Empty)).WithAnyArguments().Returns(false);

            //run test method
            httpDownloader.Start(new DownloadRequest
            {
                Uri = "webresource.mp3",
                FileSystem = fileSystem,
                DestinationDirectory = "directory",
                FileName = "filename.ext",
                OnlineServiceTrack = A.Fake<IOnlineServiceTrack>()
            });

            //assertions
            gotCalled.Should().BeTrue();
        }
    }
}