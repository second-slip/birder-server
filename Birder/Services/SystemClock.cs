using System;

namespace Birder.Services
{
    public class SystemClock : ISystemClock
    {
        public DateTime GetNow { get { return DateTime.Now; } }
        public DateTime GetToday { get { return DateTime.Today; } }
        public DateTime GetEndOfToday { get { return DateTime.Today.Date.AddDays(1).AddTicks(-1); } }
    }
}
