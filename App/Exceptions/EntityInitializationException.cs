namespace DfdsTestTask.Exceptions;

public class EntityInitializationException: Exception
{
    public Type OnTypeToInitialize { get; set; }

    public EntityInitializationException(
        string initializationErrorMessage, 
        Type typeToInitialize
        ): base(initializationErrorMessage)
    {
        this.OnTypeToInitialize = typeToInitialize;
    }
}