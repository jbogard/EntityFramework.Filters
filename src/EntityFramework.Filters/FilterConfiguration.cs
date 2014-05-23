namespace EntityFramework.Filters
{
    using System;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq.Expressions;

    public class FilterConfiguration<TEntity> : IFilterConfiguration<TEntity>
        where TEntity : class
    {
        private readonly string _name;
        private readonly EntityTypeConfiguration<TEntity> _config;

        public FilterConfiguration(string name, EntityTypeConfiguration<TEntity> config)
        {
            _name = name;
            _config = config;
        }

        public void Condition(Expression<Func<TEntity, bool>> predicate)
        {
            var definitionFactory = new FilterDefinitionFactory<TEntity>();
            var filterDefinition = definitionFactory.Create(_name, predicate);
            _config.HasTableAnnotation("globalFilter_" + _name, filterDefinition);
        }
    }
}