using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Utilities;

namespace Utilities
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, new Func<Expression, Expression, Expression>(Expression.And));
        }

        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            Expression expression = ParameterRebinder.ReplaceParameters(
                first.Parameters.Zip(
                second.Parameters,
                (f, s) => new {
                    First = f,
                    Second = s
                }).ToDictionary(p => p.Second, p => p.First),
                second.Body);
            return Expression.Lambda<T>(merge(first.Body, expression), first.Parameters);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, new Func<Expression, Expression, Expression>(Expression.Or));
        }
    }
}
