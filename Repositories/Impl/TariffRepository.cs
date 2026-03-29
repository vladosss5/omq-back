using AskAgainApi.Entity.Tariff;
using AskAgainApi.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AskAgainApi.Repositories.Impl
{
    public class TariffRepository : ITariffRepository
    {
        private readonly IMongoCollection<TariffEntity> _tariffsCollection;

        public TariffRepository(
            IOptions<DatabaseSettings> AskAgainDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                AskAgainDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                AskAgainDatabaseSettings.Value.DatabaseName);

            _tariffsCollection = mongoDatabase.GetCollection<TariffEntity>(
                AskAgainDatabaseSettings.Value.TariffsCollectionName);
        }

        public async Task<List<TariffEntity>> GetAsync() =>
            await _tariffsCollection.Find(_ => true).ToListAsync();

        public async Task<TariffEntity?> GetAsync(Guid id) =>
            await _tariffsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(TariffEntity newTariff) =>
            await _tariffsCollection.InsertOneAsync(newTariff);

        public async Task UpdateAsync(Guid id, TariffEntity updatedUser) =>
            await _tariffsCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveAsync(Guid id) =>
            await _tariffsCollection.DeleteOneAsync(x => x.Id == id);

    }
}
