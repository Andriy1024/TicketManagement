namespace TMS.Ticketing.IntegrationTest.Common;

[CollectionDefinition(Name)]
public class MongoDBCollection : ICollectionFixture<MongoDBFactory>
{
    public const string Name = "MongoDBCollection";

    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

[CollectionDefinition(Name)]
public class MongoDBReplicaSetCollection : ICollectionFixture<MongoReplicaSetFactory>
{
    public const string Name = "MongoDBReplicaSetCollection";

    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}