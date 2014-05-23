namespace EntityFramework.Filters
{
    using System;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq.Expressions;

    public class FilterConvention : Convention
    {
        private FilterConvention(string name, Type entityType, LambdaExpression predicate)
        {
            var configuration = Types().Where(entityType.IsAssignableFrom);
            configuration.Configure(ctc =>
            {
                var factory =
                    (FilterDefinitionFactory)
                        Activator.CreateInstance(typeof(FilterDefinitionFactory<>).MakeGenericType(ctc.ClrType));

                var filterDefinition = factory.Create(name, predicate);

                ctc.HasTableAnnotation("globalFilter_" + name, filterDefinition);
            });
        }

        public static FilterConvention Create<TEntity>(string name, Expression<Func<TEntity, bool>> predicate)
        {
            return new FilterConvention(name, typeof(TEntity), predicate);
        }

        public static FilterConvention Create<TEntity, T0>(string name, Expression<Func<TEntity, T0, bool>> predicate)
        {
            return new FilterConvention(name, typeof(TEntity), predicate);
        }

        public static FilterConvention Create<TEntity, T0, T1>(string name, Expression<Func<TEntity, T0, T1, bool>> predicate)
        {
            return new FilterConvention(name, typeof(TEntity), predicate);
        }

    }
}