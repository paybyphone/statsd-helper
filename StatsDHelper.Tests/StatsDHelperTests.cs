using System;
using FluentAssertions;
using StatsdClient;
using Xunit;

namespace StatsDHelper.Tests
{
    public class StatsDHelperTests : BaseTest
    {
        [Fact]
        public void when_creating_with_missing_app_settings_helper_should_be_a_null_helper()
        {
            var statsDHelper = StatsDHelper.Instance;

            statsDHelper.Should().BeOfType<NullStatsDHelper>();
        }

        [Fact]
        public void when_creating_with_valid_app_settings_helper_should_be_a_real_helper()
        {
            var statsDHelper = StatsDHelper.Instance;

            statsDHelper.Should().BeOfType<StatsDHelper>();
        }
    }
}