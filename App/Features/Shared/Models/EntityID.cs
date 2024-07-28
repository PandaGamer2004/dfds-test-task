using DfdsTestTask.Exceptions;

namespace DfdsTestTask.Features.Shared.Models;

public class EntityID
{
    public int Value { get; init; }

    public Type RefferenceTo { get; private set; }
    
    protected static EntityID FromValue(int userId, Type type)
    {
        if (userId == 0)
        {
            throw new EntityInitializationException(
                "Unable to initialize reference to the user with id equal to 0",
                typeof(EntityID)
            );
        }

        return new EntityID()
        {
            RefferenceTo = type,
            Value = userId
        };
    }
}