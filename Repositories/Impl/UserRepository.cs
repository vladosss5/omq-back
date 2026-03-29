using AskAgainApi.Entity.Session;
using AskAgainApi.Entity.User;
using AskAgainApi.Enums;
using AskAgainApi.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace AskAgainApi.Repositories.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserEntity> _usersCollection;
        private readonly IMongoCollection<SessionEntity> _sessionsCollection;

        public UserRepository(
            IOptions<DatabaseSettings> AskAgainDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                AskAgainDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                AskAgainDatabaseSettings.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<UserEntity>(
                AskAgainDatabaseSettings.Value.UsersCollectionName);
            
            _sessionsCollection = mongoDatabase.GetCollection<SessionEntity>(
                AskAgainDatabaseSettings.Value.SessionsCollectionName);
        }

        public async Task<List<UserEntity>> GetAsync() =>
            await _usersCollection.Find(_ => true).ToListAsync();

        public async Task<UserEntity?> GetAsync(Guid id) =>
            await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(UserEntity newUser) =>
            await _usersCollection.InsertOneAsync(newUser);

        public async Task RemoveAsync(Guid id) =>
            await _usersCollection.DeleteOneAsync(x => x.Id == id);

        public async Task UpdateAsync(UserEntity updatedUser) =>
            await _usersCollection.ReplaceOneAsync(x => x.Id == updatedUser.Id, updatedUser);


        public async Task ChangeTariffAsync(Guid userId, TariffEnum tariff)
        {
            var filter = Builders<UserEntity>.Filter
                .Eq(user => user.Id, userId);
            var update = Builders<UserEntity>.Update
                .Set(user => user.TariffId, tariff);

            await _usersCollection.FindOneAndUpdateAsync(filter, update);
        }

        public async Task IncrementCountOfAvailableToCreateSessions(Guid userId, int count = 1)
        {
            var filter = Builders<UserEntity>.Filter
                .Eq(user => user.Id, userId);
            var update = Builders<UserEntity>.Update
                .Inc(user => user.CountSessionsAvailableToCreate, count);

            await _usersCollection.FindOneAndUpdateAsync(filter, update);
        }

        public async Task<UserEntity?> GetByIdAsync(Guid id) =>
            await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();


        public async Task<UserEntity?> GetByLoginAsync(string login) =>
            await _usersCollection.Find(x => x.Login == login).FirstOrDefaultAsync();


        public async Task AddSessionToUserAsync(Guid id, UserOrgSessionEntity orgSessionEntity)
        {
            var filter = Builders<UserEntity>.Filter.Eq(x => x.Id, id);
            var update = Builders<UserEntity>.Update.Push(x => x.OrgSessions, orgSessionEntity);

            await _usersCollection.UpdateOneAsync(filter, update);
        }

        public async Task<ICollection<UserOrgSessionEntity>?> GetOrgSessionsAsync(Guid userId)
        {
            var user = await _usersCollection.Find(x => x.Id == userId).FirstOrDefaultAsync();

            if (user != null)
                return user.OrgSessions;

            return null;
        }

        public async Task RemoveAccessToSessionAsync(Guid sessionId, Guid userId)
        {
            var filter = Builders<UserEntity>.Filter.Eq(x => x.Id, userId);
            var delete = Builders<UserEntity>.Update.PullFilter(user => user.OrgSessions, session => session.Id == sessionId);

            await _usersCollection.UpdateOneAsync(filter, delete);
        }

        public async Task<int> GetCountActiveSessionByUserIdAsync(Guid userId)
        {
            var user = await _usersCollection
                .Find(x => x.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null || user.OrgSessions == null || !user.OrgSessions.Any())
                return 0;

            var sessionIds = user.OrgSessions.Select(s => s.Id).ToList();

            var filter = Builders<SessionEntity>.Filter.And(
                Builders<SessionEntity>.Filter.In(x => x.Id, sessionIds),
                Builders<SessionEntity>.Filter.Ne(x => x.Start, null),
                Builders<SessionEntity>.Filter.Eq(x => x.End, null)
            );

            var result = await _sessionsCollection.CountDocumentsAsync(filter);
            return (int)result;
        }

        public async Task VerifyAsync(Guid userId)
        {
            var filter = Builders<UserEntity>.Filter.Eq(x => x.Id, userId);
            var update = Builders<UserEntity>.Update.Set(x => x.Verifyed, true);

            await _usersCollection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateSessionToUserAsync(Guid userId, UserOrgSessionEntity orgSessionEntity)
        {
            var filter = Builders<UserEntity>.Filter.Where(x => x.Id == userId && x.OrgSessions.Any(i => i.Id == orgSessionEntity.Id));
            var update = Builders<UserEntity>.Update.Set(x => x.OrgSessions.FirstMatchingElement(), orgSessionEntity);

            await _usersCollection.UpdateOneAsync(filter, update);
        }

        public async Task UpdatePasswordAsync(Guid userId, string password)
        {
            var filter = Builders<UserEntity>.Filter.Eq(x => x.Id, userId);
            var update = Builders<UserEntity>.Update.Set(x => x.Pass, password);

            await _usersCollection.UpdateOneAsync(filter, update);
        }
    }
}
