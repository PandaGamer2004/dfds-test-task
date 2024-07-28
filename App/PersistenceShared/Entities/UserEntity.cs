using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DfdsTestTask.PersistenceShared.Entities;

[PrimaryKey(nameof(Id))]
public class UserEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string PassportNumber { get; set; }

    [Timestamp]
    public byte[] Version { get; set; }
    
    public List<BookingEntity> Bookings { get; set; } = [];
}