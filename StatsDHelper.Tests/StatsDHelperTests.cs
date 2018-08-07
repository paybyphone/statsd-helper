﻿using System;
using FluentAssertions;
using NUnit.Framework;
using Rhino.Mocks;
using StatsdClient;

namespace StatsDHelper.Tests
{
    [TestFixture]
    public class StatsDHelperTests : BaseTest
    {
        [Test]
        public void when_creating_with_missing_app_settings_helper_should_be_a_null_helper()
        {
            EmptyAppSettings();

            var statsDHelper = StatsDHelper.Instance;

            statsDHelper.Should().BeOfType<NullStatsDHelper>();
        }

        [Test]
        public void when_creating_with_valid_app_settings_helper_should_be_a_real_helper()
        {
            PopulateAppSettings();

            var statsDHelper = StatsDHelper.Instance;

            statsDHelper.Should().BeOfType<StatsDHelper>();
        }
    }
}