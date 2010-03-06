using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMaverick.Physics
{
    public class Temperature : Dimension<Temperature>
    {
        static Temperature()
        {
            Dimension<Temperature>.InitializeOrdersOfMagnitude("K");
            Units.Add(new Unit { Factor = 1, Offset = 273.15, Name = "°C" });
            Units.Add(new Unit { Factor = Decimal.Divide (5, 9), Offset = 459.67, Name = "°F" });
        }

        public Temperature() : base() { }
        private Temperature(Unit unit) : base(unit) { }

        public static Temperature Kelvin { get { return new Temperature(); } }
        public static Temperature Celsius { get { return new Temperature(Units["°C"]); } }
        public static Temperature Fahrenheit { get { return new Temperature(Units["°F"]); } }
    }
}
