using FileForge.Entities;
using FileForge.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FileForge.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        // register all entities
        public DbSet<Category> Categories => Set<Category>();





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Apply soft-delete filter for all AuditableEntity models
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(GetSoftDeleteFilter(entityType.ClrType));
                }
            }
        }

        private static LambdaExpression GetSoftDeleteFilter(Type entityType)
        {
            var param = Expression.Parameter(entityType, "e");
            var prop = Expression.Property(param, nameof(AuditableEntity.DeletedAt));
            var body = Expression.Equal(prop, Expression.Constant(null));
            return Expression.Lambda(body, param);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<AuditableEntity>();

            foreach (var entry in entries)
            {
                var now = DateTime.UtcNow;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.CreatedBy = 1;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = now;
                        entry.Entity.UpdatedBy = 1;
                        break;

                    case EntityState.Deleted:
                        // Soft delete
                        entry.State = EntityState.Modified;
                        entry.Entity.DeletedAt = now;
                        entry.Entity.DeletedBy = 1;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
