namespace EntityFramework.Filters.Example
{
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Xunit;

    public class Examples
    {
        static Examples()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<ExampleContext>());
            var configuration = new MigrationsConfiguration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }

        [Fact]
        public void Should_filter_based_on_hardcoded_value()
        {
            using (var context = new ExampleContext())
            {
                //var tenant = context.Tenants.Find(1);
                //context.CurrentTenant = tenant;
                //context.EnableFilter("Tenant")
                //    .SetParameter("tenantId", tenant.TenantId);

                //Assert.Equal(1, context.BlogEntries.Count());
            }
        } 
    }
}