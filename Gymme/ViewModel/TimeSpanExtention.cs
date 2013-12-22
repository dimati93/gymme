using System;

namespace Gymme.ViewModel
{
    public static class TimeSpanExtention
    {
        public static TimeSpan TrimToSeconds(this TimeSpan span)
        {
            // ReSharper disable once PossibleLossOfFraction
            return TimeSpan.FromSeconds(span.Ticks/TimeSpan.TicksPerSecond);
        }
    }
}