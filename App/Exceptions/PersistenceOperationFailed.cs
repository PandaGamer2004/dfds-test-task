namespace DfdsTestTask.Exceptions;

public class PersistenceOperationFailedException(string failureReason) : Exception(failureReason);