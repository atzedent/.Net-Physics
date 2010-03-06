using System;
using System.Linq;
using System.Linq.Expressions;

namespace IMaverick.Physics
{
    /// <summary>
    /// See http://www.albahari.com/expressions for information and examples.
    /// </summary>
    internal static class PredicateBuilder
    {
        /// <summary>
        /// Trues this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static Expression<Func<T, Boolean>> True<T>()
        {
            return f => true;
        }

        /// <summary>
        /// Ors the specified expr1.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1">The expr1.</param>
        /// <param name="expr2">The expr2.</param>
        /// <returns></returns>
        internal static Expression<Func<T, Boolean>> Or<T>(
            this Expression<Func<T, Boolean>> expr1,
            Expression<Func<T, Boolean>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, Boolean>>
                (Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// XOrs the specified expr1.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1">The expr1.</param>
        /// <param name="expr2">The expr2.</param>
        /// <returns></returns>
        internal static Expression<Func<T, Boolean>> ExclusiveOr<T>(
            this Expression<Func<T, Boolean>> expr1,
            Expression<Func<T, Boolean>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, Boolean>>
                (Expression.ExclusiveOr(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// Ands the specified expr1.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1">The expr1.</param>
        /// <param name="expr2">The expr2.</param>
        /// <returns></returns>
        internal static Expression<Func<T, Boolean>> And<T>(
            this Expression<Func<T, Boolean>> expr1,
            Expression<Func<T, Boolean>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, Boolean>>
                (Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }

        // Extended the ExpressionBuilder with a Not method.
        /// <summary>
        /// Negates the specified expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        internal static Expression<Func<T, Boolean>> Not<T>(this Expression<Func<T, Boolean>> expression)
        {
            return Expression.Lambda<Func<T, Boolean>>(Expression.Not(expression.Body), expression.Parameters);
        }
    }
}
