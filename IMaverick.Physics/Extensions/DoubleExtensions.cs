using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMaverick.Physics.Extensions
{
    public static partial class DoubleExtensions
    {
        public static PhysicalValue<TDimension> Unit<TDimension>(this Double lht, TDimension unit) where TDimension : Dimension<TDimension>, new()
        {
            return new PhysicalValue<TDimension>(lht, unit);
        }
    }
}
