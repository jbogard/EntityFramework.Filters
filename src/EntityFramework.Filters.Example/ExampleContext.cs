namespace EntityFramework.Filters.Example
{
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.SqlServer;
    using System.Linq;

    public class MigrationsConfiguration : DbMigrationsConfiguration<ExampleContext>
    {
        public MigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ExampleContext context)
        {
            var tenant1 = new Tenant
            {
                Name = "Tenant 1"
            };


            context.Tenants.Add(tenant1);
            context.SaveChanges();

            context.CurrentTenant = tenant1;

            var category = new Category
            {
                Name = "Good posts"
            };
            context.Categories.Add(category);
            context.SaveChanges();

            var author = new Author
            {
                FirstName = "John",
                LastName = "Doe"
            };
            context.Authors.Add(author);
            context.SaveChanges();

            var blog = new BlogEntry
            {
                Title = "My entry",
                Body = "Blog entry here",
                Author = author
            };
            blog.Comments.Add(new Comment
            {
                Text = "First comment"
            });
            blog.Comments.Add(new Comment
            {
                Text = "Second comment"
            });
            blog.AddCategory(category);
            context.BlogEntries.Add(blog);
            context.SaveChanges();

            var tenant2 = new Tenant
            {
                Name = "Tenant 2"
            };


            context.Tenants.Add(tenant2);
            context.SaveChanges();

            context.CurrentTenant = tenant2;

            var category2 = new Category
            {
                Name = "Bad posts"
            };
            context.Categories.Add(category2);
            context.SaveChanges();

            var author2 = new Author
            {
                FirstName = "Jane",
                LastName = "Doe"
            };
            context.Authors.Add(author2);
            context.SaveChanges();
            
            var blog2 = new BlogEntry
            {
                Title = "My other entry",
                Body = "Blog entry here",
                Author = author
            };
            blog2.Comments.Add(new Comment
            {
                Text = "First comment"
            });
            blog2.Comments.Add(new Comment
            {
                Text = "Second comment"
            });
            blog2.AddCategory(category2);
            context.BlogEntries.Add(blog2);
            context.SaveChanges();
        }
    }

    public class ExampleConfiguration : DbConfiguration
    {
        public ExampleConfiguration()
        {
            AddInterceptor(new FilterInterceptor());
        }
    }

    public class ExampleContext : DbContext
    {
        public ExampleContext() :base("name=EntityFramework.Filters.Example.ExampleContext")
        {
        }

        public DbSet<BlogEntry> BlogEntries { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Author> Authors { get; set; }

        public Tenant CurrentTenant { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogEntry>()
                .Filter("BadCategory", fc => fc.Condition(be => be.Categories.Select(c => c.Category.Name).Contains("Bad posts")))
                .HasMany(m => m.Categories).WithRequired(m => m.BlogEntry).WillCascadeOnDelete(true);
            modelBuilder.Entity<BlogEntry>()
                .HasRequired(m => m.Tenant).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Comment>()
                .HasRequired(m => m.Tenant).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Category>()
                .HasRequired(m => m.Tenant).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<BlogEntryCategory>()
                .HasRequired(m => m.Tenant).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Author>()
                .HasRequired(m => m.Tenant).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Conventions.Add(FilterConvention.Create<ITenantEntity, int>("Tenant", (e, tenantId) => e.TenantId == tenantId));
        }

        public override int SaveChanges()
        {
            var tenantEntities = ChangeTracker.Entries<ITenantEntity>().ToArray();
            foreach (var item in tenantEntities.Where(t => t.State == EntityState.Added))
            {
                item.Entity.Tenant = CurrentTenant;
            }
            return base.SaveChanges();
        }
    }
}