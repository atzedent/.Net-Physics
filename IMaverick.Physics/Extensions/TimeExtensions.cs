using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMaverick.Physics.Extensions
{
    public static partial class TimeExtensions
    {
        public static TimeSpan ToTimespan(this PhysicalValue<Time> operand)
        {
            return TimeSpan.FromMilliseconds(operand.ScaleTo(Time.Millisecond).Value);
        }

        public static PhysicalValue<Time> AsTime(this TimeSpan timeSpan)
        {
            return timeSpan.TotalMilliseconds.ms();
        }
    }
}
