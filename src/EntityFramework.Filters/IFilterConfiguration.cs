namespace EntityFramework.Filters
{
    using System;
    using System.Linq.Expressions;

    public interface IFilterConfiguration<TEntity>
    {
        void Condition(Expression<Func<TEntity, bool>> predicate);
    }
}