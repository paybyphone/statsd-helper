using System;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace StatsDHelper.Tests
{
    public class PrefixProviderTests : BaseTest
    {
        [Fact]
        public void when_getting_a_prefix_returned_prefix_is_formed_correctly()
        {
            var hostPropertiesProvider = A.Fake<IHostPropertiesProvider>();

            A.CallTo(() => hostPropertiesProvider.GetDomainName()).Returns("test.com");
            A.CallTo(() => hostPropertiesProvider.GetHostName()).Returns("red-iis008");

            var prefixProvider = new PrefixProvider(hostPropertiesProvider);

            var statsDHelperConfig = new StatsDHelperConfig()
            {
                ApplicationName = TestApplicationName,  
            };
            var result = prefixProvider.GetPrefix(statsDHelperConfig);

            result.Should().Be(string.Format("com.test.red-iis008.{0}",TestApplicationName));
        }

        [Fact]
        public void get_prefix_with_null_config_throws()
        {
            var hostPropertiesProvider = A.Fake<IHostPropertiesProvider>();

            A.CallTo(() => hostPropertiesProvider.GetDomainName()).Returns("test.com");
            A.CallTo(() => hostPropertiesProvider.GetHostName()).Returns("red-iis008");

            var prefixProvider = new PrefixProvider(hostPropertiesProvider);

            Assert.Throws<InvalidOperationException>(() => prefixProvider.GetPrefix(null));
        }
    }
}