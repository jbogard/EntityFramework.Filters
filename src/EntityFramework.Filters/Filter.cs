namespace EntityFramework.Filters
{
    using System.Collections.Concurrent;

    public class Filter : IFilter
    {
        public string FilterName { get; private set; }
        public bool IsEnabled { get; set; }
        public ConcurrentDictionary<string, object> ParameterValues { get; private set; }

        public Filter(string filterName)
        {
            FilterName = filterName;
            ParameterValues = new ConcurrentDictionary<string, object>();
        }

        public IFilter SetParameter(string parameter, object value)
        {
            ParameterValues.AddOrUpdate(parameter, value, (s, o) => value);
            return this;
        }
    }
}