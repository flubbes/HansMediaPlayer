using FluentAssertions;
using Hans.Core.General;
using NUnit.Framework;

namespace Hans.Tests.General
{
    [TestFixture]
    public class AppTriggerTests
    {
        [Test]
        public void TriggersEventCorrectly()
        {
            var appTrigger = new ExitAppTrigger();
            var triggered = false;
            appTrigger.GotTriggered += delegate { triggered = true; };
            appTrigger.Trigger();
            triggered.Should().BeTrue();
        }
    }
}