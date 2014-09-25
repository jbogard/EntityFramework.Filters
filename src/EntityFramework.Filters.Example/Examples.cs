namespace EntityFramework.Filters.Example
{
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Xunit;

    public class Examples
    {
        public Examples()
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<ExampleContext>());
            //SeedDb();
            //Database.SetInitializer(new CreateDatabaseIfNotExists<ExampleContext>());
        }

        [Fact]
        public void Should_filter_based_on_global_value()
        {
            using (var context = new ExampleContext())
            {
                var tenant = context.Tenants.Find(1);
                context.CurrentTenant = tenant;
                context.EnableFilter("Tenant")
                    .SetParameter("tenantId", tenant.TenantId);

                Assert.Equal(1, context.BlogEntries.Count());
            }
        }

        [Fact]
        public void Should_not_cache_filter_values()
        {
            using (var context = new ExampleContext())
            {
                var tenant = context.Tenants.Find(1);
                context.CurrentTenant = tenant;
                context.EnableFilter("Tenant")
                    .SetParameter("tenantId", tenant.TenantId);

                Assert.Equal(1, context.BlogEntries.Count());
            }

            using (var context = new ExampleContext())
            {
                var tenant = context.Tenants.Find(2);
                context.CurrentTenant = tenant;
                context.EnableFilter("Tenant")
                    .SetParameter("tenantId", tenant.TenantId);

                Assert.Equal(2, context.BlogEntries.ToList().First().TenantId);
            }
        }

        [Fact(Skip = "Expression compilation not working quite yet")]
        public void Should_filter_based_on_specific_value()
        {
            using (var context = new ExampleContext())
            {
                context.EnableFilter("BadCategory");

                var blogEntries = context.BlogEntries
                    .ToList();

                Assert.Equal(1, blogEntries.Count);
            }
        }

        private static void SeedDb()
        {
            var configuration = new MigrationsConfiguration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }
    }
}