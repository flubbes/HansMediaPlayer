using FakeItEasy;
using FluentAssertions;
using Hans.Library;
using NUnit.Framework;

namespace Hans.Tests.Library
{
    [TestFixture]
    public class FolderAnalyzerTests
    {
        private FolderAnalyzer _folderAnalyzer;

        [SetUp]
        public void SetUp()
        {
            _folderAnalyzer = new FolderAnalyzer();
        }
    }
}
