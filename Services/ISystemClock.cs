using System;

namespace Birder.Services
{
    public interface ISystemClock
    {
        DateTime Now { get; }
        DateTime Today { get; }
    }
}
