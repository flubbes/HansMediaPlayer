using Hans.Components.Library;
using NUnit.Framework;

namespace Hans.Components.Tests.Library
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
