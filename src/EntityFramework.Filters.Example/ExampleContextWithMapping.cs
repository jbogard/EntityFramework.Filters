using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.Filters.Example
{
    public class ExampleContextWithMapping : DbContext
    {
        public ExampleContextWithMapping() : base("name=EntityFramework.Filters.Example.ExampleContext")
        {
            Database.SetInitializer<ExampleContextWithMapping>(null);
        }

        public DbSet<BlogPost> BlogPoster { get; set; }
        public DbSet<Kommentar> Kommentarer { get; set; }
        public DbSet<Kategori> Kategorier { get; set; }
        public DbSet<Leietager> Leietagere { get; set; }
        public DbSet<Forfatter> Forfattere { get; set; }

        public Leietager CurrentLeietager { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BlogPostMap());
            modelBuilder.Configurations.Add(new KategoriMap());
            modelBuilder.Configurations.Add(new KommentarMap());
            modelBuilder.Configurations.Add(new ForfatterMap());
            modelBuilder.Configurations.Add(new LeietagerMap());

            modelBuilder.Entity<BlogPost>()
                .Filter("BadCategory",
                    fc => fc.Condition(be => be.Kategorier.Select(c => c.Kategori.Navn).Contains("Bad posts")))
                .HasMany(m => m.Kategorier).WithRequired(m => m.BlogPost).WillCascadeOnDelete(true);
            modelBuilder.Entity<BlogPost>()
                .HasRequired(m => m.Leietager).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Kommentar>()
                .HasRequired(m => m.Leietager).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Kategori>()
                .HasRequired(m => m.Leietager).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<BlogPostKategori>()
                .HasRequired(m => m.Leietager).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Forfatter>()
                .HasRequired(m => m.Leietager).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Conventions.Add(FilterConvention.Create<ILeietagerEntity, int>("Tenant",
                (e, leietagerId) => e.LeietagerId == leietagerId));
        }
    }
}
