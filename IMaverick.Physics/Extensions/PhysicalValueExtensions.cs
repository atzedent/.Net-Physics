using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace IMaverick.Physics.Extensions
{
    public static partial class PhysicalValueExtensions
    {
        public static Double DivideBy<TDimension>(this PhysicalValue<TDimension> lht, PhysicalValue<TDimension> rht) where TDimension : Dimension<TDimension>, new()
        {
            return
                Decimal.ToDouble(Decimal.Divide(new Decimal(lht.Value), new Decimal(rht.ScaleTo(lht.Unit).Value)));
        }

        public static Specification<PhysicalValue<TDimension>> CreateSpecification<TDimension>(this PhysicalValue<TDimension> operand, Expression<Func<PhysicalValue<TDimension>, bool>> predicate) where TDimension : Dimension<TDimension>, new()
        {
            return new Specification<PhysicalValue<TDimension>>(predicate);
        }
    }
}
