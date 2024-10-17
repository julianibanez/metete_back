using System.Linq.Expressions;

namespace Metete.Api.Infraestructure.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var combined = new Replacer(parameter).Visit(
                Expression.OrElse(expr1.Body, expr2.Body)
            );

            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }

        private class Replacer : ExpressionVisitor
        {
            private readonly ParameterExpression _parameter;

            public Replacer(ParameterExpression parameter)
            {
                _parameter = parameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return base.VisitParameter(_parameter);
            }
        }
    }
}
