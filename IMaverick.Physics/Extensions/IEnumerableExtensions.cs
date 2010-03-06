using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace IMaverick.Physics.Extensions
{
    public static partial class IEnumerableExtensions
    {
        public static IEnumerable<PhysicalValue<TDimension>> ScaleTo<TDimension>(this IEnumerable<PhysicalValue<TDimension>> list, TDimension unit) where TDimension : Dimension<TDimension>, new()
        {
            return from c in list select c.ScaleTo(unit);
        }

        public static Boolean IsSatisfied<T>(this IEnumerable<T> list, Specification<IEnumerable<T>> specification)
        {
            return specification.IsSatisfiedBy(list);
        }

        public static Specification<IEnumerable<T>> CreateSpecification<T>(this IEnumerable<T> list, Expression<Func<IEnumerable<T>, bool>> predicate)
        {
            return new Specification<IEnumerable<T>>(predicate);
        }
    }

    public static class Sequence
    {
        public static IEnumerable<PhysicalValue<TDimension>> AddRange<TDimension>(PhysicalValue<TDimension> start, PhysicalValue<TDimension> end) where TDimension : Dimension<TDimension>, new()
        {
            return AddRange(start, end, new PhysicalValue<TDimension>(1, start.Unit));
        }

        public static IEnumerable<PhysicalValue<TDimension>> AddRange<TDimension>(PhysicalValue<TDimension> start, PhysicalValue<TDimension> end, PhysicalValue<TDimension> step) where TDimension : Dimension<TDimension>, new()
        {
            step.Value = Math.Abs(step.Value);
            start = start.ScaleTo(step.Unit);
            end = end.ScaleTo(start.Unit);

            var offset = (start < end) ? step : -step;

            for (var i = start; i != end + offset; i += offset)
            {
                yield return i;
            }
        }

        public static IEnumerable<PhysicalValue<TDimension>> AddRange<TDimension>(Int32 start, Int32 end, TDimension unit, Int32 step = 1) where TDimension : Dimension<TDimension>, new()
        {
            step = Math.Abs(step);

            return start < end ? Range(start, end, unit, step) : Range(end, start, unit, step).Reverse();
        }

        private static IEnumerable<PhysicalValue<TDimension>> Range<TDimension>(int start, int end, TDimension unit, int step) where TDimension : Dimension<TDimension>, new()
        {
            return ParallelEnumerable.Range(start, end).AsOrdered().Select(p => ((double)p * step).Unit(unit)).AsSequential();
        }
    }
}
