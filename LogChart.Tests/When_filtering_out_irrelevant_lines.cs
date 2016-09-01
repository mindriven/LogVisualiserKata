using FluentAssertions;
using LogChart.Models;
using Xunit;

namespace LogChart.Tests
{
    public class When_filtering_out_irrelevant_lines
    {
        [Fact]
        public void then_only_relevant_lines_are_in_result_in_unchanged_order()
        {
            const string line1 = "1266834360 | 1266834354.91 | 9000 | NONE | ENTERQUEUE || anonymous";
            const string line2 = "1266833361 | NONE | NONE | NONE | QUEUESTART |";
            const string line3 = "1266834360 | 1266834354.91 | 9000 | Local / 2001@from -internal/n|RINGNOANSWER|0";
            const string line4 = "1266833407 |NONE|NONE|NONE|CONFIGRELOAD|";

            var result = new[] { line1, line2, line3, line4 }.OnlyRelevantLines();

            result.Should().BeEquivalentTo(new []{ line1, line2, line3 });
        }
    }
}
