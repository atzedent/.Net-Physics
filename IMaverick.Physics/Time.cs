using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMaverick.Physics.Extensions;

namespace IMaverick.Physics
{
    public class Time : Dimension<Time>
    {
        static Time()
        {
            Dimension<Time>.InitializeOrdersOfMagnitude("s");
            Units.Add(new Unit { Factor = 60, Name = "min" });
            Units.Add(new Unit { Factor = 3600, Name = "h" });
            Units.Add(new Unit { Factor = 86400, Name = "d" });
        }

        public Time() : base() { }
        private Time(String unit) : base(unit) { }

        public static Time Day { get { return new Time("d"); } }
        public static Time Hour { get { return new Time("h"); } }
        public static Time Minute { get { return new Time("min"); } }
        public static Time Second { get { return new Time(); } }
        public static Time Millisecond { get { return GetNewInstanceFor(OrderOfMagnitude.Milli); } }
        public static Time Microsecond { get { return GetNewInstanceFor(OrderOfMagnitude.Micro); } }
    }
}
