using System.Data.Entity.ModelConfiguration;

namespace EntityFramework.Filters.Example
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public interface ILeietagerEntity
    {
        int LeietagerId { get; set; }
        Leietager Leietager { get; set; }
    }

    public class Leietager
    {
        public int LeietagerId { get; set; }
        public string Navn { get; set; }
    }

    public class LeietagerMap : EntityTypeConfiguration<Leietager>
    {
        public LeietagerMap()
        {
            ToTable("Tenants");
            Property(p => p.LeietagerId).HasColumnName("TenantId");
            Property(p => p.Navn).HasColumnName("Name");
        }
    }

    public class Kategori : ILeietagerEntity
    {
        public Kategori()
        {
            BlogPoster = new Collection<BlogPostKategori>();
        }

        public int KategoriId { get; set; }
        public int LeietagerId { get; set; }
        public Leietager Leietager { get; set; }
        public string Navn { get; set; }
        public virtual ICollection<BlogPostKategori> BlogPoster { get; set; }
    }

    public class KategoriMap : EntityTypeConfiguration<Kategori>
    {
        public KategoriMap()
        {
            ToTable("Categories");
            Property(p => p.KategoriId).HasColumnName("CategoryId");
            Property(p => p.LeietagerId).HasColumnName("TenantId");
            Property(p => p.Navn).HasColumnName("Name");
        }
    }

    public class BlogPost : ILeietagerEntity
    {
        public BlogPost()
        {
            Kommentars = new Collection<Kommentar>();
            Kategorier = new Collection<BlogPostKategori>();
        }
        public int BlogPostId { get; set; }
        public string Tittel { get; set; }
        public string Kropp { get; set; }
        public virtual ICollection<Kommentar> Kommentars { get; set; }
        public virtual ICollection<BlogPostKategori> Kategorier { get; set; }

        public Forfatter Forfatter { get; set; }
        public int ForfatterId { get; set; }

        public int LeietagerId { get; set; }
        public Leietager Leietager { get; set; }

        public virtual void AddKategori(Kategori kategori)
        {
            Kategorier.Add(new BlogPostKategori
            {
                BlogPost = this,
                Kategori = kategori
            });
        }
    }

    public class BlogPostMap : EntityTypeConfiguration<BlogPost>
    {
        public BlogPostMap()
        {
            ToTable("BlogEntries");
            Property(p => p.BlogPostId).HasColumnName("BlogEntryId");
            Property(p => p.ForfatterId).HasColumnName("AuthorId");
            Property(p => p.Kropp).HasColumnName("Body");
            Property(p => p.LeietagerId).HasColumnName("TenantId");
            Property(p => p.Tittel).HasColumnName("Title");
            Property(p => p.BlogPostId).HasColumnName("BlogEntryId");
            Property(p => p.BlogPostId).HasColumnName("BlogEntryId");
        }
    }


    public class BlogPostKategori : ILeietagerEntity
    {
        public int BlogPostKategoriId { get; set; }
        public BlogPost BlogPost { get; set; }
        public int BlogPostId { get; set; }
        public Kategori Kategori { get; set; }
        public int KategoriId { get; set; }

        public int LeietagerId { get; set; }
        public Leietager Leietager { get; set; }
    }

    public class BlogPostKategoriMap : EntityTypeConfiguration<BlogPostKategori>
    {
        public BlogPostKategoriMap()
        {
            ToTable("BlogPostCategories");
            Property(p => p.BlogPostId).HasColumnName("BlogEntryId");
            Property(p => p.BlogPostKategoriId).HasColumnName("BlogEntryCategoryId");
            Property(p => p.KategoriId).HasColumnName("CategoryId");
            Property(p => p.LeietagerId).HasColumnName("TenantId");
        }
    }

    public class Kommentar : ILeietagerEntity
    {
        public int KommentarId { get; set; }
        public int LeietagerId { get; set; }
        public BlogPost BlogPost { get; set; }
        public int BlogPostId { get; set; }
        public Leietager Leietager { get; set; }
        public string Tekst { get; set; }
    }

    public class KommentarMap : EntityTypeConfiguration<Kommentar>
    {
        public KommentarMap()
        {
            ToTable("Comments");
            Property(p => p.KommentarId).HasColumnName("CommentId");
            Property(p => p.LeietagerId).HasColumnName("TenantId");
            Property(p => p.BlogPostId).HasColumnName("BlogEntryId");
            Property(p => p.Tekst).HasColumnName("Text");
        }
    }

    public class Forfatter : ILeietagerEntity
    {
        public int ForfatterId { get; set; }
        public string ForNavn { get; set; }
        public string EtterNavn { get; set; }
        public int LeietagerId { get; set; }
        public Leietager Leietager { get; set; }
    }

    public class ForfatterMap : EntityTypeConfiguration<Forfatter>
    {
        public ForfatterMap()
        {
            ToTable("Authors");
            Property(p => p.EtterNavn).HasColumnName("LastName");
            Property(p => p.ForNavn).HasColumnName("FirstName");
            Property(p => p.ForfatterId).HasColumnName("AuthorId");
            Property(p => p.LeietagerId).HasColumnName("TenantId");
        }
    }
}