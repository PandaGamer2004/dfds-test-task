using DfdsTestTask.PersistenceShared.Entities;
using Microsoft.EntityFrameworkCore;

namespace DfdsTestTask.PersistenceShared;

public class BookingManagementDbContext: DbContext
{

    public DbSet<UserEntity> Users { get; set; }

    public DbSet<BookingEntity> Bookings { get; set; }


    public DbSet<UserToBookingEntity> UserToBooking { get; set; }
    
    
    public BookingManagementDbContext(
        DbContextOptions<BookingManagementDbContext> options
        ):base(options)
    {
        
    }
    
}