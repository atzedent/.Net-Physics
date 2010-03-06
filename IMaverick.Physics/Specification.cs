using System;
using System.Linq.Expressions;

namespace IMaverick.Physics
{
    /// <summary>
    /// Specifications are a Design Pattern from Eric Evans "Domain Driven Design".
    /// They allow 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Specification < T >
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Specification&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        public Specification ( Expression < Func < T, bool > > predicate )
        {
            if ( predicate != null )
            {
                Predicate = PredicateBuilder.True < T > ( ).And ( predicate );
            }
        }

        #endregion

        #region Predicate

        /// <summary>
        /// Gets or sets the predicate.
        /// </summary>
        /// <value>The predicate.</value>
        public Expression < Func < T, bool > > Predicate { get; set; }

        #endregion

        #region IsSatisfiedBy

        /// <summary>
        /// Determines whether [is satisfied by] [the specified parameter].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// 	<c>true</c> if [is satisfied by] [the specified parameter]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSatisfiedBy ( T parameter )
        {
            return Predicate.Compile ( ).Invoke ( parameter );
        }

        #endregion

        #region And

        /// <summary>
        /// Ands the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public Specification < T > And ( Specification < T > other )
        {
            return new Specification < T > ( Predicate.And ( other.Predicate ) );
        }

        #endregion

        #region Or

        /// <summary>
        /// Ors the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public Specification < T > Or ( Specification < T > other )
        {
            return new Specification < T > ( Predicate.Or ( other.Predicate ) );
        }

        #endregion

        #region Xor

        /// <summary>
        /// Xs the or.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public Specification < T > Xor ( Specification < T > other )
        {
            return new Specification < T > ( Predicate.ExclusiveOr ( other.Predicate ) );
        }

        #endregion

        #region ! operator

        /// <summary>
        /// Operator !s the specified specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns></returns>
        // Named overloads is in this case not correct.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static Specification < T > operator ! ( Specification < T > specification )
        {
            return new Specification < T > ( null )
                       {
                           Predicate = specification.Predicate.Not ( )
                       };
        }

        #endregion
    }
}