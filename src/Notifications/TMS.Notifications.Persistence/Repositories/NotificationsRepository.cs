using MongoDB.Driver;

using TMS.MongoDB.Repositories;
using TMS.MongoDB.Transactions;

using TMS.Notifications.Application.Interfaces;
using TMS.Notifications.Domain.Models;

namespace TMS.Notifications.Persistence.Repositories;

internal class NotificationsRepository : MongoRepository<NotificationEntity, Guid>, INotificationsRepository
{
    protected override string CollectionName => "Notifications";

    public NotificationsRepository(IMongoDatabase database, MongoTransactionScope transactionScope)
         : base(database, transactionScope) { }
}