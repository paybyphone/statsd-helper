using System;
using FakeItEasy;
using FluentAssertions;
using StatsdClient;
using Xunit;

namespace StatsDHelper.Tests
{
    public class StatsDHelperTests : BaseTest, IDisposable
    {
        [Fact]
        public void when_creating_with_missing_app_settings_helper_should_be_a_null_helper()
        {
            StatsDHelper.Configure(new StatsDHelperConfig());
            var statsDHelper = StatsDHelper.Instance;

            Assert.IsType<NullStatsDHelper>(statsDHelper);
        }

        [Fact]
        public void when_creating_with_valid_app_settings_helper_should_be_a_real_helper()
        {
            StatsDHelper.Configure(StatsDHelperConfig);
            var statsDHelper = StatsDHelper.Instance;

            Assert.IsType<StatsDHelper>(statsDHelper);
        }

        [Fact]
        public void when_calling_instance_before_configuring_helper_should_throw()
        {
            Assert.Throws<InvalidOperationException>(() => StatsDHelper.Instance);
        }

        public void Dispose()
        {
            StatsDHelper.Cleanup();
        }
    }
}