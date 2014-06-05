using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using FluentAssertions;
using Hans.Library;
using NAudio.Wave;
using NUnit.Framework;
using NUnit.Framework.Constraints;

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
    }
}
