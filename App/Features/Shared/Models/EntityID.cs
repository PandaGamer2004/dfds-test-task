using DfdsTestTask.Exceptions;

namespace DfdsTestTask.Features.Shared.Models;

public class EntityID
{
    public int Value { get; private set; }

    public Type RefferenceTo { get; private set; }
    
    protected static TEntity FromValue<TEntity>(TEntity atEntity, int userId, Type type)
    where TEntity: EntityID
    {
        if (userId == 0)
        {
            throw new EntityInitializationException(
                "Unable to initialize reference to the user with id equal to 0",
                typeof(EntityID)
            );
        }

        atEntity.RefferenceTo = type;
        atEntity.Value = userId;
        return atEntity;
    }
}