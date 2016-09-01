using FluentAssertions;
using LogChart.Models;
using System.Linq;
using Xunit;

namespace LogChart.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public void when_calculating_display_info_it_works_correctly()
        {
            var input = new[] {
            "0|1266833905.2|9000|NONE|ENTERQUEUE||01773597776",
            "0|1266833905.2|9000|Local/2001@from-internal/n|RINGNOANSWER|0",
            "10|1266833905.2|9000|Local/2008@from-internal/n|RINGNOANSWER|0",
            "20|1266833905.2|9000|Local/2004@from-internal/n|CONNECT|1|1266833911.7",
            "30|1266833905.2|9000|Local/2004@from-internal/n|COMPLETEAGENT|1|3|1",
            "40|1266833925.10|9000|NONE|ENTERQUEUE||01773597776",
            "40|1266833925.10|9000|Local/2001@from-internal/n|RINGNOANSWER|0",
            "50|1266833925.10|9000|Local/2008@from-internal/n|RINGNOANSWER|1000",
            "50|1266833925.10|9000|Local/2010@from-internal/n|RINGNOANSWER|1000",
            "50|1266833925.10|9000|Local/2007@from-internal/n|RINGNOANSWER|1000",
            "50|1266833925.10|9000|Local/2006@from-internal/n|RINGNOANSWER|1000",
            "60|1266833925.10|9000|Local/2002@from-internal/n|RINGNOANSWER|2000",
            "60|1266833925.10|9000|Local/2009@from-internal/n|RINGNOANSWER|2000",
            "60|1266833925.10|9000|Local/2005@from-internal/n|RINGNOANSWER|2000",
            "60|1266833925.10|9000|Local/2004@from-internal/n|CONNECT|2|1266833933.27",
            "100|1266833925.10|9000|Local/2004@from-internal/n|COMPLETECALLER|2|4|1"};

            var result = input.ToDrawingInfos();
            result.Points.Should().HaveCount(8);
            result.Calls.Should().HaveCount(2);

            var firstCall = result.Calls.First();
            firstCall.Height.Should().Be(1);
            firstCall.DisplacementFromLeft.Should().Be(0);
            firstCall.TotalWidth.Should().Be(30);
            firstCall.RingingWidth.Should().Be(20);

            var secondCall = result.Calls.Last();
            secondCall.Height.Should().Be(1);
            secondCall.DisplacementFromLeft.Should().Be(40);
            secondCall.TotalWidth.Should().Be(60);
            secondCall.RingingWidth.Should().Be(20);
        }


        [Fact]
        public void number_of_people_needed_to_serve_all_calls_is_calculated_correctly()
        {
            var input = new[] {
            "0  |A|9000|NONE|ENTERQUEUE||01773597776",
            "0  |A|9000|Local/2001@from-internal/n|RINGNOANSWER|0",
            "10 |A|9000|Local/2008@from-internal/n|RINGNOANSWER|0",
            "20 |A|9000|Local/2004@from-internal/n|CONNECT|1|1266833911.7",
            "40 |B|9000|NONE|ENTERQUEUE||01773597776",
            "40 |B|9000|Local/2001@from-internal/n|RINGNOANSWER|0",
            "50 |B|9000|Local/2008@from-internal/n|RINGNOANSWER|1000",
            "50 |B|9000|Local/2010@from-internal/n|RINGNOANSWER|1000",
            "50 |B|9000|Local/2007@from-internal/n|RINGNOANSWER|1000",
            "60 |C|9000|Local/2002@from-internal/n|RINGNOANSWER|2000",
            "60 |C|9000|Local/2009@from-internal/n|RINGNOANSWER|2000",
            "70 |C|9000|Local/2005@from-internal/n|RINGNOANSWER|2000",
            "70 |C|9000|Local/2004@from-internal/n|CONNECT|2|1266833933.27",
            "80 |B|9000|Local/2006@from-internal/n|ABANDON|1000",
            "90 |A|9000|Local/2004@from-internal/n|COMPLETEAGENT|1|3|1",
            "100|C|9000|Local/2004@from-internal/n|COMPLETECALLER|2|4|1"};

            var result = input.ToDrawingInfos();

            result.Calls.Should().HaveCount(3);

            var callA = result.Calls.First();
            callA.Height.Should().Be(1);
            callA.DisplacementFromLeft.Should().Be(0);
            callA.TotalWidth.Should().Be(90);
            callA.RingingWidth.Should().Be(20);

            var callB = result.Calls.Skip(1).First();
            callB.Height.Should().Be(2);
            callB.DisplacementFromLeft.Should().Be(40);
            callB.TotalWidth.Should().Be(40);
            callB.RingingWidth.Should().Be(40);

            var callC = result.Calls.Skip(2).First();
            callC.Height.Should().Be(3);
            callC.DisplacementFromLeft.Should().Be(60);
            callC.TotalWidth.Should().Be(40);
            callC.RingingWidth.Should().Be(10);
        }
    }
}
