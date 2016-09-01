using System.Linq;

namespace LogChart.Models
{
    public class LoggedEvent
    {
        private string[] splittedLine;
        public LoggedEvent(string line)
        {
            splittedLine = line.Split('|').Select(x=>x.Trim()).ToArray();
        }

        public long OccuredAt => long.Parse(splittedLine[0]);
        public string CallId => splittedLine[1];
        public string EventTypeIdentifier => splittedLine[4];
    }
}