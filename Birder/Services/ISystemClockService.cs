using System;

namespace Birder.Services
{
    public interface ISystemClockService
    {
        DateTime GetNow { get; }
        DateTime GetToday { get; }
        DateTime GetEndOfToday { get; }
    }
}
