using FluentAssertions;
using LogChart.Models;
using Xunit;

namespace LogChart.Tests
{
    public class When_having_LoggedEvent
    {
        [Theory]
        [InlineData("1266834005|1266833996.31|9000|Local/2007@from-internal/n|RINGNOANSWER|3000", 1266834005, "1266833996.31", "RINGNOANSWER")]
        [InlineData("1266834038|1266833996.30|9000|Local/2004@from-internal/n|CONNECT|36|1266834033.87", 1266834038, "1266833996.30", "CONNECT")]
        [InlineData("1266834046|1266833996.30|9000|Local/2004@from-internal/n|COMPLETEAGENT|36|8|1", 1266834046, "1266833996.30", "COMPLETEAGENT")]
        public void It_can_parse_the_input_parameter_correctly(string input, long occuredAt, string callId, string eventTypeId)
        {
            var result = new LoggedEvent(input);

            result.OccuredAt.Should().Be(occuredAt);
            result.CallId.Should().Be(callId);
            result.EventTypeIdentifier.Should().Be(eventTypeId);
        }
    }
}
