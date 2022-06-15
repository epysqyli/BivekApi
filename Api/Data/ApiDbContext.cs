using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Api.Data
{
    public class ApiDbContext : IdentityDbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<WorkingPaper> WorkingPapers { get; set; }
        public DbSet<Dataset> Datasets { get; set; }
        public DbSet<DataCategory> DataCategories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        { }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ArticleTag>()
                .HasKey(at => new { at.ArticleId, at.TagId });

            builder.Entity<ArticleTag>()
                .HasOne(at => at.Tag)
                .WithMany(at => at.ArticleTags)
                .HasForeignKey(at => at.TagId);

            builder.Entity<ArticleTag>()
                .HasOne(at => at.Article)
                .WithMany(at => at.ArticleTags)
                .HasForeignKey(at => at.ArticleId);
        }
    }
}