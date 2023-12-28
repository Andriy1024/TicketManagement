namespace TMS.MongoDB.Exceptions;

public sealed class MongoDbException : Exception
{
    public MongoDbException(string message)
        : base(message)
    {
    }

    public static MongoDbException OperationIsAcknowledged() =>
        new("This operation conflicted with another operation. MongoDB operation is not acknowledged. Please retry your operation.");
}
