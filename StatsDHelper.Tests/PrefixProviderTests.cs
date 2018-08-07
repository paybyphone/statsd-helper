using FluentAssertions;
using NUnit.Framework;
using Rhino.Mocks;

namespace StatsDHelper.Tests
{
    public class PrefixProviderTests : BaseTest
    {
        [Test]
        public void when_getting_a_prefix_returned_prefix_is_formed_correctly()
        {
            PopulateAppSettings();

            var hostPropertiesProvider = MockRepository.GenerateStub<IHostPropertiesProvider>();

            hostPropertiesProvider.Stub(o => o.GetDomainName())
                .Return("test.com");

            hostPropertiesProvider.Stub(o => o.GetHostName())
                .Return("red-iis008");

            var prefixProvider = new PrefixProvider(hostPropertiesProvider);

            var result = prefixProvider.GetPrefix();

            result.Should().Be(string.Format("com.test.red-iis008.{0}",TestApplicationName));
        }
    }
}