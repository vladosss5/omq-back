using AskAgainApi.Entity.PollResult;
using AskAgainApi.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace AskAgainApi.Repositories.Impl
{
    public class PollResultRepository : IPollResultRepository
    {
        private readonly IMongoCollection<PollResultEntity> _pollResultsCollection;

        public PollResultRepository(IOptions<DatabaseSettings> AskAgainDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                AskAgainDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                AskAgainDatabaseSettings.Value.DatabaseName);

            _pollResultsCollection = mongoDatabase.GetCollection<PollResultEntity>(
                AskAgainDatabaseSettings.Value.PollResultCollectionName);
        }

        public async Task CreateAsync(PollResultEntity pollResult) =>
            await _pollResultsCollection.InsertOneAsync(pollResult);


        public async Task RemoveAsync(Guid id) =>
            await _pollResultsCollection.DeleteOneAsync(x => x.PollId == id);


        public async Task AddUserToOptionAsync(Guid userId, Guid sessionId, Guid pollId, Guid optionId)
        {
            var filterBySession = Builders<PollResultEntity>.Filter
                .Eq(pollResult => pollResult.SessionId, sessionId);
            var filterByPoll = Builders<PollResultEntity>.Filter
                .Eq(pollResult => pollResult.PollId, pollId);
            var filterByOption = Builders<PollResultEntity>.Filter
                .ElemMatch(pollResult => pollResult.Options, option => option.Id == optionId);

            var filter = Builders<PollResultEntity>.Filter.And(filterBySession, filterByPoll, filterByOption);

            var update = Builders<PollResultEntity>.Update
                .AddToSet(pollResult => pollResult.Options.FirstMatchingElement().Users, userId);

            await _pollResultsCollection.UpdateOneAsync(filter, update);
        }

        public async Task<PollResultEntity?> GetByPollIdAsync(Guid pollId) =>
            await _pollResultsCollection.Find(x => x.PollId == pollId).FirstOrDefaultAsync();

        public async Task<ICollection<PollResultEntity>> GetBySessionIdAsync(Guid sessionId) =>
            await _pollResultsCollection.Find(x => x.SessionId == sessionId).ToListAsync();

        public async Task SetStartTime(Guid pollId, DateTime startTime)
        {
            var filter = Builders<PollResultEntity>.Filter.Eq(x => x.PollId, pollId);
            var update = Builders<PollResultEntity>.Update.Set(x => x.Start, startTime);

            await _pollResultsCollection.UpdateOneAsync(filter, update);
        }

        public async Task SetEndTime(Guid pollId, DateTime? endTime)
        {
            var filter = Builders<PollResultEntity>.Filter.Eq(x => x.PollId, pollId);
            var update = Builders<PollResultEntity>.Update.Set(x => x.End, endTime);

            await _pollResultsCollection.UpdateOneAsync(filter, update);
        }
    }
}
