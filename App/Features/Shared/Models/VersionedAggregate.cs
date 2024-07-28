namespace DfdsTestTask.Features.Shared.Models;

public abstract class VersionedAggregate
{
    public int AggregateVersion { get; set; }
}