using System;
using FluentAssertions;
using NUnit.Framework;
using Rhino.Mocks;
using StatsdClient;

namespace StatsDHelper.Tests
{
    [TestFixture]
    public class StatsDHelperExceptionHandlingTests
    {
        [Test]
        public void when_client_logcount_throws_exception_should_be_handled()
        {
            var statsDClient = MockRepository.GenerateStub<IStatsd>();
            var prefixProvider = MockRepository.GenerateStub<IPrefixProvider>();

            var statsDHelper = new StatsDHelper(prefixProvider, statsDClient);

            statsDClient.Stub(o => o.LogCount(Arg<string>.Is.Anything, Arg<int>.Is.Anything)).Throw(new Exception());

            statsDHelper.Invoking(o => o.LogCount("name", 3))
                .ShouldNotThrow();
        }

        [Test]
        public void when_client_loggauge_throws_exception_should_be_handled()
        {
            var statsDClient = MockRepository.GenerateStub<IStatsd>();
            var prefixProvider = MockRepository.GenerateStub<IPrefixProvider>();

            var statsDHelper = new StatsDHelper(prefixProvider, statsDClient);

            statsDClient.Stub(o => o.LogGauge(Arg<string>.Is.Anything, Arg<int>.Is.Anything)).Throw(new Exception());

            statsDHelper.Invoking(o => o.LogGauge("name", 3))
                .ShouldNotThrow();
        }

        [Test]
        public void when_client_logtiming_throws_exception_should_be_handled()
        {
            var statsDClient = MockRepository.GenerateStub<IStatsd>();
            var prefixProvider = MockRepository.GenerateStub<IPrefixProvider>();

            var statsDHelper = new StatsDHelper(prefixProvider, statsDClient);

            statsDClient.Stub(o => o.LogTiming(Arg<string>.Is.Anything, Arg<int>.Is.Anything)).Throw(new Exception());

            statsDHelper.Invoking(o => o.LogTiming("name", 3))
                .ShouldNotThrow();
        }

        [Test]
        public void when_client_logset_throws_exception_should_be_handled()
        {
            var statsDClient = MockRepository.GenerateStub<IStatsd>();
            var prefixProvider = MockRepository.GenerateStub<IPrefixProvider>();

            var statsDHelper = new StatsDHelper(prefixProvider, statsDClient);

            statsDClient.Stub(o => o.LogSet(Arg<string>.Is.Anything, Arg<int>.Is.Anything)).Throw(new Exception());

            statsDHelper.Invoking(o => o.LogSet("name", 3))
                .ShouldNotThrow();
        }
    }
}