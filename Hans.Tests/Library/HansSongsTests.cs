using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Hans.Library;
using NAudio.Wave;
using NUnit.Framework;

namespace Hans.Tests.Library
{
    [TestFixture]
    public class HansSongsTests
    {
        [Test]
        public void CanCreateHansSong()
        {
            new HansSong("path");
        }

        [Test]
        public void CanPrepareFile()
        {
            var hansSong = new HansSong("path");
            var fakeAudioFileReader = A.Fake<AudioFileReader>();
            A.CallTo(() => fakeAudioFileReader.Length).Returns(1337);
            A.CallTo(() => new AudioFileReader("path")).Returns(fakeAudioFileReader);
            hansSong.PrepareToPlay();
            
        }
    }
}
