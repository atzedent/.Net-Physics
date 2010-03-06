using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMaverick.Physics.Extensions
{
    public static partial class DoubleExtensions
    {
        public static PhysicalValue<Length> km(this Double operand)
        {
            return new PhysicalValue<Length>(operand, Length.Kilometer);
        }
        public static PhysicalValue<Length> m(this Double operand)
        {
            return new PhysicalValue<Length>(operand, Length.Meter);
        }
        public static PhysicalValue<Length> cm(this Double operand)
        {
            return new PhysicalValue<Length>(operand, Length.Centimeter);
        }
    }
}
