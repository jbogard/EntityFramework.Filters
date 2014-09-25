using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EntityFramework.Filters.Example
{
    using System.Data.Entity;

    public class ExamplesWithMapping
    {
        public ExamplesWithMapping()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<ExampleContextWithMapping>());
            // Initialize database in the Examples class 
        }

        [Fact]
        public void Should_filter_based_on_global_value_with_mapping()
        {
            using (var context = new ExampleContextWithMapping())
            {
                var tenant = context.Leietagere.Find(1);
                context.CurrentLeietager = tenant;
                context.EnableFilter("Tenant")
                    .SetParameter("leietagerId", tenant.LeietagerId);

                Assert.Equal(1, context.BlogPoster.Count());
            }
        }

        [Fact(Skip = "Expression compilation not working quite yet")]
        public void Should_filter_based_on_specific_value_with_mapping()
        {
            using (var context = new ExampleContextWithMapping())
            {
                context.EnableFilter("BadCategory");

                var blogEntries = context.BlogPoster
                    .ToList();

                Assert.Equal(1, blogEntries.Count);
            }
        }
    }
}
