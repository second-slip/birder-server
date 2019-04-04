using System;

namespace Birder.Services
{
    public class SystemClock : ISystemClock
    {
        public DateTime Now { get { return DateTime.Now; } }
        public DateTime Today { get { return DateTime.Today; } }
    }
}
