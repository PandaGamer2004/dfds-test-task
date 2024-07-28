using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DfdsTestTask.PersistenceShared.Entities;

[PrimaryKey(nameof(Id))]
public class BookingEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DateTime Date { get; set; }
    
    [ConcurrencyCheck]
    public int AggregateVersion { get; set; }

    public List<UserEntity> Users { get; set; } = [];
}