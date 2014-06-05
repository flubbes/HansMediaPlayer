using FluentAssertions;
using Hans.General;
using NUnit.Framework;

namespace Hans.Tests.General
{
    [TestFixture]
    public class AppTriggerTests
    {
        [Test]
        public void TriggersEventCorrectly()
        {
            var appTrigger = new AppTrigger();
            var triggered = false;
            appTrigger.GotTriggered += () => triggered = true;
            appTrigger.Trigger();
            triggered.Should().BeTrue();
        }
    }
}
