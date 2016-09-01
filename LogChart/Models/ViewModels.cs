using System.Collections.Generic;

namespace LogChart.Models
{
    public class ChartDrawingInfo
    {
        public ChartDrawingInfo(IEnumerable<CallDrawingInfo> calls, IEnumerable<TimePointDrawingInfo> points)
        {
            Calls = calls;
            Points = points;
        }

        public IEnumerable<CallDrawingInfo> Calls { get; }
        public IEnumerable<TimePointDrawingInfo> Points { get; }
    }

    public class TimePointDrawingInfo
    {
        public TimePointDrawingInfo(double displacementFromLeft, string label)
        {
            DisplacementFromLeft = displacementFromLeft;
            Label = label;
        }

        public double DisplacementFromLeft { get; }
        public string Label { get; }
    }

    public class CallDrawingInfo
    {
        public CallDrawingInfo(string callId, int height, double displacementFromLeft, double totalWidth, double ringingWidth, string toolTip)
        {
            CallId = callId;
            Height = height;
            DisplacementFromLeft = displacementFromLeft;
            TotalWidth = totalWidth;
            RingingWidth = ringingWidth;
            ToolTip = toolTip;
        }

        public string CallId { get; }
        public int Height { get; }
        public double DisplacementFromLeft { get; }
        public double TotalWidth { get; }
        public double RingingWidth { get; }
        public string ToolTip { get; }
    }

}