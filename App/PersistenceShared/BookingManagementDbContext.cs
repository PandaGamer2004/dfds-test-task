using DfdsTestTask.PersistenceShared.Entities;
using Microsoft.EntityFrameworkCore;

namespace DfdsTestTask.PersistenceShared;

public class BookingManagementDbContext(DbContextOptions<BookingManagementDbContext> options) : DbContext(options)
{

    public DbSet<UserEntity> Users { get; set; }

    public DbSet<BookingEntity> Bookings { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookingEntity>()
            .Property(it => it.AggregateVersion)
            .HasDefaultValue(1);
    }
}