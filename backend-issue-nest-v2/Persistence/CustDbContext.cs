using backend_issue_nest_v2.Models;
using backend_issue_nest_v2.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace backend_issue_nest_v2.Persistence
{
    public class CustDbContext(DbContextOptions<CustDbContext> options) : DbContext(options)
    {
        public  DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TicketConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DateTime saveDate = DateTime.Now.ToLocalTime();
            var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                // updated entry
                if (entry.Metadata.FindProperty("ModifiedAt") != null)
                    entry.Property("ModifiedAt").CurrentValue = saveDate;

                if (entry.State == EntityState.Added)
                {
                    // created entry
                    if (entry.Metadata.FindProperty("CreatedAt") != null)
                        entry.Property("CreatedAt").CurrentValue = saveDate;
                }
            }

            return (await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false));
        }
    }
}
