using FluentAssertions;
using LogChart.Models;
using System;
using Xunit;

namespace LogChart.Tests
{
    public class When_creating_LoggedCall
    {
        [Fact]
        public void and_all_events_are_from_the_same_talk__no_exception_gets_thrown()
        {
            var events = new[] { new LoggedEvent("1266833931 | 1266833925.10 | 9000 | Local / 2001@from -internal/n|RINGNOANSWER|0"),
                                new LoggedEvent("1266833932|1266833925.10|9000|Local/2008@from-internal/n|RINGNOANSWER|1000"),
                                new LoggedEvent("1266833932|1266833925.10|9000|Local/2010@from-internal/n|RINGNOANSWER|1000")};

            Action act = () => new LoggedCall(events);

            act.ShouldNotThrow();
        }

        [Fact]
        public void and_events_are_from_different_talks__no_exception_gets_thrown()
        {
            var events = new[] { new LoggedEvent("1266833931 | 1266833925.10 | 9000 | Local / 2001@from -internal/n|RINGNOANSWER|0"),
                                new LoggedEvent("1266833932|1266833925.44|9000|Local/2008@from-internal/n|RINGNOANSWER|1000"),
                                new LoggedEvent("1266833932|1266833925.10|9000|Local/2010@from-internal/n|RINGNOANSWER|1000")};

            Action act = () => new LoggedCall(events);

            act.ShouldThrow<InvalidOperationException>();
        }
    }

    public class When_having_LoggedCall
    {
        [Fact]
        public void talk_duration_is_calculated_properly()
        {
            var pickedUpEvents = new[] { new LoggedEvent("1266833933|1266833925.10|9000|Local/2005@from-internal/n|RINGNOANSWER|2000"),
                                         new LoggedEvent("1266833933|1266833925.10|9000|Local/2004@from-internal/n|CONNECT|2|1266833933.27"),
                                         new LoggedEvent("1266833937|1266833925.10|9000|Local/2004@from-internal/n|COMPLETECALLER|2|4|1")};

            var pickedCall = new LoggedCall(pickedUpEvents);

            pickedCall.TalkDuration.Should().Be(4);
        }

        [Fact]
        public void talk_duration_is_null_if_call_was_not_picked_up()
        {
            var notPickedUpEvents = new[] { new LoggedEvent("1266833933|1266833925.10|9000|Local/2005@from-internal/n|RINGNOANSWER|2000"),
                                            new LoggedEvent("1266833934 | 1266833925.10 | 9000 | NONE | ABANDON | 2 | 2 | 13")};

            var notPickedCall = new LoggedCall(notPickedUpEvents);

            notPickedCall.TalkDuration.Should().NotHaveValue();
        }

        [Fact]
        public void ringing_duration_is_calculated_properly()
        {
            var pickedUpEvents = new[] { new LoggedEvent("1266833900|1266833925.10|9000|Local/2005@from-internal/n|RINGNOANSWER|2000"),
                                         new LoggedEvent("1266833933|1266833925.10|9000|Local/2004@from-internal/n|CONNECT|2|1266833933.27"),
                                         new LoggedEvent("1266833937|1266833925.10|9000|Local/2004@from-internal/n|COMPLETECALLER|2|4|1")};

            var notPickedUpEvents = new[] { new LoggedEvent("1266833933|1266833925.10|9000|Local/2005@from-internal/n|RINGNOANSWER|2000"),
                                            new LoggedEvent("1266833934 | 1266833925.10 | 9000 | NONE | ABANDON | 2 | 2 | 13")};

            var notPickedCall = new LoggedCall(notPickedUpEvents);
            var pickedCall = new LoggedCall(pickedUpEvents);

            pickedCall.RingingDuration.Should().Be(33);
            notPickedCall.RingingDuration.Should().Be(1);
        }

        [Fact]
        public void event_occurences_are_calculated_properly()
        {
            var events = new[] { new LoggedEvent("1266833900|1266833925.10|9000|Local/2005@from-internal/n|RINGNOANSWER|2000"),
                                         new LoggedEvent("1266833933|1266833925.10|9000|Local/2004@from-internal/n|CONNECT|2|1266833933.27"),
                                         new LoggedEvent("1266833937|1266833925.10|9000|Local/2004@from-internal/n|COMPLETECALLER|2|4|1")};

            var call = new LoggedCall(events);

            call.FirstEventAt.Should().Be(1266833900);
            call.LastEventAt.Should().Be(1266833937);
            call.EventsOccurences.Should().ContainInOrder(1266833900L, 1266833933L, 1266833937L);

        }

        [Fact]
        public void call_identifier_gets_returned_correctly()
        {
            var events = new[] { new LoggedEvent("1266833931 | 1266833925.10 | 9000 | Local / 2001@from -internal/n|RINGNOANSWER|0"),
                                new LoggedEvent("1266833932|1266833925.10|9000|Local/2010@from-internal/n|RINGNOANSWER|1000")};

            var result = new LoggedCall(events);

            result.CallId.Should().Be("1266833925.10");

        }


        [Fact]
        public void it_is_considered_picked_up_only_if_CONNECT_event_occured()
        {
            var pickedUpEvents = new[] { new LoggedEvent("1266833933|1266833925.10|9000|Local/2005@from-internal/n|RINGNOANSWER|2000"),
                                         new LoggedEvent("1266833933|1266833925.10|9000|Local/2004@from-internal/n|CONNECT|2|1266833933.27"),
                                         new LoggedEvent("1266833937|1266833925.10|9000|Local/2004@from-internal/n|COMPLETECALLER|2|4|1")};

            var notPickedUpEvents = new[] { new LoggedEvent("1266833933|1266833925.10|9000|Local/2005@from-internal/n|RINGNOANSWER|2000") };

            var pickedCall = new LoggedCall(pickedUpEvents);
            var notPickedCall = new LoggedCall(notPickedUpEvents);

            pickedCall.WasPickedUp.Should().BeTrue();
            notPickedCall.WasPickedUp.Should().BeFalse();

        }


        [Fact]
        public void it_is_considered_in_progress_if_no_COMPLETECALLER_or_COMPLETEAGENT_or_ABANDON_events_occured()
        {
            var completted1 = new LoggedCall(new[] { new LoggedEvent("1266833933|1266833925.10|9000|Local/2005@from-internal/n|RINGNOANSWER|2000"),
                                                     new LoggedEvent("1266833933|1266833925.10|9000|Local/2004@from-internal/n|CONNECT|2|1266833933.27"),
                                                     new LoggedEvent("1266833937|1266833925.10|9000|Local/2004@from-internal/n|COMPLETECALLER|2|4|1")});

            var completted2 = new LoggedCall(new[] { new LoggedEvent("1266833933|1266833925.10|9000|Local/2005@from-internal/n|RINGNOANSWER|2000"),
                                                     new LoggedEvent("1266833933|1266833925.10|9000|Local/2004@from-internal/n|CONNECT|2|1266833933.27"),
                                                     new LoggedEvent("1266833937|1266833925.10|9000|Local/2004@from-internal/n|COMPLETEAGENT|2|4|1")});


            var completted3 = new LoggedCall(new[] { new LoggedEvent("1266833933|1266833925.10|9000|Local/2005@from-internal/n|RINGNOANSWER|2000"),
                                                     new LoggedEvent("1266833934 | 1266833925.10 | 9000 | NONE | ABANDON | 2 | 2 | 13")});

            var inProgress = new LoggedCall(new[] { new LoggedEvent("1266833933|1266833925.10|9000|Local/2005@from-internal/n|RINGNOANSWER|2000"),
                                                    new LoggedEvent("1266833933|1266833925.10|9000|Local/2004@from-internal/n|CONNECT|2|1266833933.27") });

            completted1.IsInProgress.Should().BeFalse();
            completted2.IsInProgress.Should().BeFalse();
            completted3.IsInProgress.Should().BeFalse();
            inProgress.IsInProgress.Should().BeTrue();

        }
    }
}
