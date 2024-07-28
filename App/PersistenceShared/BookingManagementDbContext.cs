using DfdsTestTask.PersistenceShared.Entities;
using Microsoft.EntityFrameworkCore;

namespace DfdsTestTask.PersistenceShared;

public class BookingManagementDbContext: DbContext
{

    public DbSet<UserEntity> Users { get; set; }

    public DbSet<BookingEntity> Bookings { get; set; }

    
    
    public BookingManagementDbContext(
        DbContextOptions<BookingManagementDbContext> options
        ):base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookingEntity>()
            .Property(it => it.AggregateVersion)
            .HasDefaultValue(1);
    }
}