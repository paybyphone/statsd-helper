using FakeItEasy;
using Xunit;

namespace StatsDHelper.Tests
{
    public class TimerTokenTests
    {
        [Fact]
        public void when_token_is_used_in_a_using_block_timing_is_logged()
        {
            var statsDHelper = A.Fake<IStatsDHelper>();

            using (new TimerToken(statsDHelper, "name"))
            {}

            A.CallTo(() => statsDHelper.LogTiming(A<string>._, A<long>._)).MustHaveHappened();
        }
    }
}