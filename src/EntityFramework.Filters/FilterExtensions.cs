namespace EntityFramework.Filters
{
    using System;
    using System.Collections.Concurrent;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration;
    using System.Reflection;

    public static class FilterExtensions
    {
        private static ConcurrentDictionary<object, Filter> _filterConfigurations;

        internal static ConcurrentDictionary<object, Filter> FilterConfigurations
        {
            get
            {
                if (_filterConfigurations == null)
                    _filterConfigurations = new ConcurrentDictionary<object, Filter>();

                return _filterConfigurations;
            }
        }

        public static EntityTypeConfiguration<TEntity> Filter<TEntity>(this EntityTypeConfiguration<TEntity> config, string name, Action<IFilterConfiguration<TEntity>> filterConfiguration) where TEntity : class
        {
            var filterConfig = new FilterConfiguration<TEntity>(name, config);

            filterConfiguration(filterConfig);

            return config;
        }

        public static IFilter EnableFilter(this DbContext context, string filterName)
        {
            var internalContext = context.GetInternalContext();
            var config = FilterConfigurations.GetOrAdd(internalContext, fn => new Filter(filterName));
            config.IsEnabled = true;

            var eventInfo = internalContext.GetType().GetEvent("OnDisposing", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            eventInfo.AddEventHandler(internalContext, new EventHandler<EventArgs>((o, e) => FilterConfigurations.TryRemove(o, out config)));

            return config;
        }

        public static void DisableFilter(this DbContext context, string filterName)
        {
            var internalContext = context.GetInternalContext();
            var config = FilterConfigurations.GetOrAdd(internalContext, fn => new Filter(filterName));
            config.IsEnabled = false;

            var eventInfo = internalContext.GetType().GetEvent("OnDisposing", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            eventInfo.AddEventHandler(internalContext, new EventHandler<EventArgs>((o, e) => FilterConfigurations.TryRemove(o, out config)));
        }

        internal static object GetInternalContext(this DbContext context)
        {
            return typeof(DbContext)
                .GetProperty("InternalContext", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetGetMethod(true)
                .Invoke(context, null);
        }
    }
}