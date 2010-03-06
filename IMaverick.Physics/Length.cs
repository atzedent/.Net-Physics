using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMaverick.Physics.Extensions;

namespace IMaverick.Physics
{
    public class Length : Dimension<Length>
    {
        static Length()
        {
            Dimension<Length>.InitializeOrdersOfMagnitude("m");
        }

        public Length() : base() { }

        public static Length Meter { get { return new Length(); } }
        public static Length Kilometer { get { return GetNewInstanceFor(OrderOfMagnitude.Kilo); } }
        public static Length Centimeter { get { return GetNewInstanceFor(OrderOfMagnitude.Centi); } }
    }
}
