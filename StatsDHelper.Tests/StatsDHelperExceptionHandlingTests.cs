using System;
using FakeItEasy;
using FluentAssertions;
using StatsdClient;
using Xunit;

namespace StatsDHelper.Tests
{
    public class StatsDHelperExceptionHandlingTests
    {
        [Fact]
        public void when_client_logcount_throws_exception_should_be_handled()
        {
            var statsDClient = A.Fake<IStatsd>();
            var prefixProvider = A.Fake<IPrefixProvider>();
            
            var statsDHelper = new StatsDHelper(prefixProvider, statsDClient);

            A.CallTo(() => statsDClient.LogCountAsync(A<string>._, A<long>._)).Throws<Exception>();

            statsDHelper.LogCount("name", 3); //Should not throw Exception
        }

        [Fact]
        public void when_client_loggauge_throws_exception_should_be_handled()
        {
            var statsDClient = A.Fake<IStatsd>();
            var prefixProvider = A.Fake<IPrefixProvider>();

            var statsDHelper = new StatsDHelper(prefixProvider, statsDClient);

            A.CallTo(() => statsDClient.LogGaugeAsync(A<string>._, A<long>._)).Throws<Exception>();

            statsDHelper.LogGauge("name", 3);
        }

        [Fact]
        public void when_client_logtiming_throws_exception_should_be_handled()
        {
            var statsDClient = A.Fake<IStatsd>();
            var prefixProvider = A.Fake<IPrefixProvider>();

            var statsDHelper = new StatsDHelper(prefixProvider, statsDClient);

            A.CallTo(() => statsDClient.LogTimingAsync(A<string>._, A<long>._)).Throws<Exception>();

            statsDHelper.LogTiming("name", 3);
        }

        [Fact]
        public void when_client_logset_throws_exception_should_be_handled()
        {
            var statsDClient = A.Fake<IStatsd>();
            var prefixProvider = A.Fake<IPrefixProvider>();

            var statsDHelper = new StatsDHelper(prefixProvider, statsDClient);

            A.CallTo(() => statsDClient.LogSetAsync(A<string>._, A<long>._)).Throws<Exception>();

            statsDHelper.LogSet("name", 3);
        }
    }
}