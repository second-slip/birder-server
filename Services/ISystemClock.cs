using System;

namespace Birder.Services
{
    public interface ISystemClock
    {
        DateTime GetNow { get; }
        DateTime GetToday { get; }
        DateTime GetEndOfToday { get; }
    }
}
