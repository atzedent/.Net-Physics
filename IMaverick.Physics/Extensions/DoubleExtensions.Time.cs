using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMaverick.Physics.Extensions
{
    public static partial class DoubleExtensions
    {
        public static PhysicalValue<Time> s(this Double operand)
        {
            return new PhysicalValue<Time>(operand, Time.Second);
        }
        public static PhysicalValue<Time> min(this Double operand)
        {
            return new PhysicalValue<Time>(operand, Time.Minute);
        }
        public static PhysicalValue<Time> h(this Double operand)
        {
            return new PhysicalValue<Time>(operand, Time.Hour);
        }
        public static PhysicalValue<Time> d(this Double operand)
        {
            return new PhysicalValue<Time>(operand, Time.Day);
        }
        public static PhysicalValue<Time> ms(this Double operand)
        {
            return new PhysicalValue<Time>(operand, Time.Millisecond);
        }
        public static PhysicalValue<Time> µs(this Double operand)
        {
            return new PhysicalValue<Time>(operand, Time.Microsecond);
        }
    }
}
