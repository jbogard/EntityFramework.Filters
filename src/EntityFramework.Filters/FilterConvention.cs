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

        public static FilterConvention Create<TEntity, T0, T1, T2>(string name, Expression<Func<TEntity, T0, T1, T2, bool>> predicate)
        {
            return new FilterConvention(name, typeof(TEntity), predicate);
        }

        public static FilterConvention Create<TEntity, T0, T1, T2, T3>(string name, Expression<Func<TEntity, T0, T1, T2, T3, bool>> predicate)
        {
            return new FilterConvention(name, typeof(TEntity), predicate);
        }

        public static FilterConvention Create<TEntity, T0, T1, T2, T3, T4>(string name, Expression<Func<TEntity, T0, T1, T2, T3, T4, bool>> predicate)
        {
            return new FilterConvention(name, typeof(TEntity), predicate);
        }

        public static FilterConvention Create<TEntity, T0, T1, T2, T3, T4, T5>(string name, Expression<Func<TEntity, T0, T1, T2, T3, T4, T5, bool>> predicate)
        {
            return new FilterConvention(name, typeof(TEntity), predicate);
        }

        public static FilterConvention Create<TEntity, T0, T1, T2, T3, T4, T5, T6>(string name, Expression<Func<TEntity, T0, T1, T2, T3, T4, T5, T6, bool>> predicate)
        {
            return new FilterConvention(name, typeof(TEntity), predicate);
        }

        public static FilterConvention Create<TEntity, T0, T1, T2, T3, T4, T5, T6, T7>(string name, Expression<Func<TEntity, T0, T1, T2, T3, T4, T5, T6, T7, bool>> predicate)
        {
            return new FilterConvention(name, typeof(TEntity), predicate);
        }

        public static FilterConvention Create<TEntity, T0, T1, T2, T3, T4, T5, T6, T7, T8>(string name, Expression<Func<TEntity, T0, T1, T2, T3, T4, T5, T6, T7, T8, bool>> predicate)
        {
            return new FilterConvention(name, typeof(TEntity), predicate);
        }

        public static FilterConvention Create<TEntity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(string name, Expression<Func<TEntity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> predicate)
        {
            return new FilterConvention(name, typeof(TEntity), predicate);
        }

        public static FilterConvention Create<TEntity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string name, Expression<Func<TEntity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>> predicate)
        {
            return new FilterConvention(name, typeof(TEntity), predicate);
        }

        public static FilterConvention Create<TEntity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string name, Expression<Func<TEntity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool>> predicate)
        {
            return new FilterConvention(name, typeof(TEntity), predicate);
        }

        public static FilterConvention Create<TEntity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string name, Expression<Func<TEntity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool>> predicate)
        {
            return new FilterConvention(name, typeof(TEntity), predicate);
        }

        public static FilterConvention Create<TEntity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string name, Expression<Func<TEntity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool>> predicate)
        {
            return new FilterConvention(name, typeof(TEntity), predicate);
        }

        public static FilterConvention Create<TEntity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string name, Expression<Func<TEntity, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool>> predicate)
        {
            return new FilterConvention(name, typeof(TEntity), predicate);
        }
    }
}