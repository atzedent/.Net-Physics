using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMaverick.Physics.Extensions
{
    public static partial class DoubleExtensions
    {
        public static PhysicalValue<Temperature> Celsius(this Double operand)
        {
            return new PhysicalValue<Temperature>(operand, Temperature.Celsius);
        }

        public static PhysicalValue<Temperature> Kelvin(this Double operand)
        {
            return new PhysicalValue<Temperature>(operand, Temperature.Kelvin);
        }

        public static PhysicalValue<Temperature> Fahrenheit(this Double operand)
        {
            return new PhysicalValue<Temperature>(operand, Temperature.Fahrenheit);
        }
    }
}
