namespace DfdsTestTask.Features.Shared.Models;

public abstract class VersionedAggregate<TVersion>
{
    public TVersion AggregateVersion { get; set; }
}