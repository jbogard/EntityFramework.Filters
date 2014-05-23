namespace EntityFramework.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq.Expressions;

    public class FilterDefinition
    {
        public string Name { get; private set; }
        public Func<DbContext, IDictionary<string, object>, Expression> Predicate { get; private set; }

        public FilterDefinition(string name, Func<DbContext, IDictionary<string, object>, Expression> predicate)
        {
            Name = name;
            Predicate = predicate;
        }
    }
}