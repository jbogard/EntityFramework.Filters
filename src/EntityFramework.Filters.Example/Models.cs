namespace EntityFramework.Filters.Example
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public interface ITenantEntity
    {
        int TenantId { get; set; }
        Tenant Tenant { get; set; }
    }

    public class Tenant
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
    }

    public class Category : ITenantEntity
    {
        public Category()
        {
            Blogs = new Collection<BlogEntryCategory>();
        }

        public int CategoryId { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string Name { get; set; }
        public virtual ICollection<BlogEntryCategory> Blogs { get; set; }
    }
    public class BlogEntry : ITenantEntity
    {
        public BlogEntry()
        {
            Comments = new Collection<Comment>();
            Categories = new Collection<BlogEntryCategory>();
        }
        public int BlogEntryId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<BlogEntryCategory> Categories { get; set; }

        public Author Author { get; set; }
        public int AuthorId { get; set; }

        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        public virtual void AddCategory(Category category)
        {
            Categories.Add(new BlogEntryCategory
            {
                BlogEntry = this,
                Category = category
            });
        }
    }

    public class BlogEntryCategory : ITenantEntity
    {
        public int BlogEntryCategoryId { get; set; }
        public BlogEntry BlogEntry { get; set; }
        public int BlogEntryId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }

    public class Comment : ITenantEntity
    {
        public int CommentId { get; set; }
        public int TenantId { get; set; }
        public BlogEntry BlogEntry { get; set; }
        public int BlogEntryId { get; set; }
        public Tenant Tenant { get; set; }
        public string Text { get; set; }
    }

    public class Author : ITenantEntity
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}