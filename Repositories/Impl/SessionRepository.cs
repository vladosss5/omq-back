using AskAgainApi.Entity.Session;
using AskAgainApi.Entity.Session.Poll;
using AskAgainApi.Entity.Session.Question;
using AskAgainApi.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace AskAgainApi.Repositories.Impl
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IMongoCollection<SessionEntity> _sessionsCollection;

        public SessionRepository(
            IOptions<DatabaseSettings> AskAgainDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                AskAgainDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                AskAgainDatabaseSettings.Value.DatabaseName);

            _sessionsCollection = mongoDatabase.GetCollection<SessionEntity>(
                AskAgainDatabaseSettings.Value.SessionsCollectionName);
        }

        public async Task<List<SessionEntity>> GetAsync() =>
            await _sessionsCollection.Find(_ => true).ToListAsync();

        public async Task<SessionEntity?> GetByIdAsync(Guid id) =>
            await _sessionsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<SessionEntity?> GetByShortIdAsync(int shortId) =>
            await _sessionsCollection.Find(x => x.ShortId == shortId).FirstOrDefaultAsync();

        public async Task CreateAsync(SessionEntity newSession)
        {
            var newSessionWithShortId = newSession;
            newSessionWithShortId.ShortId = await GenerateShortId();

            await _sessionsCollection.InsertOneAsync(newSession);
        }

        private async Task<int> GenerateShortId()
        {
            Random rnd = new();
            int shortId = 0;

            bool generationIsFail = true;

            for (int i = 0; i < 10; i++)
            {
                shortId = rnd.Next(1, 1000000000);
                if (await CheckUsingShortId(shortId))
                {
                    generationIsFail = false;
                }
            }

            if (generationIsFail)
                throw new Exception("Sessions collection is overflow for current length shortId");

            return shortId;
        }

        private async Task<bool> CheckUsingShortId(int shortId)
        {
            return null == await _sessionsCollection.Find(x => x.ShortId == shortId).FirstOrDefaultAsync();
        }

        public async Task UpdateSettingAndNameAsync(Guid sessionId, SessionSettingsEntity settings, string sessionName)
        {
            var filter = Builders<SessionEntity>.Filter.Eq(x => x.Id, sessionId);
            var update = Builders<SessionEntity>.Update.Set(x => x.Name, sessionName).Set(x => x.Settings, settings);

            await _sessionsCollection.UpdateOneAsync(filter, update);
        }

        public async Task RemoveAsync(Guid id) =>
            await _sessionsCollection.DeleteOneAsync(x => x.Id == id);

        public async Task CreatePollAsync(Guid sessionId, SessionPollEntity pollEntity)
        {
            var filter = Builders<SessionEntity>.Filter.Eq(x => x.Id, sessionId);
            var update = Builders<SessionEntity>.Update.Push(x => x.Polls, pollEntity);

            await _sessionsCollection.UpdateOneAsync(filter, update);
        }

        public async Task<SessionPollEntity?> GetPollAsync(Guid sessionId, Guid pollId)
        {
            var session = await _sessionsCollection.Find(x => x.Id == sessionId).FirstOrDefaultAsync();

            return session.Polls.FirstOrDefault(x => x.Id == pollId);
        }

        public async Task RemovePollAsync(Guid sessionId, Guid pollId)
        {
            var filter = Builders<SessionEntity>.Filter.Eq(x => x.Id, sessionId);
            var delete = Builders<SessionEntity>.Update.PullFilter(session => session.Polls, poll => poll.Id == pollId);

            await _sessionsCollection.UpdateOneAsync(filter, delete);
        }

        public async Task CreateQuestionAsync(Guid sessionId, SessionQuestionEntity questionEntity)
        {
            var filter = Builders<SessionEntity>.Filter.Eq(x => x.Id, sessionId);
            var update = Builders<SessionEntity>.Update.Push(x => x.Questions, questionEntity);

            await _sessionsCollection.UpdateOneAsync(filter, update);
        }

        public async Task RemoveQuestionAsync(Guid sessionId, Guid questionId)
        {
            var filter = Builders<SessionEntity>.Filter.Eq(x => x.Id, sessionId);
            var delete = Builders<SessionEntity>.Update.PullFilter(session => session.Questions, question => question.Id == questionId);

            await _sessionsCollection.UpdateOneAsync(filter, delete);

        }
        public async Task LikeQuestionAsync(Guid userId, Guid sessionId, Guid questionId)
        {
            var filter = Builders<SessionEntity>.Filter.Where(x => x.Id == sessionId && x.Questions.Any(q => q.Id == questionId));
            var update = Builders<SessionEntity>.Update.AddToSet(x => x.Questions.FirstMatchingElement().LikesUsersId, userId);

            await _sessionsCollection.UpdateOneAsync(filter, update);
        }

        public async Task<SessionQuestionEntity?> GetQuestionAsync(Guid sessionId, Guid questionId)
        {
            var session = await _sessionsCollection.Find(x => x.Id == sessionId).FirstOrDefaultAsync();

            return session.Questions.FirstOrDefault(x => x.Id == questionId);
        }

        public async Task SetStartSessionAsync(Guid sessionId, DateTime startSession, DateTime endSession)
        {
            var filter = Builders<SessionEntity>.Filter.Eq(x => x.Id, sessionId);
            var update = Builders<SessionEntity>.Update.Set(session => session.Start, startSession)
                .Set(session => session.Status, Enums.SessionStateEnum.Started)
                .Set(session => session.End, endSession);

            await _sessionsCollection.UpdateOneAsync(filter, update);
        }
        public async Task SetEndSessionAsync(Guid sessionId, DateTime endSession)
        {
            var filter = Builders<SessionEntity>.Filter.Eq(x => x.Id, sessionId);
            var update = Builders<SessionEntity>.Update.Set(session => session.End, endSession)
                .Set(session => session.Status, Enums.SessionStateEnum.Finished);


            await _sessionsCollection.UpdateOneAsync(filter, update);
        }

        public async Task ArchiveQuestionAsync(Guid sessionId, Guid questionId)
        {
            var filter = Builders<SessionEntity>.Filter.Where(x => x.Id == sessionId && x.Questions.Any(i => i.Id == questionId));
            var update = Builders<SessionEntity>.Update.Set(x => x.Questions.FirstMatchingElement().IsActive, false);

            await _sessionsCollection.UpdateOneAsync(filter, update);
        }
        public async Task UnarchiveQuestionAsync(Guid sessionId, Guid questionId)
        {
            var filter = Builders<SessionEntity>.Filter.Where(x => x.Id == sessionId && x.Questions.Any(i => i.Id == questionId));
            var update = Builders<SessionEntity>.Update.Set(x => x.Questions.FirstMatchingElement().IsActive, true);

            await _sessionsCollection.UpdateOneAsync(filter, update);
        }

        public async Task DislikeQuestionAsync(Guid userId, Guid sessionId, Guid questionId)
        {
            var filter = Builders<SessionEntity>.Filter.Where(x => x.Id == sessionId && x.Questions.Any(q => q.Id == questionId));
            var update = Builders<SessionEntity>.Update.Pull(x => x.Questions.FirstMatchingElement().LikesUsersId, userId);

            await _sessionsCollection.UpdateOneAsync(filter, update);
        }
    }
}
