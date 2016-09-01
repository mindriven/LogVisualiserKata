using System.Collections.Generic;
using System.Linq;

namespace LogChart.Models
{
    public static class LogToDrawingInforTransformation
    {
        public static ChartDrawingInfo ToDrawingInfos(this IEnumerable<string> lines)
        {
            var relevantLines = lines.OnlyRelevantLines();
            var events = relevantLines.Select(x => new LoggedEvent(x));
            var eventsByCall = events.GroupBy(x => x.CallId);
            var calls = eventsByCall.Select(x => new LoggedCall(x));
            return calls.ToDrawingInfos();
        }

        public static ChartDrawingInfo ToDrawingInfos(this IEnumerable<LoggedCall> calls)
        {
            double beginningOfTime = calls.Min(x => x.FirstEventAt);
            double endOfTime = calls.Max(x => x.LastEventAt);
            double xAxisLength = endOfTime - beginningOfTime;
            var callsDrawingInfos = calls.Select(x => new CallDrawingInfo(x.CallId,
                                                        height: calls.Count(y => y.FirstEventAt <= x.FirstEventAt && y.LastEventAt >= x.FirstEventAt),
                                                        displacementFromLeft: (x.FirstEventAt - beginningOfTime) / xAxisLength * 100,
                                                        totalWidth: (x.LastEventAt - x.FirstEventAt) / xAxisLength * 100,
                                                        ringingWidth: x.RingingDuration / xAxisLength * 100,
                                                        toolTip: $"From {x.FirstEventAt} to {x.LastEventAt}. Was {(x.WasPickedUp ? string.Empty : "NOT")} picked up."
                                                        ));
            var pointsDrawingInfos = calls.SelectMany(x => x.EventsOccurences).Distinct()
                                          .Select(x => new TimePointDrawingInfo(
                                                displacementFromLeft: (x - beginningOfTime) / xAxisLength * 100,
                                                label: x.ToString()
                                          ));
            return new ChartDrawingInfo(callsDrawingInfos, pointsDrawingInfos);
        }

        public static IEnumerable<string> OnlyRelevantLines(this IEnumerable<string> allLines)
        {
            return allLines.Where(x => !string.IsNullOrEmpty(x) && !x.Contains("|CONFIGRELOAD|") && !x.Contains("|QUEUESTART|"));
        }
    }
}