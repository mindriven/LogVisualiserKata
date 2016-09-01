using System;
using System.Collections.Generic;
using System.Linq;
namespace LogChart.Models
{
    public class LoggedCall
    {
        private IEnumerable<LoggedEvent> events;
        public LoggedCall(IEnumerable<LoggedEvent> events) {
            var howManyDifferentIds = events.Select(x => x.CallId).Distinct().Count();
            if (howManyDifferentIds != 1)
                throw new InvalidOperationException("Tried to create a call with events from multiple calls!");
            this.events = events;
        }

        public string CallId => events.First().CallId;
        public bool IsInProgress => EndingEvent == null;
        public bool WasPickedUp => ConnectEvent != null;
        public long? TalkDuration => HangUpEvent?.OccuredAt - ConnectEvent?.OccuredAt;
        public long RingingDuration => (WasPickedUp ? ConnectEvent.OccuredAt : events.Last().OccuredAt) - events.First().OccuredAt;
        public long FirstEventAt => events.First().OccuredAt;
        public long LastEventAt => events.Last().OccuredAt;
        public IEnumerable<long> EventsOccurences => events.Select(x=>x.OccuredAt).Distinct();

        private static string[] hangUpEventsIdentifiers = new string[] { "COMPLETEAGENT", "COMPLETECALLER" };
        private static string[] endingEventsIdentifiers = hangUpEventsIdentifiers.Union(new[] { "ABANDON" }).ToArray();
        private LoggedEvent ConnectEvent => events.SingleOrDefault(x => x.EventTypeIdentifier == "CONNECT");
        private LoggedEvent HangUpEvent => events.SingleOrDefault(x => hangUpEventsIdentifiers.Contains(x.EventTypeIdentifier));
        private LoggedEvent EndingEvent => events.SingleOrDefault(x => endingEventsIdentifiers.Contains(x.EventTypeIdentifier));
        private IEnumerable<LoggedEvent> RingingEvents => events.Where(x => x.EventTypeIdentifier == "RINGNOANSWER");
    }
}