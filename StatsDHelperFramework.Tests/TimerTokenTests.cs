using NUnit.Framework;
using Rhino.Mocks;

namespace StatsDHelper.Tests
{
    [TestFixture]
    public class TimerTokenTests
    {
        [Test]
        public void when_token_is_used_in_a_using_block_timing_is_logged()
        {
            var statsDHelper = MockRepository.GenerateStub<IStatsDHelper>();

            using (new TimerToken(statsDHelper, "name"))
            {}

            statsDHelper.AssertWasCalled(o => o.LogTiming(Arg<string>.Is.Anything,Arg<long>.Is.Anything));
        }
    }
}