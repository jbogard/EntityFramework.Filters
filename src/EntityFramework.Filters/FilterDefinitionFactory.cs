namespace EntityFramework.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    public class FilterDefinitionFactory<TEntity> : FilterDefinitionFactory where TEntity : class
    {
        public override FilterDefinition Create(string name, LambdaExpression predicate)
        {
            Func<DbContext, IDictionary<string, object>, Expression> dude =
                (ctxt, queryParams) => ctxt.Set<TEntity>().Where(Transform(predicate, queryParams)).Expression;

            return new FilterDefinition(name, dude);
        }
        private static Expression<Func<TEntity, bool>> Transform(LambdaExpression expression, IDictionary<string, object> queryParams)
        {
            var replacer = new ParameterReplacer(queryParams);
            return (Expression<Func<TEntity, bool>>)replacer.Visit(expression);
        }

        private class ParameterReplacer : ExpressionVisitor
        {
            private readonly IDictionary<string, object> _paramValues;

            public ParameterReplacer(IDictionary<string, object> paramValues)
            {
                _paramValues = paramValues;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return _paramValues.ContainsKey(node.Name)
                    ? Expression.Constant(_paramValues[node.Name])
                    : base.VisitParameter(node);
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                var body = Visit(node.Body);
                return Expression.Lambda<Func<TEntity, bool>>(body, node.Parameters[0]);
            }
        }
    }

    public abstract class FilterDefinitionFactory
    {
        public abstract FilterDefinition Create(string name, LambdaExpression predicate);
    }
}